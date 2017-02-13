using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Thriftier.Schema
{

    public enum Level
    {
        WARNING,
        ERROR
    }

    public class Report
    {
        public Level Level { get; set; }
        public Location Location { get; set; }
        public string Message { get; set; }

        static public Report create(Level level, Location location, String message)
        {
            return new
                Report
                {
                    Level = level,
                    Location = location,
                    Message = message
                };

        }
    }
    public class ErrorReporter
    {


        public bool HasError { get; private set; }
        private List<Report> reports = new List<Report>();

        public void warn(Location location, String message)
        {
            reports.Add(Report.create(Level.WARNING, location, message));
        }

        public void error(Location location, String message)
        {
            HasError = true;
            reports.Add(Report.create(Level.ERROR, location, message));
        }

        public IEnumerable<Report> Reports
        {
            get { return reports; }
        }



    public IEnumerable<string> formattedReports() {

            StringBuilder sb = new StringBuilder();
            foreach (Report report in reports)
            {
                switch (report.Level) {
                    case Level.WARNING: sb.Append("W: "); break;
                    case Level.ERROR:   sb.Append("E: "); break;
                    default:
                        throw new Exception("Unexpected report level: " + report.Level);
                }

                sb.Append(report.Message);
                sb.Append(" (at ");
                sb.Append(report.Location);
                sb.Append(")");

                yield return sb.ToString();

            }
        }

    }
}