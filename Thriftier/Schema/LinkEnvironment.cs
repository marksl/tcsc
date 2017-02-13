using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Thriftier.Schema
{
    public class LinkEnvironment
    {
        private readonly ErrorReporter _errorReporter;

        private readonly Dictionary<Program, Linker> linkers = new Dictionary<Program, Linker> ();

        public Linker GetLinker(Program program)
        {
            Linker linker;
            if (!linkers.TryGetValue(program, out linker)) {
                linker = new Linker(this, program, _errorReporter);
                linkers.Add(program, linker);
            }
            return linker;
        }

        private ErrorReporter Reporter => _errorReporter;


        public LinkEnvironment(ErrorReporter errorReporter)
        {
            _errorReporter = errorReporter;
        }


        public bool HasErrors()
        {
            return _errorReporter.HasError;
        }


        public IEnumerable<string> getErrors() {
            if (!HasErrors())
            {
                yield break;
            }

            foreach (Report report in _errorReporter.Reports) {
                String level = report.Level.ToString();
                String msg = level + ": " + report.Message + "(" + report.Location + ")";

                yield return msg;

            }
        }
    }
}