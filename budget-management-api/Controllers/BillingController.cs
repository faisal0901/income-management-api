using System.Net;
using System.Security.Claims;
using budget_management_api.Dtos;
using budget_management_api.DTOS;
using budget_management_api.Entities;
using budget_management_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/bills")]
[Authorize]
public class BillingController : ControllerBase
{
  private readonly IBillingService _billingService;

  public BillingController(IBillingService billingService)
  {
    _billingService = billingService;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] Bill billing)
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    billing.UserId = Guid.Parse(userId);
    var newBill = await _billingService.Create(billing);
    var response = new CommonResponse<BillingResponse>
    {
      StatusCode = (int)HttpStatusCode.Created,
      Message = "Successfully Created Billing",
      Data = newBill
    };
    return Created("/api/bills", response);
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var subCategories = await _billingService.GetAll(Guid.Parse(userId));
    var response = new CommonResponse<IEnumerable<Bill>>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get bills",
      Data = subCategories
    };
    return Ok(response);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(Guid id)
  {

    var billing = await _billingService.GetById(id);
  
    var response = new CommonResponse<BillingResponse>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "successfully get bills",
      Data = billing
    };
    return Ok(response);
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] Bill billing)
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    billing.UserId = Guid.Parse(userId);
    var updated = await _billingService.Update(billing);
    var response = new CommonResponse<Bill>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "Successfully Updated Billing.",
      Data = updated
    };
    return Ok(response);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteById(Guid id)
  {
    await _billingService.DeleteById(id);
    var response = new CommonResponse<string?>
    {
      StatusCode = (int)HttpStatusCode.OK,
      Message = "Successfully Deleted Billing.",
    };
    return Ok(response);
  }
}