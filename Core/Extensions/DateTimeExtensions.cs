using System;

namespace Shares.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static double Unix(this DateTime time)
        {
            var zeroPoint = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time.Subtract(zeroPoint).TotalSeconds;
        }
    }
}
