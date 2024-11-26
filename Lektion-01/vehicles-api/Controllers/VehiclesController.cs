using Microsoft.AspNetCore.Mvc;

namespace vehicles_api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class VehiclesController : ControllerBase
{
  [HttpGet()]
  public ActionResult ListVehicles()
  {
    var demo = new { success = true, data = "Det funkar fortfarande!" };
    return Ok(demo);
    // return Ok(new { success = true, data = "Det funkar fortfarande!" });
  }
}