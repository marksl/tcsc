using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Thriftier.Schema
{
    public class BuiltinType : ThriftType
    {
        public static ThriftType BOOL = new BuiltinType("bool");
        public static ThriftType BYTE = new BuiltinType("byte");
        public static ThriftType I8 = new BuiltinType("i8");
        public static ThriftType I16 = new BuiltinType("i16");
        public static ThriftType I32 = new BuiltinType("i32");
        public static ThriftType I64 = new BuiltinType("i64");
        public static ThriftType DOUBLE = new BuiltinType("double");
        public static ThriftType STRING = new BuiltinType("string");
        public static ThriftType BINARY = new BuiltinType("binary");
        public static ThriftType VOID = new BuiltinType("void");

        private static ImmutableDictionary<string, ThriftType> BUILTINS;

        static BuiltinType()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, ThriftType>();

            builder.Add(BOOL.Name, BOOL);
            builder.Add(BYTE.Name, BYTE);
            builder.Add(I8.Name, I8);
            builder.Add(I16.Name, I16);
            builder.Add(I32.Name, I32);
            builder.Add(I64.Name, I64);
            builder.Add(DOUBLE.Name, DOUBLE);
            builder.Add(STRING.Name, STRING);
            builder.Add(BINARY.Name, BINARY);
            builder.Add(VOID.Name, VOID);

            BUILTINS = builder.ToImmutable();
        }

        public static ThriftType get(String name)
        {
            return BUILTINS[name];
        }

        private ImmutableDictionary<string, string> _annotations;

        public BuiltinType(string name)
            :
            this(name, ImmutableDictionary.Create<string, string>())

        {


        }

        BuiltinType(string name, ImmutableDictionary<string, string> annotations)
            : base(name)
        {
            this._annotations = annotations;
        }


        public override T Accept<T>(Visitor<T> visitor)
        {
            if (this.Equals(BOOL))
            {
                return visitor.visitBool(this);
            }
            else if (this.Equals(BYTE) || this.Equals(I8))
            {
                return visitor.visitByte(this);
            }
            else if (this.Equals(I16))
            {
                return visitor.visitI16(this);
            }
            else if (this.Equals(I32))
            {
                return visitor.visitI32(this);
            }
            else if (this.Equals(I64))
            {
                return visitor.visitI64(this);
            }
            else if (this.Equals(DOUBLE))
            {
                return visitor.visitDouble(this);
            }
            else if (this.Equals(STRING))
            {
                return visitor.visitString(this);
            }
            else if (this.Equals(BINARY))
            {
                return visitor.visitBinary(this);
            }
            else
            {
                throw new Exception("Unexpected ThriftType: " + Name);
            }
        }

        public override bool IsBuiltin()
        {
            return true;
        }

        public override ThriftType WithAnnotations(Dictionary<string, string> annotations)
        {
            return new BuiltinType(Name, merge(this._annotations, annotations));
        }

        public override ImmutableDictionary<string, string> Annotations()
        {
            return _annotations;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (o == null) return false;
            if (GetType() != o.GetType()) return false;

            BuiltinType that = (BuiltinType) o;

            if (Name.Equals(that.Name))
            {
                return true;
            }

            // 'byte' and 'i8' are synonyms
            if (this.Name.Equals(BYTE.Name) && that.Name.Equals(I8.Name))
            {
                return true;
            }

            if (this.Name.Equals(I8.Name) && that.Name.Equals(BYTE.Name))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            String name = Name;
            if (name.Equals(I8.Name))
            {
                name = BYTE.Name;
            }
            return name.GetHashCode();
        }
    }
}