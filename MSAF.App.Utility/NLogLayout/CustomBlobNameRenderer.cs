using MSAF.App.Utility.Helpers;
using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace MSAF.App.Utility.NLogLayout
{
    [LayoutRenderer("custom-blobname")]
    public class CustomBlobNameRenderer : LayoutRenderer
    {
        private const string filePath = "{0}/{1:yyyy}/{1:MM}/{1:dd}/{1:HH}.log";
        private const string filePathWithRoot = "{0}/{1}/{2:yyyy}/{2:MM}/{2:dd}/{2:HH}.log";
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            //var loggerName = logEvent.LoggerName;
            //var logLevel = logEvent.Level;
            //var moduleName = loggerName.StartsWith("Function") && loggerName.Split(".").Length > 1 ? loggerName.Split(".")[1] : string.Empty;

            //if (!string.IsNullOrEmpty(moduleName))
            //{
            //    builder.Append(filePathWithRoot.Format(moduleName, logLevel, DataUtils.CurrentDateTimeOffset));
            //}
            //else
            //{
            //    builder.Append(filePath.Format(logLevel.ToString(), DataUtils.CurrentDateTimeOffset));
            //}
        }

    }
}
