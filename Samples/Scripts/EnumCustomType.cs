namespace HLMLabs.PartialEnum.Runtime
{
    [EnumHolder(typeof(EnumCustomType))]
    public partial struct EnumCustomType
    {
        public static readonly TypedEnum<EnumCustomType> Default = new(0);
        public static readonly TypedEnum<EnumCustomType> ValueA = new(1);
    }
}