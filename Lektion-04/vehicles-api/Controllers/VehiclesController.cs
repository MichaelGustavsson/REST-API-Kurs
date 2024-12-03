using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vehicles_api.Data;
using vehicles_api.Entities;

namespace vehicles_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class VehiclesController : ControllerBase
{
  // Lista alla fordon som finns i databasen...
  private readonly VehicleContext _context;
  public VehiclesController(VehicleContext context)
  {
    _context = context;
  }

  [HttpGet()]
  // async nyckelordet gör att metoden kommer att köras asynkront
  // det vill säga inte blockera(icke-blockerande kod).
  // Task indikerar att vi kommer för eller senare returnera något.
  // Vi kan inte garantera när detta sker, men det kommer...
  public async Task<ActionResult> ListVehicles()
  {
    try
    {
      var vehicles = await _context.Vehicles
      .Include(v => v.Manufacturer)
      .Select(vehicle => new // Projicering av data så det passar vårt ändamål...
      { // Vi skapar ett nytt C# objekt...
        vehicle.Id,
        regNo = vehicle.RegistrationNumber,
        manufacturer = vehicle.Manufacturer.Name,
        modelType = vehicle.Model,
        vehicleType = vehicle.Manufacturer.Name + " " + vehicle.Model,
        vehicle.ModelYear,
        vehicle.Mileage,
        vehicle.ImageUrl,
        vehicle.Value
      })
      .ToListAsync(); // Skickar frågan till databasen...
      return Ok(new { success = true, data = vehicles });
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }

  }

  // Skapa en ny async endpoint metod som hämtar en bil baserat på bilens id...
  // Presentera samma information som ovan...
  [HttpGet("{id}")]
  public async Task<ActionResult> Find(int id)
  {
    try
    {
      var vehicle = await _context.Vehicles
      .Where(v => v.Id == id)
      .Include(m => m.Manufacturer)
      .Select(vehicle => new
      {
        vehicle.Id,
        regNo = vehicle.RegistrationNumber,
        manufacturer = vehicle.Manufacturer.Name,
        modelType = vehicle.Model,
        vehicle.ModelYear,
        vehicle.Mileage,
        vehicle.ImageUrl,
        vehicle.Value
      })
      .SingleOrDefaultAsync();

      if (vehicle is not null) return Ok(new { success = true, data = vehicle });

      return NotFound($"Tyvärr vi kunde inte hitta någon bil med id {id}, har angivit fel id?");
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }

  }

  [HttpGet("regno/{regNo}")]
  public async Task<ActionResult> FindByRegNumber(string regNo)
  {
    try
    {
      var vehicle = await _context.Vehicles
      .Where(v => v.RegistrationNumber.ToLower() == regNo.ToLower())
      .Include(m => m.Manufacturer)
      .Select(vehicle => new
      {
        vehicle.Id,
        regNo = vehicle.RegistrationNumber,
        manufacturer = vehicle.Manufacturer.Name,
        modelType = vehicle.Model,
        vehicle.ModelYear,
        vehicle.Mileage,
        vehicle.ImageUrl,
        vehicle.Value
      })
      .SingleOrDefaultAsync();

      if (vehicle is not null) return Ok(new { success = true, data = vehicle });

      return NotFound($"Tyvärr vi kunde inte hitta någon bil med registreringsnummer {regNo}, har angivit fel registreringsnummer?");
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }

  }

  [HttpPost()]
  public async Task<ActionResult> Add(Vehicle vehicle)
  {
    try
    {
      var exists = await _context.Vehicles.FirstOrDefaultAsync(c => c.RegistrationNumber == vehicle.RegistrationNumber);

      if (exists is not null) return BadRequest($"Det finns redan en bil med registreringsnummer {vehicle.RegistrationNumber} i systemet.");

      var manufacturer = await _context.Manufacturers.FindAsync(vehicle.ManufacturerId);

      vehicle.Manufacturer = manufacturer;
      _context.Vehicles.Add(vehicle);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(Find), new { id = vehicle.Id }, new
      {
        vehicle.Id,
        regNo = vehicle.RegistrationNumber,
        manufacturer = vehicle.Manufacturer.Name,
        modelType = vehicle.Model,
        vehicle.ModelYear,
        vehicle.Mileage,
        vehicle.ImageUrl,
        vehicle.Value
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(int id, Vehicle vehicle)
  {
    try
    {
      var toUpdate = await _context.Vehicles.FindAsync(id);

      if (toUpdate is null)
      {
        return BadRequest($"Tyvärr vi kunde inte hitta någon bil med id {id}");
      }

      toUpdate.RegistrationNumber = vehicle.RegistrationNumber;
      toUpdate.Model = vehicle.Model;
      toUpdate.ModelYear = vehicle.ModelYear;
      toUpdate.Mileage = vehicle.Mileage;
      toUpdate.ImageUrl = vehicle.ImageUrl;
      toUpdate.Value = vehicle.Value;
      toUpdate.Description = vehicle.Description;

      await _context.SaveChangesAsync();

      return NoContent();
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult> Patch(int id, Vehicle vehicle)
  {
    try
    {
      var toUpdate = await _context.Vehicles.FindAsync(id);

      if (toUpdate is null)
      {
        return BadRequest($"Tyvärr vi kunde inte hitta någon bil med id {id}");
      }

      toUpdate.RegistrationNumber = vehicle.RegistrationNumber;
      toUpdate.Mileage = vehicle.Mileage;

      await _context.SaveChangesAsync();

      return NoContent();
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }

  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(int id)
  {
    try
    {
      var toDelete = await _context.Vehicles.FindAsync(id);
      if (toDelete is not null)
      {
        _context.Vehicles.Remove(toDelete);
        await _context.SaveChangesAsync();
      }
      return NoContent();
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Ett fel inträffade i vårt system, {ex.Message}");
    }
  }
}