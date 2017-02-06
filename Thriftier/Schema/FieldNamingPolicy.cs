namespace Thriftier.Schema
{
    public abstract class FieldNamingPolicy
    {
        public abstract string Apply(string name);

        /**
         * The default policy is to leave names unaltered from their definition in Thrift IDL.
         */
        public class DefaultFieldNamingPolicy : FieldNamingPolicy
        {
            public override string Apply(string name)
            {
                return name;
            }
        }
        public static FieldNamingPolicy Default = new DefaultFieldNamingPolicy();
    }
}