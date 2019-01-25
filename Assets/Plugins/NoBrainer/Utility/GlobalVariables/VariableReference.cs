using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NoBrainer.Utility
{
    [Serializable]
    public abstract class VariableReference
    {
        public abstract bool IsGlobal { get; }
    }

    [Serializable]
    public class VariableReference<VariableType, GlobalVariableType> : VariableReference
        where GlobalVariableType : GlobalVariable<VariableType>
    {
        [SerializeField, FormerlySerializedAs("globalReference")]
        private GlobalVariableType globalVariable;

        [SerializeField]
        private VariableType localValue;

        [SerializeField]
        private bool usingGlobal;

        protected virtual void OnBecameLocal()
        { }

        protected virtual void OnBecameGlobal()
        { }

        public virtual VariableType Value
        {
            get
            {
                if (IsGlobal)
                {
                    if (globalVariable != null)
                        return globalVariable.Value;
                    else
                        return default(VariableType);
                }
                else
                {
                    return localValue;
                }
            }
            set
            {
                if (IsGlobal)
                {
                    if (globalVariable != null)
                        globalVariable.Value = value;
                    else
                        Debug.LogWarningFormat("Cannot assign value to VariableReference in Global state with unset globalReference!");
                }
                else
                {
                    localValue = value;
                }
            }
        }

        public GlobalVariableType GlobalVariable
        {
            get
            {
                return globalVariable;
            }
            set
            {
                globalVariable = value;
            }
        }

        public VariableType LocalValue
        {
            get
            {
                return localValue;
            }
            set
            {
                localValue = value;
            }
        }

        public override bool IsGlobal
        {
            get
            {
                return usingGlobal;// state == ReferenceState.Local;
            }
        }

        public static implicit operator VariableType(VariableReference<VariableType, GlobalVariableType> variable)
        {
            return variable.Value;
        }
    }
}
