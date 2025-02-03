using eshop.api.Data;
using eshop.api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerRepository repo) : ControllerBase
{
  private readonly ICustomerRepository _repo = repo;

  [HttpGet()]
  public async Task<ActionResult> GetAllCustomers()
  {
    var customers = await _repo.List();
    return Ok(new { success = true, data = customers });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetCustomer(int id)
  {
    try
    {
      return Ok(new { success = true, data = await _repo.Find(id) });
    }
    catch (Exception ex)
    {
      return NotFound(new { success = false, message = ex.Message });
    }
  }

  [HttpPost()]
  public async Task<ActionResult> AddCustomer(CustomerPostViewModel model)
  {
    try
    {
      var result = await _repo.Add(model);
      if (result)
      {
        return StatusCode(201);
      }
      else
      {
        return BadRequest();
      }
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }

  }
}