using Services;
using System;

namespace WPFApp
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetUTCNow()
        {
            return DateTime.UtcNow;
        }
    }
}
