using System;

namespace HLMLabs.PartialEnum.Runtime
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class EnumHolderAttribute : Attribute
    {
        public Type TagType { get; }

        public EnumHolderAttribute(Type tagType) => TagType = tagType;
    }
}
