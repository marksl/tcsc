using System.Collections.Generic;

namespace Thriftier.Schema
{
    public enum NamespaceScope
    {
        ALL,
        CPP,
        JAVA,
        PY,
        PY_TWISTED,
        PERL,
        RB,
        COCOA,
        CSHARP,
        PHP,
        SMALLTALK_CATEGORY,
        SMALLTALK_PREFIX,
        C_GLIB,
        GO,
        LUA,
        ST,
        DELPHI,
        JAVASCRIPT,
        UNKNOWN
    }

    public class NamespaceScopeMapping
    {
        static Dictionary<string, NamespaceScope> _mapping = new Dictionary<string, NamespaceScope>()
        {
            {"*", NamespaceScope.ALL},
            {"cpp", NamespaceScope.CPP},
            {"java", NamespaceScope.JAVA},
            {"py", NamespaceScope.PY},
            {"py.twisted", NamespaceScope.PY_TWISTED},
            {"perl", NamespaceScope.PERL},
            { "rb", NamespaceScope.RB},
            {"cocoa" ,NamespaceScope.COCOA},
            {"csharp", NamespaceScope.CSHARP},
            {"php", NamespaceScope.PHP},
            {"smalltalk.category", NamespaceScope.SMALLTALK_CATEGORY},
            {"smalltalk.prefix", NamespaceScope.SMALLTALK_PREFIX},
            {"cglib" ,NamespaceScope.C_GLIB},
            {"go", NamespaceScope.GO},
            {"lua", NamespaceScope.LUA},
            {"st", NamespaceScope.ST},
            {"delphi", NamespaceScope.DELPHI},
            {"js", NamespaceScope.JAVASCRIPT},
            {"none", NamespaceScope.UNKNOWN}
        };

        public static NamespaceScope forThriftName(string name)
        {
            NamespaceScope scope = NamespaceScope.UNKNOWN;
            _mapping.TryGetValue(name, out scope);
            return scope;
        }


        public static string forThriftName(NamespaceScope scope)
        {
            foreach (var k in _mapping)
            {
                if (k.Value == scope)
                    return k.Key;
            }

            return null;
        }


    }
}