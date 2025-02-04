using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgCenterMgmt.Shared.Settings
{
    public class AppSettings
    {
        public TenantSettings TenantSettings { get; set; } = new TenantSettings();
        public Logging Logging { get; set; } = new Logging();
        public string AllowedHosts { get; set; } = "*";
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}
