using MSAF.App.Utility.Helpers;
using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace MSAF.App.Utility.NLogLayout
{
    [LayoutRenderer("custom-datetime")]
    public class CustomDateTimeRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(DataUtils.CurrentDateTimeOffset);
        }
    }
}
