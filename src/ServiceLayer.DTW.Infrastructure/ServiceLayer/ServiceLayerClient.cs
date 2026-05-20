using Microsoft.Extensions.Options;
using ServiceLayer.DTW.Application.Interfaces;
using ServiceLayer.DTW.Domain.Models;
using ServiceLayer.DTW.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ServiceLayer.DTW.Infrastructure.ServiceLayer
{
    public class ServiceLayerClient : IServiceLayerClient
    {
        private readonly HttpClient _http;
        private readonly ServiceLayerConfig _config;
        public ServiceLayerClient(HttpClient http, IOptions<ServiceLayerConfig> config)
        {
            _http = http;
            _config = config.Value;
        }

        public async Task<bool> BusinessPartnerExistsAsync(string cardCode, CancellationToken ct = default)
        {
            var response = await _http.GetAsync(
            $"BusinessPartners('{cardCode}')?$select=CardCode", ct);

            if (response.StatusCode == HttpStatusCode.NotFound) return false;
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task CreateBusinessPartnerAsync(BusinessPartner bp, CancellationToken ct = default)
        {
            var dto = BusinessPartnerMapper.ToSLDto(bp);
            var response = await _http.PostAsJsonAsync("BusinessPartners", dto, ct);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(await ReadErrorMessage(response));
        }

        public async Task LoginAsync(CancellationToken ct = default)
        {
            var payload = new
            {
                CompanyDB = _config.CompanyDb,
                UserName = _config.UserName,
                Password = _config.Password,
            };

            var response = await _http.PostAsJsonAsync("login", payload, ct);
            response.EnsureSuccessStatusCode();
        }

        public async Task LogoutAsync(CancellationToken ct = default)
        {
            var response = await _http.PostAsync("logout", null, ct);
        }

        public async Task UpdateBusinessPartnerAsync(BusinessPartner bp, CancellationToken ct = default)
        {
            var dto = BusinessPartnerMapper.ToSLDto(bp);
            var response = await _http.PatchAsJsonAsync($"BusinessPartners('{bp.CardCode}')", dto, ct);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(await ReadErrorMessage(response));
        }

        private static async Task<string> ReadErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                return json.GetProperty("error").GetProperty("message").GetProperty("value").GetString()
                       ?? response.ReasonPhrase
                       ?? "Unknown error";
            }
            catch
            {
                return response.ReasonPhrase ?? "Unknown error";
            }
        }
    }
}
