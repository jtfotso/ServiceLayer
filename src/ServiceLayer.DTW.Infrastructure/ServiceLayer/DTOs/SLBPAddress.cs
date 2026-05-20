using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceLayer.DTW.Infrastructure.DTOs
{
    public class SLBPAddress
    {
        [JsonPropertyName("AddressName")] public string AddressName { get; set; } = string.Empty;
        [JsonPropertyName("AddressType")] public string AddressType { get; set; } = string.Empty;
        [JsonPropertyName("Street")] public string Street { get; set; } = string.Empty;
        [JsonPropertyName("City")] public string City { get; set; } = string.Empty;
        [JsonPropertyName("ZipCode")] public string ZipCode { get; set; } = string.Empty;
        [JsonPropertyName("Country")] public string Country { get; set; } = string.Empty;
        [JsonPropertyName("State")] public string State { get; set; } = string.Empty;

        /// <summary>Captures any U_* UDF columns from the address file automatically.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
    }
}
