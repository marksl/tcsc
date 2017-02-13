using System;
using System.Collections.Immutable;
using Thriftier.Schema.Parser;

namespace Thriftier.Schema
{
    public abstract class UserType : ThriftType, UserElement
    {
    private readonly ImmutableDictionary<NamespaceScope, String> _namespaces;
    private readonly UserElementMixin mixin; // visible for subtype builders

    protected UserType(Program program, UserElementMixin mixin)
     : base(mixin.Name)
    {
        this._namespaces = program.Namespaces;
        this.mixin = mixin;
    }

//    protected UserType(UserTypeBuilder<? extends UserType, ? extends UserTypeBuilder<?, ?>> builder) {
//        super(builder.mixin.name());
//        this.namespaces = builder.namespaces;
//        this.mixin = builder.mixin;
//    }

    public String getNamespaceFor(NamespaceScope n) {
        String ns = NamespaceScopeMapping.forThriftName(n);
        if (ns == null && n != NamespaceScope.ALL) {
            ns = NamespaceScopeMapping.forThriftName(NamespaceScope.ALL);
        }

        return ns;
    }

    public ImmutableDictionary<NamespaceScope, String> Namespaces => _namespaces;


    public  Location Location => mixin.location();


    public string Documentation => mixin.documentation();

        public override ImmutableDictionary<string, string> Annotations => mixin.annotations();


        public bool HasJavaDoc { get { return mixin.HasJavaDoc; } }
        public bool IsDeprecated { get { return mixin.IsDeprecated; } }


        protected bool Equals(UserType other)
        {
            return base.Equals(other) && Equals(_namespaces, other._namespaces) && Equals(mixin, other.mixin);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_namespaces != null ? _namespaces.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mixin != null ? mixin.GetHashCode() : 0);
                return hashCode;
            }
        }


// wtf
//    abstract static class UserTypeBuilder<
//        TType extends UserType,
//        TBuilder extends UserTypeBuilder<TType, TBuilder>> extends AbstractUserElementBuilder<TType, TBuilder> {
//
//    private ImmutableMap<NamespaceScope, String> namespaces;
//
//    UserTypeBuilder(TType type) {
//        super(((UserType) type).mixin);
//        this.namespaces = ((UserType) type).namespaces;
//    }
//
//    @SuppressWarnings("unchecked")
//    TBuilder namespaces(ImmutableMap<NamespaceScope, String> namespaces) {
//        this.namespaces = Preconditions.checkNotNull(namespaces, "namespaces");
//        return (TBuilder) this;
//    }
//    }
}
}