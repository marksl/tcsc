using System.Collections.Immutable;

namespace Thriftier.Schema.Parser
{
    public class FieldElement
    {
        public string Name { get; }
        public Location Location { get; }
        public string Documentation { get; }
        public virtual AnnotationElement Annotations { get; }    }
}