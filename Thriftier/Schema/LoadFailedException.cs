using System;

namespace Thriftier.Schema
{
    public class LoadFailedException : Exception
    {
        private ErrorReporter errorReporter;

        public LoadFailedException(Exception cause, ErrorReporter errorReporter)

        {

        }


    }
}