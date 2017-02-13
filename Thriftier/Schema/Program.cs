using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Thriftier.Schema.Parser;

namespace Thriftier.Schema
{
    public class Program
    {
        private ThriftFileElement _element;


        public Program(ThriftFileElement element)
        {
            _element = element;
        }

        public Location Location => _element.Location;
        public IEnumerable<Program> Includes {
            get
            {
                throw new NotImplementedException();
            }}

        public ImmutableDictionary<NamespaceScope, string> Namespaces
        {
            get { throw new NotImplementedException(); }
        }

        public void LoadIncludedPrograms(Loader loader, HashSet<Program> visited)
        {
            throw new System.NotImplementedException();
        }
    }
}