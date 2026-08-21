using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HLMLabs.PartialEnum.Runtime
{
    public static class TypedEnumOptions
    {
        private static readonly object Sync = new();
        private static readonly Dictionary<Type, (string[] names, int[] values)> Cache = new();
        private static readonly HashSet<Type> InvalidTags = new();
        private static Dictionary<Type, List<Type>> _holdersByTag;

        public static bool TryGet(Type tagType, out string[] names, out int[] values)
        {
            names = Array.Empty<string>();
            values = Array.Empty<int>();

            if (tagType == null)
                return false;

            lock (Sync)
            {
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
        }

        private static bool TryBuild(Type tagType, out string[] names, out int[] values)
        {
            names = Array.Empty<string>();
            values = Array.Empty<int>();

            var enumType = typeof(TypedEnum<>).MakeGenericType(tagType);
            var valueProperty = enumType.GetProperty(nameof(TypedEnum<int>.Value));
            if (valueProperty == null)
                return false;

            var holders = GetHolders(tagType);
            if (holders.Count == 0)
                return false;

            var fields = holders
                .SelectMany(holder => holder.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(field => field.FieldType == enumType)
                .Select(field => (Name: field.Name, Value: (int)valueProperty.GetValue(field.GetValue(null))))
                .OrderBy(field => field.Value)
                .ToArray();

            if (fields.Length == 0)
                return false;

            names = fields.Select(field => field.Name).ToArray();
            values = fields.Select(field => field.Value).ToArray();
            return true;
        }

        internal static IReadOnlyList<Type> GetHolders(Type tagType)
        {
            lock (Sync)
            {
                EnsureHoldersIndexed();
                return _holdersByTag.TryGetValue(tagType, out var holders)
                    ? holders
                    : Array.Empty<Type>();
            }
        }

        internal static IReadOnlyCollection<Type> GetKnownTagTypes()
        {
            lock (Sync)
            {
                EnsureHoldersIndexed();
                return _holdersByTag.Keys.ToArray();
            }
        }

        private static void EnsureHoldersIndexed()
        {
            if (_holdersByTag != null)
                return;

            var holdersByTag = new Dictionary<Type, List<Type>>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!ShouldScan(assembly))
                    continue;

                foreach (var type in GetLoadableTypes(assembly))
                {
                    var attr = type.GetCustomAttribute<EnumHolderAttribute>();
                    if (attr?.TagType == null)
                        continue;

                    if (!holdersByTag.TryGetValue(attr.TagType, out var holders))
                    {
                        holders = new List<Type>();
                        holdersByTag[attr.TagType] = holders;
                    }

                    holders.Add(type);
                }
            }

            _holdersByTag = holdersByTag;
        }

        private static bool ShouldScan(Assembly assembly)
        {
            if (assembly.IsDynamic)
                return false;

            var name = assembly.GetName().Name;
            if (string.IsNullOrEmpty(name))
                return false;

            return !(name.StartsWith("System", StringComparison.Ordinal)
                     || name.StartsWith("Microsoft", StringComparison.Ordinal)
                     || name.StartsWith("Unity.", StringComparison.Ordinal)
                     || name.StartsWith("UnityEngine", StringComparison.Ordinal)
                     || name.StartsWith("UnityEditor", StringComparison.Ordinal)
                     || name.StartsWith("mscorlib", StringComparison.Ordinal)
                     || name.StartsWith("netstandard", StringComparison.Ordinal)
                     || name.StartsWith("Mono.", StringComparison.Ordinal));
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(type => type != null);
            }
        }
    }
}
