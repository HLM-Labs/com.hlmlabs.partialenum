using System;
using UnityEngine;

namespace HLMLabs.PartialEnum.Runtime
{
    [Serializable]
    public struct TypedEnum<TTag> : IEquatable<TypedEnum<TTag>>
    {
        [SerializeField] private int value;

        public int Value => value;

        public TypedEnum(int value) => this.value = value;

        public bool Equals(TypedEnum<TTag> other) => value == other.value;
        public override bool Equals(object obj) => obj is TypedEnum<TTag> other && Equals(other);
        public override int GetHashCode() => value;
        public static bool operator ==(TypedEnum<TTag> a, TypedEnum<TTag> b) => a.value == b.value;
        public static bool operator !=(TypedEnum<TTag> a, TypedEnum<TTag> b) => !(a == b);

        public override string ToString()
        {
            if (TypedEnumOptions.TryGet(typeof(TTag), out var names, out var values))
            {
                var index = Array.IndexOf(values, value);
                if (index >= 0)
                    return names[index];
            }

            return value.ToString();
        }
    }
}
