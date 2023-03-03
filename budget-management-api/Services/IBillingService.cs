using budget_management_api.DTOs;
using budget_management_api.DTOS;
using budget_management_api.Entities;

namespace budget_management_api.Services;

public interface IBillingService
{
  Task<BillingResponse> Create(Bill payload);
  Task<BillingResponse> GetById(Guid id);
  Task<IEnumerable<Bill>> GetAll(Guid Userid);
  Task<Bill> Update(Bill payload);
  Task DeleteById(Guid id);
}