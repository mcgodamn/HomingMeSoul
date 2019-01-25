using UnityEditor;
using UnityEngine;

namespace NoBrainer.Utility
{
    [CustomPropertyDrawer(typeof(GameEvent), true)]
    public class GameEventDrawer : PropertyDrawer
    {
        private static GUIContent buttonContent = new GUIContent("Raise Event", "Only works in playmode!");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GameEvent gameEvent = ReflectionUtility.FindFieldByPath<GameEvent>(property.serializedObject.targetObject, property.propertyPath);
            SerializedProperty eventArgs = property.FindPropertyRelative("inspectorArgs");

            Rect eventArgsPosition;
            Rect buttonPosition;

            if (eventArgs != null)
            {
                eventArgsPosition = position;
                eventArgsPosition.height = EditorGUI.GetPropertyHeight(eventArgs, eventArgs.isExpanded);

                buttonPosition = eventArgsPosition;
                buttonPosition.y = eventArgsPosition.yMax + EditorGUIUtility.standardVerticalSpacing;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;
            }
            else
            {
                eventArgsPosition = position;
                eventArgsPosition.height = 0.0f;

                buttonPosition = eventArgsPosition;
                buttonPosition.y = eventArgsPosition.yMax;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;
            }

            if (eventArgs != null)
            {
                EditorGUI.PropertyField(eventArgsPosition, eventArgs, new GUIContent(eventArgs.type.ToString()), true);
            }

            GUI.enabled = Application.isPlaying;

            if (GUI.Button(buttonPosition, buttonContent))
            {
                gameEvent.RaiseEvent();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0.0f;

            SerializedProperty eventArgs = property.FindPropertyRelative("inspectorArgs");

            if (eventArgs != null)
            {
                height += EditorGUI.GetPropertyHeight(eventArgs, eventArgs.isExpanded) + EditorGUIUtility.standardVerticalSpacing;
            }

            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
    }

}