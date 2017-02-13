using System;

namespace Thriftier.Schema.Parser
{
    public class TypedefElement
    {
        public string Name { get; }
        public Location Location { get; }
        public string Documentation { get; }
        public virtual AnnotationElement Annotations { get; }

        public string newName()
        {
            throw new NotImplementedException();
        }
    }
}