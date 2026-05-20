using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceLayer.DTW.Infrastructure.DTOs
{
    public class SLContactEmployee
    {
        [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("FirstName")] public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("LastName")] public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("Phone1")] public string Phone1 { get; set; } = string.Empty;
        [JsonPropertyName("MobilePhone")] public string MobilePhone { get; set; } = string.Empty;
        [JsonPropertyName("E_Mail")] public string Email { get; set; } = string.Empty;
        [JsonPropertyName("Position")] public string Position { get; set; } = string.Empty;

        /// <summary>Captures any U_* UDF columns from the contacts file automatically.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
    }
}
