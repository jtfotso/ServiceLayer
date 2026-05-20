using ServiceLayer.DTW.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.DTW.Application.Interfaces
{
    public interface IServiceLayerClient
    {
        Task LoginAsync(CancellationToken ct = default);
        Task LogoutAsync(CancellationToken ct = default);
        Task CreateBusinessPartnerAsync(BusinessPartner bp, CancellationToken ct = default);
        Task UpdateBusinessPartnerAsync(BusinessPartner bp, CancellationToken ct = default);
        Task<bool> BusinessPartnerExistsAsync(string cardCode, CancellationToken ct = default);
    }
}
