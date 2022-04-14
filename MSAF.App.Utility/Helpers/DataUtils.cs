namespace MSAF.App.Utility.Helpers
{
    public static class DataUtils
    {
        public static DateTimeOffset CurrentDateTimeOffset
        {
            get
            {
                var configTimeZone = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TimeZone")) ? "SE Asia Standard Time" : Environment.GetEnvironmentVariable("TimeZone");
                DateTimeOffset currentTimeOffset = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(configTimeZone));
                return currentTimeOffset;
            }
        }
    }
}
