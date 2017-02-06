using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Thriftier.Schema;

namespace ThriftierCompiler
{
    /// <summary>
    ///
    /// ThriftyCompiler --out=/path/to/output
    ///           --path=dir/for/search/path
    ///           file1.thrift
    ///           file2.thrift
    ///
    /// --out: is required, and specifies the directory to which generated
    /// C# sources will be written.
    ///
    /// --path can be given multiple times.  Each directory so specified
    /// will be placed on the search path.  When resolving include statements
    /// during thrift compilation, these directories will be searched for included files.
    ///
    /// </summary>
    internal class Program
    {
        private static String OUT_PREFIX = "--out=";
        private static String PATH_PREFIX = "--path=";
        private static String PARCELABLE_ARG = "--parcelable";

        private static string outputDirectory;
        private static readonly List<String> thriftFiles = new List<String>();
        private static readonly List<String> searchPath = new List<String>();
        private static String listTypeName;
        private static String setTypeName;
        private static String mapTypeName;
        private static bool emitNullabilityAnnotations = false;
        private static bool emitParcelable = false;
        private static FieldNamingPolicy fieldNamingPolicy = FieldNamingPolicy.Default;

        public static void Main(string[] args)
        {
            try
            {
                string currentDirectory = Assembly.GetEntryAssembly().Location;
                searchPath.Insert(0, currentDirectory);
                Console.Out.WriteLine(currentDirectory);
                ParseArgs(args);
                Compile();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }


        public static void ParseArgs(string[] args)
        {
            foreach(string arg in args)
            {
                if (arg.StartsWith(OUT_PREFIX))
                {
                    String path = arg.Substring(OUT_PREFIX.Length);
                    outputDirectory = path;
                }
                else if (arg.StartsWith(PATH_PREFIX))
                {

                    String dirname = arg.Substring(PATH_PREFIX.Length);
                    searchPath.Add(dirname);
                }
                else if (arg.Trim().Equals(PARCELABLE_ARG))
                {
                    emitParcelable = true;
                }
                else if (arg.StartsWith("-"))
                {
                    throw new ArgumentException("Unrecognized argument: " + arg);
                }
                else
                {
                    thriftFiles.Add(arg);
                }
            }

            if (outputDirectory == null)
            {
                throw new ArgumentException("Output path must be provided (missing --out=path)");
            }
        }

        private static void Compile()
        {
            Loader loader = new Loader();
            foreach (String thriftFile in thriftFiles)
            {
                loader.addThriftFile(thriftFile);
            }

            for (String dir : searchPath) {
                loader.addIncludePath(new File(dir));
            }

            Schema schema;
            try {
                schema = loader.Load();
            } catch (LoadFailedException e) {
                foreach (string report in e.errorReporter().formattedReports()) {
                    Console.Out.WriteLine(report);
                }

                Environment.Exit(1);
                return;
            }

            ThriftyCodeGenerator gen = new ThriftyCodeGenerator(schema, fieldNamingPolicy);
            if (listTypeName != null) {
                gen = gen.withListType(listTypeName);
            }

            if (setTypeName != null) {
                gen = gen.withSetType(setTypeName);
            }

            if (mapTypeName != null) {
                gen = gen.withMapType(mapTypeName);
            }

            TypeProcessorService svc = TypeProcessorService.getInstance();
            TypeProcessor processor = svc.get();
            if (processor != null) {
                gen = gen.usingTypeProcessor(processor);
            }

            gen.emitAndroidAnnotations(emitNullabilityAnnotations);
            gen.emitParcelable(emitParcelable);

            gen.generate(outputDirectory);
        }
    }
}