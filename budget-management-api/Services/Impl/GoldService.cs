using System.Diagnostics;
using System.Net.Http.Headers;
using budget_management_api.Dtos;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace budget_management_api.Services.Impl;
using Newtonsoft.Json;
public class GoldService:IGoldService
{

    private readonly IRepository<Gold> _goldRepository;
    private readonly ITransactionService _transactionService;
    private readonly IWalletService _walletService;
    private readonly IPersistence _persistence;
    private readonly ISubCategoryService _subCategoryService;

    public GoldService(IRepository<Gold> walletRepository, IWalletService walletService, IPersistence persistence, ITransactionService transactionService, ISubCategoryService subCategoryService)
    {
        _goldRepository = walletRepository;
        _walletService = walletService;
        _persistence = persistence;
        _transactionService = transactionService;
        _subCategoryService = subCategoryService;
    }

    public async Task<double?> GetCurrentPriceGold()
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-access-token","goldapi-4mqm9tldit2u4l-io");
        using HttpResponseMessage response = await client.GetAsync("https://www.goldapi.io/api/XAU/USD");
        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            JObject  result = (JObject)JsonConvert.DeserializeObject(data);
            return (double)result["price_gram_24k"];
        }

        return null;

    }

    public async Task<long?> ConvertGoldToIdr()
    {
        try
        {
            var UsdGoldPrice =await GetCurrentPriceGold();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey","FkRw61oGH4FwnNSQV903ogSHPd9uiiPR");
            using HttpResponseMessage response = await client.GetAsync($"https://api.apilayer.com/exchangerates_data/convert?to=idr&from=usd&amount={UsdGoldPrice}");
            string data = await response.Content.ReadAsStringAsync();
            JObject  result = (JObject)JsonConvert.DeserializeObject(data);
            return (long)result["result"];

        
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }
    
    public async Task<GoldResponse> BuyNewGold(GoldRequest payload)
    {
        var result = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var currentPrice =await ConvertGoldToIdr();
            var totalPrice = payload.gram * currentPrice;
            var wallet=await _walletService.SubtractWalletBalance(EWalletType.SavingWallet, (long)totalPrice,payload.UserId);
            var goldInsert = new Gold()
            {
                Gram = payload.gram,
                WalletId = wallet.Id,
                Price = (long)totalPrice,
                TransDate = DateTime.Now,
                
            };
            var gold = await _goldRepository.Save(goldInsert);
            var transInsert = new Transactional()
            {
                UserId = payload.UserId,
                Balance = (long)totalPrice,
                Type = ETransactionType.Expand,
                TransDate = DateTime.Now,
                WalletId = wallet.Id,
                SubCategoryId = Guid.Parse("C2A961B2-672F-401D-9169-8538F64742B7")
            };
            await _transactionService.CreateNewTransactional(transInsert);
            await _persistence.SaveChangesAsync();
            return new GoldResponse()
            {
                Gram = gold.Gram,
                WalletType = EWalletType.SavingWallet.ToString(),
                price = (long)currentPrice,
                total = (long)totalPrice,
                UserId = payload.UserId.ToString()
            };
        });
        return result;
    }

    public async Task<GoldReportResponse> GetGold(Guid userId)
    {
        var wallet = await _walletService.GetWalletByType(EWalletType.SavingWallet, userId);
        long buytotalPrice = 0;
        int totalGram = 0;
        var gold = await _goldRepository.FindAll(g =>g.WalletId.Equals(wallet.Id) );
        foreach (var g in gold)
        {
            buytotalPrice += g.Price;
            totalGram += g.Gram;
        }
        var currentIdrPriceGold = await ConvertGoldToIdr();

        var currentTotalPrice = totalGram *currentIdrPriceGold;
        double percentage = (double)(currentTotalPrice - buytotalPrice) * 100 / buytotalPrice;
        return new GoldReportResponse()
        {
            currentTotalPrice = (long)currentTotalPrice,
            percentage = percentage,
            GoldTotalGram = totalGram,
            TotalPriceWhenBuying = buytotalPrice
        };
    }
}

