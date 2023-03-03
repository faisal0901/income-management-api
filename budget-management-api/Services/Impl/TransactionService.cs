using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Exceptions;
using budget_management_api.Repositories;

namespace budget_management_api.Services;

public class TransactionService:ITransactionService
{
   

    private readonly IRepository<Transactional> _transactionalRepository;
    private readonly IPersistence _persistence;
    private readonly IWalletService _walletService;
    private readonly ISubCategoryService _subCategoryService;
    public TransactionService(IRepository<Transactional> transactionalRepository, IPersistence persistence, IWalletService walletService,ISubCategoryService subCategoryService)
    {
        _transactionalRepository = transactionalRepository;
        _persistence = persistence;
        _walletService = walletService;
        _subCategoryService = subCategoryService;
    }

    public async Task<IncomeResponse> CreateNewIncome(Transactional payload)
    {
        var result = await _persistence.ExecuteTransactionAsync(async () =>
        {
            payload.Type = ETransactionType.Income;
            payload.TransDate=DateTime.Now;
            var transactional = await _transactionalRepository.Save(payload);
            
            var emergencyBalance = 0.05 * transactional.Balance;
            var addEmergencyBalance= await _walletService.AddWalletBalance(EWalletType.EmergencyWallet, (long)emergencyBalance,payload.UserId);
           
            var needsBalance = 0.50 * transactional.Balance;
            var addNeedsBalance= await _walletService.AddWalletBalance(EWalletType.NeedsWallet, (long)needsBalance,payload.UserId);
               
            var wantsBalance = 0.30 * transactional.Balance;
            var addWantsBalance =await _walletService.AddWalletBalance(EWalletType.WantsWallet, (long)wantsBalance,payload.UserId);
            
            var savingBalance = 0.15 * transactional.Balance;
            var addSavingBalance= await _walletService.AddWalletBalance(EWalletType.SavingWallet, (long)savingBalance,payload.UserId);
            await _persistence.SaveChangesAsync();
            return new IncomeResponse()
               {
                   Id = transactional.Id.ToString(),
                   Balance = transactional.Balance,
                   TransDate = transactional.TransDate,
                   UserId = transactional.UserId.ToString(),
                   emergencyBalance = addEmergencyBalance.WalletBalance,
                   needsBalance = addNeedsBalance.WalletBalance,
                   savingBalance = addSavingBalance.WalletBalance,
                   wantsBalance = addWantsBalance.WalletBalance,
               };
        });
        return result;

    }

    private EWalletType GetWalletTypeFromCategory(string categoryName)
    {
        switch (categoryName)
        {
            case "darurat":
                return EWalletType.EmergencyWallet;
            case "keinginan":
                return EWalletType.WantsWallet;
            case "kebutuhan":
                return EWalletType.NeedsWallet;
            case "tabungan":
                return EWalletType.SavingWallet;
            default:
                throw new Exception("Invalid category name");
        }
    }

    public async Task<ExpendResponse> CreateNewExpand(Transactional payload)
    {
       
        
        var getCategory =await _subCategoryService.GetCategoryId((Guid)payload.SubCategoryId);
       
        payload.Type = ETransactionType.Expand;
        payload.TransDate=DateTime.Now;
       
            var walletType = GetWalletTypeFromCategory(getCategory.CategoryName);
            var wallet = await _walletService.SubtractWalletBalance(walletType, payload.Balance, payload.UserId);
        var transactional = await _persistence.ExecuteTransactionAsync(async () =>
        {
            payload.WalletId = wallet.Id;
            var savedTransactional= await _transactionalRepository.Save(payload);
            await _persistence.SaveChangesAsync();
            return  savedTransactional;
         

        });
        return new ExpendResponse()
        {
            Id = transactional.Id.ToString(),
            Balance = transactional.Balance,
            TransDate = transactional.TransDate,
            walletType = wallet.WalletType.ToString(),
            UserId = payload.UserId.ToString(),
            Category = getCategory.CategoryName,
            SubCategory = getCategory.SubCategoryName,
        };
    } 
    public async Task<DailyMonthlyTotalResponse> TotalMonthlyIncome(Guid userId, int date)
    {
        var month = await _transactionalRepository.Find(mt => 
            mt.TransDate.Month == date && mt.UserId == userId);
        if (month is null) throw new NotFoundException("Monthly Report Not Found");
        
        var currentMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month == date && mt.Type == ETransactionType.Income && mt.UserId == userId);
        
        long currentMonth = 0;

        foreach (var value in currentMonths)
        {
            currentMonth += value.Balance;
        }

        long avg ;

        try
        {
           avg = currentMonth / currentMonths.Count();
        }
        catch (Exception e)
        {
            avg = 0;
        }

