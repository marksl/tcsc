using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using Thriftier.Schema.Parser;

namespace Thriftier.Schema
{
    public class Loader
    {

        /**
     * Attempts to identify strings that represent absolute filesystem paths.
     * Does not attempt to support more unusual paths like UNC ("\\c\path") or
     * filesystem URIs ("file:///c/path").
     */
        private static Regex ABSOLUTE_PATH_PATTERN = new Regex("^(/|\\w:\\\\).*");

        /*private static Predicate<File> IS_THRIFT = new Predicate<File>() {
            @Override
            public boolean apply(@Nullable File input) {
                return input != null && input.getName().endsWith(".thrift");
            }
        };*/

        /**
         * A list of thrift files to be loaded.  If empty, all .thrift files within
         * {@link #includePaths} will be loaded.
         */
        private List<string> thriftFiles = new List<string>();

        /**
         * The search path for imported thrift files.  If {@link #thriftFiles} is
         * empty, then all .thrift files located on the search path will be loaded.
         */
        private List<string> includePaths = new List<string>();

        private ErrorReporter errorReporter = new ErrorReporter();

        private LinkEnvironment environment = new LinkEnvironment(errorReporter);

        private Dictionary<String, Program> loadedPrograms;

        public Loader addThriftFile(String file) {
            if(file == null) throw new ArgumentNullException(nameof(file));

            thriftFiles.Add(file);
            return this;
        }

        public Loader addIncludePath(string path) {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (!Directory.Exists(path))
             throw new ArgumentException("path must be a directory", nameof(path));

            includePaths.Add(path);
            return this;
        }

        public Schema Load()
        {
            try
            {
                loadFromDisk();
                linkPrograms();
                return new Schema(loadedPrograms.values());
            }
            catch (Exception e)
            {
                throw new LoadFailedException(e, errorReporter);
            }
        }


        private void loadFromDisk()
        {
            List<String> filesToLoad = new List<string>(thriftFiles);
            if (!filesToLoad.Any())
            {
                foreach (string file in includePaths)
                {
                    var thriftFiles = Directory.EnumerateFiles(file, "*.thrift", SearchOption.AllDirectories);
                    foreach (var thriftFile in thriftFiles)
                    {
                        filesToLoad.Add(thriftFile);
                    }
                }
            }

            Dictionary<String, ThriftFileElement> loadedFiles = new Dictionary<string, ThriftFileElement>();
            foreach (string path in filesToLoad)
            {
                loadFileRecursively(path, loadedFiles);
            }

// Convert to Programs
            loadedPrograms = new Dictionary<string, Program>();

            foreach (ThriftFileElement fileElement in loadedFiles.Values)
            {
                File file = new File(fileElement.Location.Base, fileElement.Location.Path);

                if (!file.exists())
                    throw new AssertionError(
                        "We have a parsed ThriftFileElement with a non-existing location");

                if (!file.isAbsolute()) throw new AssertionError("We have a non-canonical path");

                Program program = new Program(fileElement);
                loadedPrograms.put(file.getCanonicalPath(), program);
            }

            // Link included programs together
            HashSet<Program> visited = new HashSet<Program>();
            foreach (Program program in loadedPrograms.Values)
            {
                program.loadIncludedPrograms(this, visited);
            }
        }

/**
 * Loads and parses a Thrift file and all files included (both directly and
 * transitively) by it.
 *
 * @param path A relative or absolute path to a Thrift file.
 * @param loadedFiles A mapping of absolute paths to parsed Thrift files.
 */
        private void loadFileRecursively(String path, Dictionary<String, ThriftFileElement> loadedFiles)
        {
            ThriftFileElement element = null;
            DirectoryInfo dir = null;

            FileInfo file = findFirstExisting(path, null);

            if (file != null)
            {
// Resolve symlinks, redundant '.' and '..' segments.
                file = file.getCanonicalFile();

                if (loadedFiles.containsKey(file.getAbsolutePath()))
                {
                    return;
                }

                dir = file.getParentFile();
                element = loadSingleFile(file.getParentFile(), file.getName());
            }

            if (element == null)
            {
                throw new FileNotFoundException(
                    "Failed to locate " + path + " in " + includePaths);
            }

            loadedFiles.Add(file.getAbsolutePath(), element);


            ImmutableList<IncludeElement> includes = element.includes();
            if (includes.size() > 0)
            {
                includePaths.addFirst(dir);
                for (IncludeElement include :
                includes)
                {
                    if (!include.isCpp())
                    {
                        loadFileRecursively(include.path(), loadedFiles);
                    }
                }
                includePaths.removeFirst();
            }
        }

        private void linkPrograms()
        {
            lock (environment)
            {
                foreach (Program program in loadedPrograms.Values)
                {
                    Linker linker = environment.getLinker(program);
                    linker.link();
                }

                if (environment.hasErrors())
                {
                    throw new IllegalStateException("Linking failed");
                }
            }
        }

        private ThriftFileElement loadSingleFile(File base, String path) throws IOException {
File file = new File(base, path).getAbsoluteFile();
if (!file.exists()) {
return null;
}

Source source = Okio.source(file);
try {
Location location = Location.get(base.toString(), path);
String data = Okio.buffer(source).readUtf8();
return ThriftParser.parse(location, data, errorReporter);
} catch (IOException e) {
throw new IOException("Failed to load " + path + " from " + base, e);
} finally {
Closeables.close(source, true);
}
}

Program resolveIncludedProgram(Location currentPath, String importPath) throws IOException {
File resolved = findFirstExisting(importPath, currentPath);
if (resolved == null) {
throw new AssertionError("Included thrift file not found: " + importPath);
}
try {
return getAndCheck(resolved.getCanonicalPath());
} catch (IOException e) {
throw new IOException("Failed to get canonical path for file " + resolved.getAbsolutePath(), e);
}
}

/**
 * Resolves a relative path to the first existing match.
 *
 * Resolution rules favor, in order:
 * 1. Absolute paths
 * 2. The current working location, if given
 * 3. The include path, in the order given.
 *
 * @param path a relative or absolute path to the file being sought.
 * @param currentLocation the current working directory.
 * @return the first matching file on the search path, or {@code null}.
 */
private FileInfo findFirstExisting(String path, @Nullable  Location currentLocation) {
if (isAbsolutePath(path)) {
// absolute path, should be loaded as-is
File f = new File(path);
return f.exists() ? f : null;
}

if (currentLocation != null) {
File maybeFile = new File(currentLocation.base(), path).getAbsoluteFile();
if (maybeFile.exists()) {
return maybeFile;
}
}

for (File includePath : includePaths) {
File maybeFile = new File(includePath, path).getAbsoluteFile();
if (maybeFile.exists()) {
return maybeFile;
}
}

return null;
}

private Program getAndCheck(String absolutePath) {
Program p = loadedPrograms.get(absolutePath);
if (p == null) {
throw new AssertionError("All includes should have been resolved by now: " + absolutePath);
}
return p;
}

/**
 * Checks if the path is absolute in an attempted cross-platform manner.
 */
private static boolean isAbsolutePath(String path) {
return ABSOLUTE_PATH_PATTERN.matcher(path).matches();
}
}
}