using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicles_api.Data;
using vehicles_api.Entities;

namespace vehicles_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturersController : ControllerBase
{
  // Dependency Injection...
  // Vi behöver hjälp att få tag i VehicleContext...
  // Skapar ett objekt av typen VehicleContext och placerar
  // objekter i argument context...
  private readonly VehicleContext _context;
  public ManufacturersController(VehicleContext context)
  {
    _context = context;
  }

  [HttpGet()]
  public async Task<ActionResult> ListManufacturers()
  {
    var manufacturers = await _context.Manufacturers
      .Select(x => new
      {
        x.Id,
        x.Name
      })
      .ToListAsync();
    return Ok(new { success = true, data = manufacturers });
  }

  // Skapa en HttpGet metod som hämtar en tillverkar baserat på ett id...
  // men jag vill att ni bara returnerar namnet på tillverkaren inget annnat!!!
  // Endast en tillverkare SKA hämtas...
  [HttpGet("{id}")]
  public async Task<ActionResult> FindManufacturer(int id)
  {
    var manufacturer = await _context.Manufacturers
      .Where(m => m.Id == id)
      .Select(m => new
      {
        m.Name
      })
    .SingleOrDefaultAsync();


    return Ok(new { success = true, data = manufacturer });
  }

  [HttpGet("vehicles/")]
  public async Task<ActionResult> ListVehicles()
  {
    var manufacturers = await _context.Manufacturers
      .Include(m => m.Vehicles)
      .ToListAsync();
    return Ok(new { success = true, data = manufacturers });
  }

  [HttpPost()]
  public async Task<ActionResult> Add(Manufacturer manufacturer)
  {
    _context.Manufacturers.Add(manufacturer);

    await _context.SaveChangesAsync();

    // Returnerar en statuskod 201
    return CreatedAtAction(nameof(FindManufacturer), new { id = manufacturer.Id }, manufacturer);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(int id, Manufacturer manufacturer)
  {
    // Steg 1. Jag måste hämta den raden i tabellen som ska uppdateras...
    var toUpdate = await _context.Manufacturers.FindAsync(id);

    // Steg 2. Uppdatera den raden med data som kom in via argumentlistan...
    toUpdate.Name = manufacturer.Name;

    // Steg 3. Spara och returnera statuskod 204...
    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(int id)
  {
    var toDelete = await _context.Manufacturers.FindAsync(id);

    _context.Manufacturers.Remove(toDelete);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}