using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Thriftier.Schema.Parser;

namespace Thriftier.Schema
{
    public class NoSuchElementException : Exception
    {

    }

    public class EnumType : UserType
    {
    private readonly ImmutableList<EnumMember> _members;

    public EnumType(Program program, EnumElement element)
    : base(program, new UserElementMixin(element))
    {

        List<EnumMember> members = new List<EnumMember>();
        foreach (var m in element.Members)
        {
            members.Add(new EnumMember(m));
        }

        _members = members.ToImmutableList();
    }

//    private EnumType(Builder builder) {
//        super(builder);
//        this.members = builder.members;
//    }

        public ImmutableList<EnumMember> Members => _members;

    public EnumMember findMemberByName(string name) {
        foreach (EnumMember member in _members) {
            if (member.Name.Equals(name)) {
                return member;
            }
        }
        throw new NoSuchElementException();
    }

    public EnumMember findMemberById(int id) {
        foreach (EnumMember member in _members) {
            if (member.Value == id) {
                return member;
            }
        }
        throw new NoSuchElementException();
    }

        public override T Accept<T>(Visitor<T> visitor)
        {
            return visitor.visitEnum(this);
        }

        public override bool IsEnum()
        {
            return true;
        }

        public override ThriftType WithAnnotations(Dictionary<string, string> annotations)
        {
            throw new NotImplementedException();
        }

//    @Override
//    public ThriftType withAnnotations(Map<String, String> annotations) {
//        return toBuilder()
//            .annotations(merge(this.annotations(), annotations))
//            .build();
//    }

//    public Builder toBuilder() {
//        return new Builder(this);
//    }
//
//    public static class Builder extends UserType.UserTypeBuilder<EnumType, Builder> {
//    private ImmutableList<EnumMember> members;
//
//    Builder(EnumType enumType) {
//        super(enumType);
//        this.members = enumType.members;
//    }
//
//    public Builder members(List<EnumMember> members) {
//        Preconditions.checkNotNull(members, "members");
//        this.members = ImmutableList.copyOf(members);
//        return this;
//    }
//
//    @Override
//    public EnumType build() {
//        return new EnumType(this);
//    }
    }

}