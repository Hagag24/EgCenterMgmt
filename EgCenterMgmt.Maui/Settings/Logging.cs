using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearnMgmt.Shared.Settings
{
    public class Logging
    {
        public LogLevel LogLevel { get; set; } = new();
    }
}
