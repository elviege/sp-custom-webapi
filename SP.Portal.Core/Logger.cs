﻿using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SP.Portal.Core
{
    public class Logger : SPDiagnosticsServiceBase
    {
        public static string DiagnosticAreaName = "SP.Portal";

        private static Logger _Current;

        public static Logger Current
        {
            get
            {
                if (_Current == null)
                    _Current = new Logger();

                return _Current;
            }
        }

        public Logger() : base("SP.Portal Logging Service", SPFarm.Local) { }

        public enum Category
        {
            Unexpected,
            High,
            Medium,
            Information
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            var areas = new List<SPDiagnosticsArea>
            {
                new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
                {
                    new SPDiagnosticsCategory("Unexpected", TraceSeverity.Unexpected, EventSeverity.Error),
                    new SPDiagnosticsCategory("High", TraceSeverity.High, EventSeverity.Warning),
                    new SPDiagnosticsCategory("Medium", TraceSeverity.Medium, EventSeverity.Information),
                    new SPDiagnosticsCategory("Information", TraceSeverity.Verbose, EventSeverity.Information)
                })
            };
            return areas;
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog(Category.Unexpected, "SP.Portal", $"Message: \"{ex.Message}\". StackTrace: \"{ex.StackTrace}\"");
        }

        public static void WriteLog(Category categoryName, string source, string errorMessage)
        {
            SPDiagnosticsCategory category = Current.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];
            Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(source, ": ", errorMessage));
        }

        public static void WriteLog(Category categoryName, Type type, string errorMessage)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            string methodName = string.Concat(type.ToString(), ".", sf.GetMethod().Name);

            SPDiagnosticsCategory category = Current.Areas[DiagnosticAreaName].Categories[categoryName.ToString()];

            Current.WriteTrace(0, category, category.TraceSeverity, string.Concat(methodName, ": ", errorMessage));
        }
    }
}
