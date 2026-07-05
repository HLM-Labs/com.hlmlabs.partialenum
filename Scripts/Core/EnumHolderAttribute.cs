using System;

namespace HLMLabs.PartialEnum.Runtime
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class EnumHolderAttribute : Attribute
    {
        public Type HolderType { get; }
        public EnumHolderAttribute(Type holderType) => HolderType = holderType;
    }
}