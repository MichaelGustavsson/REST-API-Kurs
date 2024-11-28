using Microsoft.AspNetCore.Mvc;
using vehicles_api.Models;
using vehicles_api.Utilities;

namespace vehicles_api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class VehiclesController : ControllerBase
{
  private readonly IWebHostEnvironment _environment;

  public VehiclesController(IWebHostEnvironment environment)
  {
    _environment = environment;
  }

  [HttpGet()]
  public ActionResult ListVehicles()
  {
    var path = string.Concat(_environment.ContentRootPath, "/Data/vehicles.json");
    var vehicles = Storage<Vehicle>.ReadJson(path);

    return Ok(new { success = true, data = vehicles });
  }

  [HttpGet("{id}")]
  public ActionResult FindVehicle(int id)
  {
    var path = string.Concat(_environment.ContentRootPath, "/Data/vehicles.json");
    var vehicles = Storage<Vehicle>.ReadJson(path);

    var vehicle = vehicles.SingleOrDefault(v => v.Id == id);

    if (vehicle is not null)
    {
      return Ok(new { success = true, data = vehicle });
    }

    return NotFound(new { success = false, message = $"Tyvärr kunde vi inte hitta någon bil med id: {id}" });
  }

  // http://localhost:5001/api/v1/vehicles
  [HttpPost()]
  public ActionResult AddVehicle(Vehicle vehicle)
  {
    // 1. Vi måste få in det data som ska sparas✅
    // 2. Skapa kod för att lägga till en ny bil i vår json fil...✅
    var path = string.Concat(_environment.ContentRootPath, "/Data/vehicles.json");
    var vehicles = Storage<Vehicle>.ReadJson(path);

    // Skapa ett id genom att ta antalet bilar i listan och addera 1...✅
    vehicle.Id = vehicles.Count + 1;
    // Lägga till vår nya bil i listan...✅
    vehicles.Add(vehicle);

    // 3. Skriv ner den uppdaterade listan till json filen...✅
    Storage<Vehicle>.WriteJson(path, vehicles);

    // 4. Returnera korrekt statuskod till avsändaren (201 Created)...✅
    // HATEOS...
    // Mer korrekt sätta att returnera statuskod 201...
    return CreatedAtAction(nameof(FindVehicle), new { id = vehicle.Id }, vehicle);
    // return Created("http://localhost:5001/api/v1/vehicles/" + vehicle.Id, vehicle);
  }

}