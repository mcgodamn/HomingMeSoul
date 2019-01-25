using System;
using System.Reflection;
using UnityEngine;

namespace NoBrainer.Utility
{

    public static partial class ReflectionUtility
    {
        public static T FindFieldByPath<T>(object subject, string propertyPath)
        {
            if (!propertyPath.Contains("."))
            {
                return FindFieldByName<T>(subject, propertyPath);
            }
            else
            {
                string[] splitPropertyPath = propertyPath.Split('.');

                object currentSubject = subject;

                foreach (string propertyName in splitPropertyPath)
                {
                    currentSubject = FindFieldByName<object>(currentSubject, propertyName);
                }

                return (T)currentSubject;
            }
        }

        public static T FindFieldByName<T>(object subject, string fieldName)
        {
            FieldInfo field = FindFieldInfoByName(subject.GetType(), fieldName);

            if (field != null)
            {
                T refValue = (T)field.GetValue(subject);

                if (refValue != null)
                {
                    return refValue;
                }
                else
                {
                    Debug.LogWarningFormat("Could not cast field '{0}' to type '{1}'. Is of type '{2}'.",
                        fieldName, typeof(T).FullName, field.FieldType.FullName);
                    return default(T);
                }
            }
            else
            {
                Debug.LogWarningFormat("Could not find field with name '{0}' on type '{1}'.",
                    fieldName, subject.GetType().FullName);
                return default(T);
            }
        }

        public static FieldInfo FindFieldInfoByName(Type type, string fieldName)
        {
            BindingFlags searchFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

            FieldInfo fieldInfo = type.GetField(fieldName, searchFlags);

            if (fieldInfo != null)
            {
                return fieldInfo;
            }
            else if (type.BaseType != typeof(object))
            {
                return FindFieldInfoByName(type.BaseType, fieldName);
            }
            else
            {
                return null;
            }
        }
    }

}