using Microsoft.AspNetCore.Mvc;
using vehicles_api.Models;
using vehicles_api.Utilities;

namespace vehicles_api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class VehiclesController : ControllerBase
{
  private readonly IWebHostEnvironment _environment;

  // Constructor dependency injection
  public VehiclesController(IWebHostEnvironment environment)
  {
    _environment = environment;
  }

  // http://localhost:5001/api/v1/vehicles
  [HttpGet()]
  public ActionResult ListVehicles()
  {
    // Skapa en url som pekar på den fysiska sökvägen till vehicles.json
    var path = string.Concat(_environment.ContentRootPath, "/Data/vehicles.json");
    var vehicles = Storage<Vehicle>.ReadJson(path);

    return Ok(new { success = true, data = vehicles });
  }

  // http://localhost:5001/api/v1/vehicles/12
  [HttpGet("{id}")]
  public ActionResult FindVehicle(int id)
  {
    // Skapa en url som pekar på den fysiska sökvägen till vehicles.json
    var path = string.Concat(_environment.ContentRootPath, "/Data/vehicles.json");
    var vehicles = Storage<Vehicle>.ReadJson(path);

    var vehicle = vehicles.SingleOrDefault(v => v.Id == id);

    // Defensiv programmering
    if (vehicle is not null)
    {
      return Ok(new { success = true, data = vehicle });
    }

    return NotFound();
  }

}