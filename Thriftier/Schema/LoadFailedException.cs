using System;

namespace Thriftier.Schema
{
    public class LoadFailedException : Exception
    {
        public ErrorReporter ErrorReporter { get; private set; }


        public LoadFailedException(Exception cause, ErrorReporter errorReporter)
        {
            ErrorReporter = errorReporter;
        }


    }
}