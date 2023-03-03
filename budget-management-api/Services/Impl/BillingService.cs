using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Repositories;
using budget_management_api.Services;

public class BillingService : IBillingService
{
  private readonly IRepository<Bill> _billingRepository;
  private readonly IPersistence _persistence;

  public BillingService(IRepository<Bill> billingRepository, IPersistence persistence)
  {
    _billingRepository = billingRepository;
    _persistence = persistence;
  }

  public async Task<BillingResponse> Create(Bill payload)
  {
    var billInsert = new Bill()
    {
      BillDate = payload.BillDate,
      BillAmount = payload.BillAmount,
      UserId = payload.UserId,
      BillName = payload.BillName
    };
    
      var save = await _billingRepository.Save(billInsert);
      await _persistence.SaveChangesAsync();
     
    

    BillingResponse response = new()
    {
      Id = save.Id,
      BillName = save.BillName,
      BillAmount = save.BillAmount,
      EachBilledDate = save.BillDate,
      UserId = save.UserId,
    };

    return response;
  }

  

  public async Task<BillingResponse> GetById(Guid id)
  {
    try
    {
      var billing = await _billingRepository.Find(billing => billing.Id.Equals(id));

      if (billing is null) throw new Exception("billing not found");

      BillingResponse response = new()
      {
        Id = billing.Id,
        BillName = billing.BillName,
        BillAmount = billing.BillAmount,
        EachBilledDate = billing.BillDate,
        UserId = billing.UserId
      };

      return response;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  // public Task<Bill> Update(Bill payload)
  // {
  //   throw new NotImplementedException();
  // }
  // public async Task<BillingResponse> getByidInclude(Guid id)
  // {
  //   try
  //   {
  //     var billing = await _billingRepository.Find(billing => billing.Id.Equals(id), new string[] { "Category" });
  //
  //     if (billing is null) throw new Exception("billing not found");
  //
  //     BillingResponse response = new()
  //     {
  //       Id = billing.Id,
  //       BillName = billing.BillName,
  //       BillAmount = billing.BillAmount,
  //       EachBilledDate = billing.BillDate,
  //     };
  //
  //     return response;
  //   }
  //   catch (Exception e)
  //   {
  //     Console.WriteLine(e);
  //     throw;
  //   }
  // }

  public async Task<IEnumerable<Bill>> GetAll(Guid userId)
  {
    var billing = await _billingRepository.FindAll(bill =>bill.UserId.Equals(userId) );

    return billing;
  }


  public async Task<Bill> Update(Bill payload)
  {
    var billing = GetById(payload.Id);
    if (billing is null) throw new Exception("Sub Category not found.");

    //var attach = _billingRepository.Attach(payload);
    var updateData = _billingRepository.Update(payload);
    await _persistence.SaveChangesAsync();
    return updateData;
  }

  public async Task DeleteById(Guid id)
  {
    var billing = await _billingRepository.FindById(id);
    if (billing is null) throw new Exception("billing not found");
    _billingRepository.Delete(billing);
    await _persistence.SaveChangesAsync();
  }
}