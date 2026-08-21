using HLMLabs.PartialEnum.Runtime;
using UnityEngine.Scripting;

namespace HLMLabs.PartialEnum.Samples.Extension
{
    /// <summary>
    /// Lives in its own assembly to show that a tag can be extended from anywhere.
    /// A holder does not have to be the tag itself, so no <c>partial</c> is involved
    /// and the assembly boundary stops being a problem.
    /// </summary>
    [Preserve]
    [EnumHolder(typeof(EnumCustomType))]
    public static class EnumCustomTypeExtension
    {
        public static readonly TypedEnum<EnumCustomType> ValueB = new(1000);
        public static readonly TypedEnum<EnumCustomType> ValueC = new(1001);
    }
}
