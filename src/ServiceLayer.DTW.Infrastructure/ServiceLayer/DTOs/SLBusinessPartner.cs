using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceLayer.DTW.Infrastructure.DTOs
{
    public class SLBusinessPartner
    {
        [JsonPropertyName("CardCode")] public string CardCode { get; set; } = string.Empty;
        [JsonPropertyName("CardName")] public string CardName { get; set; } = string.Empty;
        [JsonPropertyName("CardType")] public string CardType { get; set; } = string.Empty;
        [JsonPropertyName("GroupCode")] public int? GroupCode { get; set; }
        [JsonPropertyName("Currency")] public string? Currency { get; set; }
        [JsonPropertyName("PayTermsGrpCode")] public int? PayTermsGrpCode { get; set; }
        [JsonPropertyName("Phone1")] public string? Phone1 { get; set; }
        [JsonPropertyName("EmailAddress")] public string? EmailAddress { get; set; }
        [JsonPropertyName("Website")] public string? Website { get; set; }
        [JsonPropertyName("FederalTaxID")] public string? FederalTaxID { get; set; }

        [JsonPropertyName("BPAddresses")]
        public List<SLBPAddress> BPAddresses { get; set; } = [];

        [JsonPropertyName("ContactEmployees")]
        public List<SLContactEmployee> ContactEmployees { get; set; } = [];

        /// <summary>Captures any U_* UDF columns from the header file automatically.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
    }
}
