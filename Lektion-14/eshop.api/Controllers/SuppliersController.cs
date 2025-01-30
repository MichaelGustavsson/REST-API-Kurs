using eshop.api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
  private readonly DataContext _context;
  public SuppliersController(DataContext context)
  {
    _context = context;
  }

  [HttpGet()]
  public async Task<ActionResult> ListAllSuppliers()
  {
    // Steg 1.
    var suppliers = await _context.Suppliers
      .Select(supplier => new
      {
        supplier.Id,
        supplier.Name,
        supplier.Email,
        supplier.Phone
      })
      .ToListAsync();

    return Ok(new { success = true, data = suppliers });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetSupplier(int id)
  {
    // Steg 1.
    var supplier = await _context.Suppliers
      .Include(s => s.SupplierAddresses)
      .Select(supplier => new
      {
        supplier.Id,
        supplier.Name,
        supplier.Email,
        supplier.Phone,
        Addresses = supplier.SupplierAddresses
          .Select(address => new
          {
            address.Address.AddressLine,
            address.Address.PostalAddress.PostalCode,
            address.Address.PostalAddress.City
          })
      })
      .SingleOrDefaultAsync(s => s.Id == id);

    return Ok(new { success = true, data = supplier });
  }
}
