using System;
using System.Linq;
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
            foreach (var tagType in TypedEnumOptions.GetKnownTagTypes())
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
            if (!TypedEnumOptions.TryGet(tagType, out var names, out var values))
            {
                Debug.LogError($"{tagType.Name}: could not resolve typed enum options.");
                return;
            }

            var holders = TypedEnumOptions.GetHolders(tagType);
            var holderNames = string.Join(", ", holders.Select(holder => holder.Name));

            var duplicates = names
                .Zip(values, (name, value) => (name, value))
                .GroupBy(entry => entry.value)
                .Where(group => group.Count() > 1);

            foreach (var group in duplicates)
            {
                Debug.LogError(
                    $"{tagType.Name} [{holderNames}]: duplicate value {group.Key} on fields " +
                    $"[{string.Join(", ", group.Select(entry => entry.name))}].");
            }
        }
    }
#endif
}
