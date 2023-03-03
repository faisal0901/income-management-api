using budget_management_api.Dtos;
using budget_management_api.Entities;
using Newtonsoft.Json.Linq;

namespace budget_management_api.Services.Impl;

public interface IGoldService
{
    Task<double?> GetCurrentPriceGold();
    Task<long?> ConvertGoldToIdr();
    Task<GoldResponse> BuyNewGold(GoldRequest payload);
    Task<GoldReportResponse> GetGold(Guid parse);
}