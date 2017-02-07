namespace Thriftier.Schema
{
    public class LinkEnvironment
    {
        private readonly ErrorReporter _errorReporter;

        public LinkEnvironment(ErrorReporter errorReporter)
        {
            _errorReporter = errorReporter;
        }

        public Linker GetLinker(Program program)
        {
            throw new System.NotImplementedException();
        }

        public bool HasErrors()
        {
            throw new System.NotImplementedException();
        }
    }
}