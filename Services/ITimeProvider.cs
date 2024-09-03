using System;

namespace Services
{
    public interface ITimeProvider
    {
        DateTime GetUTCNow();
    }
}
