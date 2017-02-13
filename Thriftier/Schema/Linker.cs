using System;
using System.Collections.Generic;
using System.IO;

namespace Thriftier.Schema
{
    public class LinkFailureException : Exception
    {

    }

    public class Linker
    {
        private readonly LinkEnvironment _environment;
        private readonly Program _program;
        private readonly ErrorReporter _reporter;

        private bool linking = false;
        private bool linked = false;

        private readonly Dictionary<string, ThriftType> typesByName = new LinkedHashMap<>();


        public Linker(LinkEnvironment linkEnvironment, Program program, ErrorReporter errorReporter)
        {
            _environment = linkEnvironment;
            _program = program;
            _reporter = errorReporter;
        }

        public void Link()
        {
// Not sure abot this.
//            if (!Thread.holdsLock(environment)) {
//                throw new AssertionError("Linking must be locked on the environment!");
//            }

            if (linking) {
                _reporter.error(_program.Location, "Circular link detected; file transitively includes itself.");
                return;
            }

            if (linked) {
                return;
            }

            linking = true;

            try {
                linkIncludedPrograms();

                registerDeclaredTypes();

                // Next, figure out what types typedefs are aliasing.
                resolveTypedefs();

                // At this point, all types defined
                linkConstants();
                linkStructFields();
                linkExceptionFields();
                linkUnionFields();
                linkServices();

                // Only validate the schema if linking succeeded; no point otherwise.
                if (!reporter.hasError()) {
                    validateTypedefs();
                    validateConstants();
                    validateStructs();
                    validateExceptions();
                    validateUnions();
                    validateServices();
                }

                linked = !environment.hasErrors();
            } catch (LinkFailureException ignored) {
                // The relevant errors will have already been
                // added to the environment; just let the caller
                // handle them.
            } finally {
                linking = false;
            }
        }

        private void linkIncludedPrograms() {
            // First, link included programs and add their resolved types
            // to our own map
            foreach (Program p in _program.Includes) {
                Linker l = _environment.GetLinker(p);
                l.Link();

                string included = Path.Combine(p.Location.Base, p.Location.Path);
                //File included = new File(p.Location.Base, p.Location.Path);
                String name = new FileInfo(included).Name;
                int ix = name.IndexOf('.');
                if (ix == -1)
                {
                    string absolutePath = Path.GetFullPath(included);
                    throw new Exception(
                        "No extension found for included file " + absolutePath + ", "
                        + "invalid include statement");
                }
                String prefix = name.Substring(0, ix);

                for (Map.Entry<String, ThriftType> entry : l.typesByName.entrySet()) {
                    // Include types defined directly within the included program,
                    // but _not_ qualified names defined in programs that _it_ includes.
                    // Include-chains like top.mid.bottom.SomeType are illegal.
                    if (entry.getKey().indexOf('.') < 0) {
                        String qualifiedName = prefix + "." + entry.getKey();
                        typesByName.put(qualifiedName, entry.getValue());
                    }
                }
            }

            // Linking included programs may have failed - if so, bail.
            if (_environment.HasErrors()) {
                throw new LinkFailureException();
            }
        }
    }
}