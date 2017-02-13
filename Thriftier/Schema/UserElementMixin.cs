using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Thriftier.Schema.Parser;

namespace Thriftier.Schema
{
    public class UserElementMixin : UserElement
    {
    private readonly string _name;
    private readonly Location _location;
    private readonly string _documentation;
    private readonly ImmutableDictionary<string, string> _annotations;

        public UserElementMixin(StructElement s)
            : this(s.Name, s.Location, s.Documentation, s.Annotations)
        {
        }

        public UserElementMixin(FieldElement field)
            : this(field.Name, field.Location, field.Documentation, field.Annotations)
        {

        }

        public UserElementMixin(EnumElement enumElement)
            : this(enumElement.Name, enumElement.Location, enumElement.Documentation, enumElement.Annotations)
        {

        }

        public UserElementMixin(EnumMemberElement member)
            : this(member.Name, member.Location, member.Documentation, member.Annotations)
        {

        }

        public UserElementMixin(TypedefElement element)
            : this(element.newName(), element.Location, element.Documentation, element.Annotations)
        {

        }

        public UserElementMixin(ServiceElement element)
            : this(element.Name, element.Location, element.Documentation, element.Annotations)
        {
        }

        public UserElementMixin(FunctionElement element)
            : this(element.Name, element.Location, element.Documentation, element.Annotations)
        {
        }

        UserElementMixin(
        String name,
        Location location,
        String documentation,
        AnnotationElement annotationElement) {

        this._name = name;
        this._location = location;
        this._documentation = documentation;

        throw new NotImplementedException();
//        ImmutableMap.Builder<String, String> annotations = ImmutableMap.builder();
//        if (annotationElement != null) {
//            annotations.putAll(annotationElement.values());
//        }
//        this._annotations = annotations.build();
    }

        private UserElementMixin(Builder builder)
        {
            this._name = builder._name;
            this._location = builder._location;
            this._documentation = builder._documentation;
            this._annotations = builder._annotations;
        }

        public Location Location => _location;
        public string Name => _name;
        public string Documentation => _documentation;
    public ImmutableDictionary<string, string> Annotations => _annotations;


    public bool HasJavaDoc => false;

    public bool IsDeprecated => hasThriftOrJavadocAnnotation("deprecated");



    /**
     * Checks for the presence of the given annotation name, in several possible
     * varieties.  Returns true if:
     *
     * <ul>
     *     <li>A Thrift annotation matching the exact name is present</li>
     *     <li>A Thrift annotation equal to the string "thrifty." plus the name is present</li>
     *     <li>The Javadoc contains "@" plus the annotation name</li>
     * </ul>
     *
     * The latter two conditions are officially undocumented, but are present for
     * legacy use.  This behavior is subject to change without notice!
     */
        bool hasThriftOrJavadocAnnotation(string name)
        {
            return this._annotations.ContainsKey(name)
                   || _annotations.ContainsKey("thrifty." + name)
                   || (HasJavaDoc && string.Format(_documentation.ToLower(), new CultureInfo("en-US"))
                           .Contains("@" + name));
        }


        public override string ToString()
        {
            return $"UserElementMixin[name='{_name}',"
                   + $"location='{_location}', documentation='{_documentation}'"
                   + $"annotations='{_annotations}']";
        }

        protected bool Equals(UserElementMixin other)
        {
            return string.Equals(_name, other._name)
                   && Equals(_location, other._location)
                   && string.Equals(_documentation, other._documentation)
                   && Equals(_annotations, other._annotations);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserElementMixin) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_location != null ? _location.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_documentation != null ? _documentation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_annotations != null ? _annotations.GetHashCode() : 0);
                return hashCode;
            }
        }
    Builder toBuilder() {
        return new Builder(this);
    }

     class Builder {
        public string _name;
        public Location _location;
        public String _documentation;
        public ImmutableDictionary<string, string> _annotations;

        public Builder(UserElement userElement) {
            this._name = userElement.Name;
            this._location = userElement.Location;
            this._documentation = userElement.Documentation;
            this._annotations = new Dictionary<string, string>(userElement.Annotations)
                .ToImmutableDictionary();
        }

        Builder name(String name) {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            this._name = name;
            return this;
        }

        Builder location(Location location) {
            if (location==null)
                throw new ArgumentNullException(nameof(location));
            this._location = location;
            return this;
        }

        Builder documentation(String documentation) {
            //if (JavadocUtil.isNonEmptyJavadoc(documentation))
            if (!string.IsNullOrWhiteSpace(documentation))
            {
                this._documentation = documentation;
            } else {
                this._documentation = "";
            }
            return this;
        }

        Builder annotations(Dictionary<string, string> annotations) {

            foreach (var a in annotations)
            {
                this._annotations.Add(a.Key, a.Value);
            }

            return this;
        }

        UserElementMixin build() {
            return new UserElementMixin(this);
        }
    }
    }

}