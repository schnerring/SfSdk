using System;

namespace SfSdk
{
    internal static class Helpers
    {
        internal static double ToUnixTimeStamp(this DateTime date)
        {
            var t = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return  t.TotalMilliseconds;
        }
    }
}