        return new DailyMonthlyTotalResponse()
        {
            Balance = currentMonth,
            Average = avg,
            Month = month.TransDate.ToString("MMMM yyyy")
        };
    }
      public async Task<Transactional> CreateNewTransactional(Transactional payload)
        {
            var transactional = await _transactionalRepository.Save(payload);
            return transactional;
        }

    // Daily Report Income
    public async Task<IEnumerable<DailyIncomeReportResponse>> DailyReportIncome(Guid userId, int date)
    {
        var currentMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month == date && mt.Type == ETransactionType.Income && mt.UserId == userId );

        var dailyResponses = new List<DailyIncomeReportResponse>();
        
        if (currentMonths.Count().Equals(0)) throw new NotFoundException("Nothing transaction yet");

        foreach (var month in currentMonths)
        {
            dailyResponses.Add(new DailyIncomeReportResponse
            {
                TransDate = month.TransDate.ToString("dd MMMM yyyy"),
                In = month.Balance
            });
        }

        return dailyResponses;
    }
    
    // Monthly Report Income
    public async Task<MonthlyIncomeReportResponse> MonthlyReportIncome(Guid userId, int date)
    {
        var currentMonth = await TotalMonthlyIncome(userId, date);
        var prevMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month == date - 1 && mt.Type == ETransactionType.Income && mt.UserId == userId);

        long prevMonth = 0;
        double percentage;

        foreach (var value in prevMonths)
        {
            prevMonth += value.Balance;
        }

        try
        {
            percentage = (double) (currentMonth.Balance - prevMonth) * 100 / prevMonth;
        }
        catch (Exception e)
        {
            percentage = 0;
        }

        return new MonthlyIncomeReportResponse
        {
            Month = currentMonth.Month,
            TotalCurrentIncome = currentMonth.Balance,
            DailyAverageIncome = currentMonth.Average,
            TotalPreviousIncome = prevMonth,
            PercentageIncrease = percentage + "%"
        };
    }
    
    // Total of Monthly Expand
    public async Task<DailyMonthlyTotalResponse> TotalMonthlyExpend(Guid userId, int date)
    {
        var month = await _transactionalRepository.Find(mt => 
            mt.TransDate.Month == date && mt.UserId == userId);
        if (month is null) throw new NotFoundException("Monthly Report Not Found");
        
        var currentMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month == date && mt.Type == ETransactionType.Expand && mt.UserId == userId);
        
        long currentMonth = 0;

        foreach (var value in currentMonths)
        {
            currentMonth += value.Balance;
        }

        long avg ;

        try
        {
            avg = currentMonth / currentMonths.Count();
        }
        catch (Exception e)
        {
            avg = 0;
        }
        
        return new DailyMonthlyTotalResponse()
        {
            Balance = currentMonth,
            Average = avg,
            Month = month.TransDate.ToString("MMMM yyyy")
        };
    }
    
    // Daily Report Expand
    public async Task<IEnumerable<DailyExpendReportResponse>> DailyReportExpend(Guid userId, int date)
    {
        var currentMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month == date && mt.Type == ETransactionType.Expand && mt.UserId == userId, new string[] {"SubCategory", "SubCategory.Category"});

        if (currentMonths.Count().Equals(0)) throw new NotFoundException("Nothing transaction yet");
        
        var dailyResponses = new List<DailyExpendReportResponse>();

        foreach (var month in currentMonths)
        {
            dailyResponses.Add(new DailyExpendReportResponse()
            {
                TransDate = month.TransDate.ToString("dd MMMM yyyy"),
                Out = month.Balance,
                SubCategory = month.SubCategory.CategoryName,
                Category = month.SubCategory.Category.CategoryName
            });
        }

        return dailyResponses;
    }
    
    // Monthly Report Expand
    public async Task<MonthlyExpendReportResponse> MonthlyReportExpend(Guid userId, int date)
    {
        var currentMonth = await TotalMonthlyExpend(userId, date);
        
        var prevMonths = await _transactionalRepository.FindAll(mt =>
            mt.TransDate.Month.Equals(date - 1) && mt.Type.Equals(ETransactionType.Expand) && mt.UserId.Equals(userId));

        long prevMonth = 0;
        double percentage;

        foreach (var value in prevMonths)
        {
            prevMonth += value.Balance;
        }

        try
        {
            percentage = (double) (currentMonth.Balance - prevMonth) * 100 / prevMonth;
        }
        catch (Exception e)
        {
            percentage = 0;
        }

        return new MonthlyExpendReportResponse
        {
            Month = currentMonth.Month,
            TotalCurrentExpend = currentMonth.Balance,
            DailyAverageExpend = currentMonth.Average,
            TotalPreviousExpend = prevMonth,
            PercentageIncrease = percentage + "%"
        };
    }

    public async Task<long> TotalIncome(Guid userId)
    {
        var incomes = await _transactionalRepository.FindAll(income => 
            income.UserId.Equals(userId) && income.Type.Equals(ETransactionType.Income));
        long income = 0;
        foreach (var inc in incomes)
        {
            income += inc.Balance;
        }
        
        return income;
    }

    public async Task<long> TotalExpend(Guid userId)
    {
        var expends = await _transactionalRepository.FindAll(exp => 
            exp.UserId.Equals(userId) && exp.Type.Equals(ETransactionType.Expand));
        long expend = 0;
        foreach (var exp in expends)
        {
            expend += exp.Balance;
        }

        return expend;
    }
   
}

