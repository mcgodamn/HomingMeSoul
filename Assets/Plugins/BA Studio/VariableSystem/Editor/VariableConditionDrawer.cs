using UnityEngine;
using UnityEditor;

namespace BA_Studio.UnityLib.VariableSystem
{
    [CustomPropertyDrawer (typeof (VariableConditionItem))]
    public class VariableConditionDrawer : PropertyDrawer {
 
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * (property.FindPropertyRelative("unfolded").boolValue ? 7.5f : 1) + 1;  // assuming original is one row
        }

        Rect groupIDRect, keyRect, typeRect, condRect, valueRect, useVarRect, targetGroupRect;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.

            EditorGUI.BeginProperty (position, label, property);                   
            
            position.height = 16f;
            property.FindPropertyRelative("unfolded").boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, position.width-96, position.height), property.FindPropertyRelative("unfolded").boolValue, property.FindPropertyRelative("groupID").stringValue + "/" + property.FindPropertyRelative("key").stringValue);
            EditorGUIUtility.labelWidth = 72;
            EditorGUI.PropertyField(new Rect(position.xMax - 96, position.y, 96, 20), property.FindPropertyRelative("isNegative"), new GUIContent("Negative"), true);
            EditorGUIUtility.labelWidth = 0;

            if (property.FindPropertyRelative("unfolded").boolValue){
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 2;

                int lineHeight = 20;
                
                groupIDRect = new Rect(position.x, position.y + lineHeight, position.width, lineHeight);
                keyRect = new Rect(position.x, position.y + lineHeight*2, position.width, lineHeight);
                typeRect = new Rect(position.x, position.y + lineHeight*3, position.width, lineHeight);
                condRect = new Rect(position.x + position.width/2 + 24, position.y + lineHeight*3, position.width/2 - 24, lineHeight);
                valueRect = new Rect(position.x, position.y + lineHeight*5, position.width, lineHeight);
                useVarRect = new Rect(position.x + position.width - 72, position.y + lineHeight*4, 48, lineHeight);
                targetGroupRect = new Rect(position.x, position.y + lineHeight*4, position.width - 48, lineHeight);

                EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), true);
                EditorGUI.PropertyField(groupIDRect , property.FindPropertyRelative("groupID"), true);
                
                property.FindPropertyRelative("targetVar").boolValue = EditorGUI.Toggle(useVarRect, property.FindPropertyRelative("targetVar").boolValue);

                switch(property.FindPropertyRelative ("type").enumNames[property.FindPropertyRelative ("type").enumValueIndex]){
                    case "Float":
                        typeRect = new Rect(position.x, position.y + lineHeight*3, position.width/2 + 42, lineHeight);

                        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), new GUIContent("Type/Condition"), true);
                        
                        if (property.FindPropertyRelative("targetVar").boolValue) {
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("targetKey"), true);
                        }
                        else {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.EndDisabledGroup();
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("vFloat"), true);
                        }
                        EditorGUI.PropertyField(condRect, property.FindPropertyRelative("cond"), GUIContent.none, true);
                        break;
                    case "String":
                        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), new GUIContent("Type"), true);

                        if (property.FindPropertyRelative("targetVar").boolValue) {
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("targetKey"), true);
                        }
                        else {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.EndDisabledGroup();
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("vString"), true);
                        }

                        break;
                    case "Bool":
                        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), new GUIContent("Type"), true);
                        
                        if (property.FindPropertyRelative("targetVar").boolValue) {
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("targetKey"), true);
                        }
                        else {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUI.PropertyField(targetGroupRect, property.FindPropertyRelative ("targetGroupID"), true);
                            EditorGUI.EndDisabledGroup();
                            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative ("vBool"), true);
                        }

                        break;
                }

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
            
        }
    }
    
}