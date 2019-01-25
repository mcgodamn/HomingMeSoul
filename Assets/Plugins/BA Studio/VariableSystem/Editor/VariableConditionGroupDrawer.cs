using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace BA_Studio.UnityLib.VariableSystem
{
    [CustomPropertyDrawer (typeof (VariableConditionGroup))]
    public class VariableConditionGroupDrawer : PropertyDrawer {
 
        Rect typeRect, itemsRect, addRect, removeRect;

        ReorderableList list;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float temp = 54;
            for (int i = 0; i < property.FindPropertyRelative("conditions").arraySize; i++) {
                temp += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions").GetArrayElementAtIndex(i), GUIContent.none, true) + ((property.FindPropertyRelative("conditions").arraySize > 10)? 2 : 1);
            }
            return temp;
        }

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            property.serializedObject.Update();
            EditorGUI.BeginProperty (position, label, property);
            typeRect.position = position.position;
            typeRect.height = 16;
            typeRect.width = position.width;
            itemsRect = new Rect(typeRect.xMin, typeRect.yMax, position.width, EditorGUI.GetPropertyHeight(property.FindPropertyRelative("conditions")));
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("groupType"));
            EditorGUI.PropertyField(itemsRect, property.FindPropertyRelative("conditions"), true);
            property.FindPropertyRelative("conditions").isExpanded = true;
            EditorGUI.EndProperty();
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
    
}