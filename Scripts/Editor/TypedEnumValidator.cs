using System;
using System.Linq;
using System.Reflection;
using HLMLabs.PartialEnum.Runtime;
using UnityEditor;
using UnityEngine;

namespace HLMLabs.PartialEnum.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class TypedEnumValidator
    {
        static TypedEnumValidator()
        {
            foreach (var tagType in TypeCache.GetTypesWithAttribute<EnumHolderAttribute>())
            {
                try
                {
                    Validate(tagType);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"TypedEnum validation failed for {tagType.FullName}: {exception.Message}");
                }
            }
        }

        private static void Validate(Type tagType)
        {
            var holderAttr = tagType.GetCustomAttribute<EnumHolderAttribute>();
            if (holderAttr?.HolderType == null)
            {
                Debug.LogError($"{tagType.Name}: {nameof(EnumHolderAttribute)} must specify a non-null holder type.");
                return;
            }

            if (!TypedEnumOptions.TryGet(tagType, out var names, out var values))
            {
                Debug.LogError($"{tagType.Name}: could not resolve typed enum options.");
                return;
            }

            var duplicates = names
                .Zip(values, (name, value) => (name, value))
                .GroupBy(entry => entry.value)
                .Where(group => group.Count() > 1);

            foreach (var group in duplicates)
            {
                Debug.LogError(
                    $"{holderAttr.HolderType.Name}: duplicate value {group.Key} on fields " +
                    $"[{string.Join(", ", group.Select(entry => entry.name))}].");
            }
        }
    }
#endif
}
