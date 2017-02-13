using System.Collections.Immutable;

namespace Thriftier.Schema.Parser
{
    public class EnumElement
    {
        public string Name { get; }
        public Location Location { get; }
        public string Documentation { get; }
        public virtual AnnotationElement Annotations { get; }

        public ImmutableList<EnumMemberElement> Members { get; set; }

    }
}