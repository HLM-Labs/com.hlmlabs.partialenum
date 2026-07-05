using System;
using System.Linq;
using System.Collections.Generic;
using HLMLabs.PartialEnum.Runtime;
using UnityEditor;
using UnityEngine;

namespace HLMLabs.PartialEnum.Editor
{
    [CustomPropertyDrawer(typeof(TypedEnum<>))]
    public class TypedEnumDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagType = ResolveTagType(fieldInfo.FieldType);
            var valueProp = property.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, property);

            if (tagType == null)
            {
                EditorGUI.PropertyField(position, valueProp, label);
                EditorGUI.EndProperty();
                return;
            }

            if (!TypedEnumOptions.TryGet(tagType, out var names, out var values))
            {
                EditorGUI.PropertyField(position, valueProp, label);
                EditorGUI.EndProperty();
                return;
            }

            int currentValue = valueProp.intValue;
            int currentIndex = Array.IndexOf(values, currentValue);

            string[] displayNames = names;
            int[] displayValues = values;

            if (currentIndex < 0)
            {
                displayNames = names.Concat(new[] { $"Unknown ({currentValue})" }).ToArray();
                displayValues = values.Concat(new[] { currentValue }).ToArray();
                currentIndex = displayNames.Length - 1;
            }

            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, displayNames);
            if (newIndex != currentIndex)
                valueProp.intValue = displayValues[newIndex];

            EditorGUI.EndProperty();
        }

        private static Type ResolveTagType(Type fieldType)
        {
            var elementType = fieldType;

            if (elementType.IsArray)
                elementType = elementType.GetElementType();
            else if (elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(List<>))
                elementType = elementType.GetGenericArguments()[0];

            if (elementType != null &&
                elementType.IsGenericType &&
                elementType.GetGenericTypeDefinition() == typeof(TypedEnum<>))
            {
                return elementType.GetGenericArguments()[0];
            }

            return null;
        }
    }
}
