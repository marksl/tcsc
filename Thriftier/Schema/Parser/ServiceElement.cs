namespace Thriftier.Schema.Parser
{
    public class ServiceElement
    {
        public string Name { get; }
        public Location Location { get; }
        public string Documentation { get; }
        public virtual AnnotationElement Annotations { get; }
    }
}