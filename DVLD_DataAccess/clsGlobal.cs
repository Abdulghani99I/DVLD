using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    internal class clsGlobal
    {
        static public void LogError(string MessageError, string SourceName = "AppDVLD")
        {
            // Create the event source if it does not exist
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            } 

            // Log an information event
            EventLog.WriteEntry(SourceName, $"Message Error: {MessageError}", EventLogEntryType.Error);
        }

        static public void LogInformation(string Message, string SourceName = "AppDVLD")
        {
            // Create the event source if it does not exist
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }

            // Log an information event
            EventLog.WriteEntry(SourceName, $"Message: {Message}", EventLogEntryType.Information);
        }


        //public static void SaveToLogEventViewer(string Message, EventLogEntryType LogType, string SourceName = "DVLD_App")
        //{
        //    // Create the event source if it does not exist
        //    if (!EventLog.SourceExists(SourceName))
        //    {
        //        EventLog.CreateEventSource(SourceName, "Application");
        //    }
        //    // Log an information event
        //    EventLog.WriteEntry(SourceName, Message, LogType);
        //}
    }

}
