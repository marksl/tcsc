using System.Collections.Immutable;

namespace Thriftier.Schema.Parser
{
    public class EnumMember : UserElement
    {
        private readonly UserElementMixin _mixin;

        public EnumMember(EnumMemberElement element)
        {
            this._mixin = new UserElementMixin(element);
            this.Value = element.Value;
        }

//        private EnumMember(Builder builder)
//        {
//            this._mixin = builder.mixin;
//            this.Value = builder.value;
//        }

        public int Value { get; }

        public string Name => _mixin.Name;

        public Location Location => _mixin.Location;

        public string Documentation => _mixin.Documentation;

        public ImmutableDictionary<string, string> Annotations => _mixin.Annotations;

        public bool HasJavaDoc => _mixin.HasJavaDoc;

        public bool IsDeprecated => _mixin.IsDeprecated;

        public override string ToString()
        {
            return Name;
        }

//    public static class Builder extends AbstractUserElementBuilder<EnumMember, Builder> {
//    private int value;
//
//    protected Builder(EnumMember member) {
//        super(member.mixin);
//        this.value = member.value;
//    }
//
//    public Builder value(int value) {
//        Preconditions.checkArgument(value >= 0, "Enum values cannot be less than zero");
//        this.value = value;
//        return this;
//    }
//
//    @Override
//    public EnumMember build() {
//        return new EnumMember(this);
//    }
//    }
    }

}