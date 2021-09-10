using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
