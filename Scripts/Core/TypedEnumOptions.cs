using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HLMLabs.PartialEnum.Runtime
{
    public static class TypedEnumOptions
    {
        private static readonly Dictionary<Type, (string[] names, int[] values)> Cache = new();
        private static readonly HashSet<Type> InvalidTags = new();

        public static bool TryGet(Type tagType, out string[] names, out int[] values)
        {
            names = Array.Empty<string>();
            values = Array.Empty<int>();

            if (tagType == null)
                return false;

            if (InvalidTags.Contains(tagType))
                return false;

            if (Cache.TryGetValue(tagType, out var cached))
            {
                names = cached.names;
                values = cached.values;
                return true;
            }

            if (!TryBuild(tagType, out names, out values))
            {
                InvalidTags.Add(tagType);
                return false;
            }

            Cache[tagType] = (names, values);
            return true;
        }

        private static bool TryBuild(Type tagType, out string[] names, out int[] values)
        {
            names = Array.Empty<string>();
            values = Array.Empty<int>();

            var holderAttr = tagType.GetCustomAttribute<EnumHolderAttribute>();
            if (holderAttr?.HolderType == null)
                return false;

            var enumType = typeof(TypedEnum<>).MakeGenericType(tagType);
            var valueProperty = enumType.GetProperty(nameof(TypedEnum<int>.Value));
            if (valueProperty == null)
                return false;

            var fields = holderAttr.HolderType
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == enumType)
                .Select(f => (Name: f.Name, Value: (int)valueProperty.GetValue(f.GetValue(null))))
                .OrderBy(f => f.Value)
                .ToArray();

            names = fields.Select(f => f.Name).ToArray();
            values = fields.Select(f => f.Value).ToArray();
            return true;
        }
    }
}
