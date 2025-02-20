using eshop.api.ViewModels.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(IUnitOfWork unitOfWork) : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  /// <summary>
  /// List all suppliers in the system.
  /// </summary>
  /// <returns>A json document/object with all current suppliers</returns>
  /// <response code="200">A json object representing each supplier</response>
  /// <response code="404">If no suppliers are found</response>
  /// <response code="400">If input is in wrong format</response>
  /// <response code="500">We are experience server issues</response>
  [ProducesResponseType(typeof(SuppliersViewModel), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(400)]
  [ProducesResponseType(500)]
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

  /// <summary>
  /// Search for a supplier based on its unique id
  /// </summary>
  /// <param name="id">A suppliers unique id</param>
  /// <returns></returns>
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

  /// <summary>
  /// Adds a new supplier to the system
  /// </summary>
  /// <remarks>
  /// Sample request:
  /// 
  ///     POST /supplier
  ///     {
  ///       "name": "Jerkstrands",
  ///       "email": "kaka@gmail.com",
  ///       "phone": "010-484 99 00",
  ///       "addresses": [
  ///         {
  ///           "addressLine": " Violvägen 17",
  ///           "postalCode": "432 22",
  ///           "city": "Göteborg",
  ///           "addressType": "Distribution"
  ///         }
  ///       ]
  ///     }
  /// </remarks>
  /// <param name="model">A json document/object representing a new Supplier</param>
  /// <returns></returns>
  /// <response code="201">If the supplier was successfully added to the system</response>
  /// <response code="400">If the data was in the wrong format or the supplier already exists</response>
  /// <response code="500">If the server is not responding</response>
  /// 
  [ProducesResponseType(201)]
  [ProducesResponseType(400)]
  [ProducesResponseType(500)]
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
}
