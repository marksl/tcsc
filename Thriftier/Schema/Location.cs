using System;
using System.Text;

namespace Thriftier.Schema
{
    public class Location
    {
        public string Base { get; private set; }
        public string Path { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }

        public static Location Get(string baseString, string path)
        {
            return new Location(baseString, path, -1, -1);
        }

        private Location(string baseString, string path, int line, int column)
        {
            if (string.IsNullOrWhiteSpace(baseString))
                throw new ArgumentNullException(nameof(baseString));
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (!(line > 0 || line == -1))
                throw new ArgumentOutOfRangeException(nameof(line), "line must be greater than 0 or -1");
            if (!(column > 0 || column == -1))
                throw new ArgumentOutOfRangeException(nameof(column), "line must be greater than 0 or -1");

            Base = baseString;
            Path = path;
            Line = line;
            Column = column;
        }

        public Location at(int line, int column)
        {
            return new Location(Base, Path, line, column);
        }

        /// <summary>
        /// Computes and returns the Thrift 'program' name, which is the filename portion
        /// of the full path *without* the .thrift extension.
        ///
        /// @return the Thrift program name representing this file.
        /// </summary>
        public string ProgramName
        {
            get
            {
                string name = Path;
                int separatorIndex = name.LastIndexOf(System.IO.Path.PathSeparator);
                if (separatorIndex != -1)
                {
                    name = name.Substring(separatorIndex + 1);
                }
                int dotIndex = name.IndexOf('.');
                if (dotIndex != -1)
                {
                    name = name.Substring(0, dotIndex);
                }
                return name;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Base.Length + Path.Length);
            if (Base.Length > 0)
            {
                sb.Append(Base).Append(System.IO.Path.DirectorySeparatorChar);
            }
            sb.Append(Path);
            if (Line != -1)
            {
                sb.Append(" at ").Append(Line);
                if (Column != -1)
                {
                    sb.Append(":").Append(Column);
                }
            }
            return sb.ToString();
        }

        protected bool Equals(Location other)
        {
            return string.Equals(Base, other.Base) && string.Equals(Path, other.Path) && Line == other.Line &&
                   Column == other.Column;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Base != null ? Base.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Line;
                hashCode = (hashCode * 397) ^ Column;
                return hashCode;
            }
        }
    }
}