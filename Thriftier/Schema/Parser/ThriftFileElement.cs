using System.Collections.Immutable;
using System.Reflection;

namespace Thriftier.Schema.Parser
{
    public abstract class ThriftFileElement
    {
        public Location Location { get; set; }
        public ImmutableList<IncludeElement> Includes { get; set; }
    }
}