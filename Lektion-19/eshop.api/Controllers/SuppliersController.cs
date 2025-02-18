using System.Reflection.Metadata.Ecma335;
using eshop.api.ViewModels.Address;
using eshop.api.ViewModels.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(IUnitOfWork unitOfWork) : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  [HttpGet()]
  public async Task<IActionResult> ListAllSuppliers()
  {
    try
    {
      return Ok(new { success = true, data = await _unitOfWork.SupplierRepository.List() });
    }
    catch (Exception ex)
    {
      return NotFound($"Tyvärr hittade vi inget {ex.Message}");
    }

  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetSupplier(int id)
  {
    try
    {
      return Ok(new { success = true, data = await _unitOfWork.SupplierRepository.Find(id) });
    }
    catch (Exception ex)
    {
      return NotFound(new { success = false, message = ex.Message });
    }
  }

  [HttpPost()]
  public async Task<IActionResult> AddSuppliers(SupplierPostViewModel model)
  {
    try
    {
      if (await _unitOfWork.SupplierRepository.Add(model))
      {
        if (_unitOfWork.HasChanges())
        {
          await _unitOfWork.Complete();
          return StatusCode(201);
        }
        else
        {
          return StatusCode(500);
        }
      }
      else
      {
        return BadRequest();
      }
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpPost("{id}/address")]
  public async Task<IActionResult> AddSupplierAddress(int id, AddressPostViewModel model)
  {
    try
    {
      if (await _unitOfWork.SupplierRepository.Add(id, model))
      {
        if (_unitOfWork.HasChanges())
        {
          await _unitOfWork.Complete();
          return StatusCode(201);
        }
        else
        {
          return BadRequest();
        }
      }
      else
      {
        return StatusCode(500);
      }
    }
    catch (Exception ex)
    {
      return StatusCode(400, ex.Message);
    }
  }

  [HttpPatch("{id}/address")]
  public async Task<IActionResult> UpdateSupplierAddress(int id, AddressPostViewModel model)
  {
    try
    {
      if (await _unitOfWork.SupplierRepository.Update(id, model))
      {
        if (_unitOfWork.HasChanges())
        {
          await _unitOfWork.Complete();
          return NoContent();
        }
        else
        {
          return StatusCode(500);
        }
      }
      else
      {
        return StatusCode(500);
      }
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete("{id}/address")]
  public async Task<IActionResult> DeleteSupplierAddress(int id, AddressPostViewModel model)
  {
    try
    {
      if (await _unitOfWork.SupplierRepository.Remove(id, model))
      {
        if (_unitOfWork.HasChanges())
        {
          await _unitOfWork.Complete();
          return NoContent();
        }
        else
        {
          return BadRequest();
        }
      }
      else
      {
        return StatusCode(500);
      }
    }
    catch (Exception ex)
    {
      return StatusCode(400, ex.Message);
    }
  }
}
