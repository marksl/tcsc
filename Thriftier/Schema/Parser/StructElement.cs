using System;
using System.Collections.Immutable;

namespace Thriftier.Schema.Parser
{
    public enum StructElementType {
        STRUCT,
        UNION,
        EXCEPTION
    }

    public abstract class StructElement {
        public Location Location { get; set; }
        public string Documentation { get; set; }
        public StructElementType Type { get; set; }
        public string Name { get; set; }
        //public ImmutableList<FieldElement> Fields { get; set; }

        //public abstract AnnotationElement annotations();
        public virtual AnnotationElement Annotations { get; }

        // todo

        public static Builder builder(Location location)
        {
            return new Builder()
                .location(location)
                .documentation(string.Empty);
        }

        StructElement() { }

        public class Builder {
            StructElement _s  = new StructElement();
            public Builder location(Location location)
            {
                _s.Location = location;
                return this;
            }

            public Builder documentation(String documentation)
            {
                _s.Documentation = documentation;
                return this;
            }

            public Builder type(StructElementType type)
            {
                _s.Type = type;
                return this;
            }

            public Builder name(String name)
            {
                _s.Name = name;
                return this;
            }
            //Builder fields(ImmutableList<FieldElement> fields);
            //Builder annotations(AnnotationElement annotations);

            public StructElement build()
            {
                return _s;
            }
        }

    }

}