using HLMLabs.PartialEnum.Runtime;

namespace HLMLabs.PartialEnum.Samples
{
    /// <summary>
    /// The tag type. It identifies the enum and declares the values owned by this assembly.
    /// Values 0-999 are reserved here so other assemblies can extend from 1000 upwards.
    /// </summary>
    [EnumHolder(typeof(EnumCustomType))]
    public struct EnumCustomType
    {
        public static readonly TypedEnum<EnumCustomType> Default = new(0);
        public static readonly TypedEnum<EnumCustomType> ValueA = new(1);
    }
}
