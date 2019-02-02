using System;
using UnityEngine;

#if UNITY_EDTIOR
using UnityEditor;
#endif

namespace NoBrainer.Utility
{
    [Serializable]
    public abstract class GlobalVariable : ScriptableObject
    {
        public abstract object UntypedValue
        { get; }
    }

    [Serializable]
    public abstract class GlobalVariable<T> : GlobalVariable
    {
        [SerializeField]
        private T value;

        public override object UntypedValue
        {
            get { return value; }
        }

        public virtual T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public static implicit operator T(GlobalVariable<T> variableOfT)
        {
            return variableOfT.Value;
        }

        public T CopyVariable()
        {
            // Calling Instantiate ensures we get an actual copy of the object, rather than a 
            // reference. Unless T is a UnityEngine.Object, in which case it is also just a 
            // reference.
            // This creates and destroys a ScriptableObject.
            var globalCopy = Instantiate(this);
            T variableCopy = globalCopy.Value;
            DestroyImmediate(globalCopy);

            return variableCopy;
        }

        #region Editor Only


        [Header("Editor Only Settings"), Space]

        [Tooltip("Do not save changes made during play mode. Note: This is the default behavior for builds.")]
        [SerializeField]
        private bool readOnly = true;

        [Tooltip("Use this area to add helpful developer notes.")]
        [SerializeField, Multiline]        
        private string description;

        private T originalValue;
        private bool backupCreated = false;

#if UNITY_EDTIOR
        public void OnEnable()
        {
            // If this is an asset stored on the disk, we make a backup of it's values.
            // We do this only for objects that are persistent, else we enter an infinite loop,
            // as OnEnable would be called on the instantiated, transient object as well.
            if (UnityEditor.EditorUtility.IsPersistent(this))
            {
                SubscribeToPlaymodeChanges();

                if (!backupCreated)
                {
                    originalValue = CopyVariable();
                    backupCreated = true;
                }
            }

        }

        private void SubscribeToPlaymodeChanges()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlaymodeChange;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlaymodeChange;
        }

        public void OnPlaymodeChange(UnityEditor.PlayModeStateChange stateChange)
        {
            if (readOnly && stateChange == UnityEditor.PlayModeStateChange.EnteredEditMode)
            {
                value = originalValue;
                UnityEditor.EditorApplication.playModeStateChanged -= OnPlaymodeChange;
            }
        }

#endif

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool BackupCreated
        {
            get { return backupCreated; }
        }

        #endregion
    }
}
