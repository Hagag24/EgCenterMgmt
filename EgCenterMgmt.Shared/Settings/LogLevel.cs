using System.Text.Json.Serialization;

namespace EgCenterMgmt.Shared.Settings
{
    public class LogLevel
    {
        public string Default { get; set; } = string.Empty;
        [JsonPropertyName("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; } = string.Empty;
    }
}
