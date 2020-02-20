using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace Xrm.DataManager.Framework
{
    public class ConsoleLogger : ILogger
    {
        protected JobSettings JobSettings;
        protected string RunId;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleLogger(JobSettings jobSettings, string runId)
        {
            JobSettings = jobSettings;
            RunId = runId;
        }

        /// <summary>
        /// Output information to console
        /// </summary>
        public void LogMessage(string message, string jobName = null) => Console.WriteLine(message);

        /// <summary>
        /// Log message to console
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message) => Console.WriteLine(message);

        /// <summary>
        /// Log custom event to App Insights
        /// </summary>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <param name="jobName"></param>
        public void LogEvent(string name, Dictionary<string, string> properties, string jobName = null)
        {
            var defaultProperties = Utilities.GetContextProperties(JobSettings, RunId);
            foreach (var property in defaultProperties)
            {
                properties.Add(property.Key, property.Value);
            }
            if (jobName != null)
            {
                properties.Add("Job Name", jobName);
            }

            foreach (var p in properties)
            {
                Console.WriteLine(string.Format("\t{0} => {1}", p.Key, p.Value));
            }
        }

        /// <summary>
        /// Log event with record details
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="crmRecord"></param>
        /// <param name="jobName"></param>
        public void LogSuccess(string eventName, Entity crmRecord, string jobName)
        {
            var recordData = new Dictionary<string, string>()
            {
                { "Record : Crm Record Id", crmRecord.Id.ToString("D") },
                { "Record : Crm Record Type", crmRecord.LogicalName }
            };

            LogEvent(eventName, recordData, jobName);
        }

        /// <summary>
        /// Log exception with record details
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="crmRecord"></param>
        /// <param name="jobName"></param>
        public void LogFailure(Exception ex, Entity crmRecord, string jobName)
        {
            var recordData = new Dictionary<string, string>()
            {
                { "Record : Crm Record Id", crmRecord.Id.ToString("D") },
                { "Record : Crm Record Type", crmRecord.LogicalName }
            };

            LogException(ex, recordData, jobName);
        }

        /// <summary>
        /// Log exception to App Insights
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        /// <param name="jobName"></param>
        public void LogException(Exception exception, Dictionary<string, string> properties, string jobName = null)
        {
            var defaultProperties = Utilities.GetContextProperties(JobSettings, RunId);
            foreach (var property in defaultProperties)
            {
                properties.Add(property.Key, property.Value);
            }
            if (jobName != null)
            {
                properties.Add("Job Name", jobName);
            }

            foreach (var p in properties)
            {
                Console.WriteLine(string.Format("\t{0} => {1}", p.Key, p.Value));
            }
        }
    }
}
