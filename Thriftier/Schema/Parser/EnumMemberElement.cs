namespace Thriftier.Schema.Parser
{
    public class EnumMemberElement
    {
        public string Name { get; }
        public Location Location { get; }
        public string Documentation { get; }
        public virtual AnnotationElement Annotations { get; }

        public int Value { get; }
    }




}