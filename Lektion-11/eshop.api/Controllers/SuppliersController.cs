using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  [HttpGet()]
  public async Task<ActionResult> ListSupplier()
  {
    var suppliers = await _context.Suppliers.ToListAsync();
    return Ok(new { success = true, data = suppliers });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetSupplier(int id)
  {
    var supplier = await _context.Suppliers.SingleOrDefaultAsync(c => c.SupplierId == id);
    return Ok(new { success = true, data = supplier });
  }

  [HttpPost()]
  public async Task<ActionResult> AddSupplier(SupplierPostViewModel model)
  {
    var supplier = new Supplier()
    {
      Name = model.Name
    };

    await _context.Suppliers.AddAsync(supplier);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierId }, supplier);
  }

  [HttpPost("product")]
  public async Task<ActionResult> AddSupplierProduct(SupplierProductPostViewModel model)
  {
    var supplierIngredient = new SupplierIngredient
    {
      IngredientId = model.IngredientId,
      SupplierId = model.SupplierId,
      ItemNumber = model.ItemNumber,
      Size = model.Size,
      Unit = model.Unit,
      Price = model.Price
    };

    await _context.SupplierIngredients.AddAsync(supplierIngredient);
    await _context.SaveChangesAsync();

    return StatusCode(201, supplierIngredient);
  }
}
