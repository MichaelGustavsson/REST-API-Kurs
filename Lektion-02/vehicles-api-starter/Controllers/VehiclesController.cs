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

    return NotFound();
  }

}