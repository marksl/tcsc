namespace Thriftier.Schema
{
    public class LinkEnvironment
    {
        private readonly ErrorReporter _errorReporter;

        public LinkEnvironment(ErrorReporter errorReporter)
        {
            _errorReporter = errorReporter;
        }
    }
}