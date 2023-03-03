using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Enum;
using budget_management_api.Repositories;
using budget_management_api.Scheduler;
using budget_management_api.Services;
using Microsoft.AspNetCore.Authorization;
using Quartz;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;


namespace budget_management_api.Scheduler;

public class MyJob:IJob
{
    
 
 
    private readonly AppDbContext _context;
    private readonly IWalletService _walletService;
    private readonly IBillingService _billingService;
    private readonly ITransactionService _transactionService;
    private readonly IPersistence _persistence;
    public MyJob(AppDbContext context, IWalletService walletService, IBillingService billingService, ITransactionService transactionService, IPersistence persistence)
    {
        _context = context;
        _walletService = walletService;
        _billingService = billingService;
        _transactionService = transactionService;
        _persistence = persistence;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        //insert data transaksi ke db
        var dataMap = context.JobDetail;
      
        var bill=await _billingService.GetById(Guid.Parse(dataMap.Key.Name));
        var wallet =await _walletService.SubtractWalletBalance(EWalletType.WantsWallet, (long)bill.BillAmount,bill.UserId);
        var TransInsert = new Transactional()
        {
            Balance = bill.BillAmount,
            Type = ETransactionType.Expand,
            TransDate = DateTime.Now,
            UserId = bill.UserId,
            WalletId = wallet.Id,
        };
        await _transactionService.CreateNewTransactional(TransInsert);
        await _persistence.SaveChangesAsync();
     
        await Task.FromResult(true);
    }

 
}
