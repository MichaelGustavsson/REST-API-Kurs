using eshop.api.Entities;
using eshop.api.Services;
using eshop.api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
  private readonly UserManager<User> _userManager;
  private readonly TokenService _tokenService;
  public AccountsController(UserManager<User> userManager, TokenService tokenService) // tokenService = new TokenService(???,???);
  {
    _tokenService = tokenService;
    _userManager = userManager;
  }

  [HttpPost("register")]
  public async Task<ActionResult> RegisterUser(RegisterUserViewModel model)
  {
    // Skapa en ny användare (IdentityUser(User))...
    model.UserName = model.Email;
    var user = new User
    {
      UserName = model.UserName,
      Email = model.Email,
      FirstName = model.FirstName,
      LastName = model.LastName,
    };

    var result = await _userManager.CreateAsync(user, model.Password);

    if (!result.Succeeded)
    {
      return StatusCode(500, "Det gick åt H-E");
    }

    await _userManager.AddToRoleAsync(user, "User");

    return StatusCode(201);
  }

  [HttpPost("login")]
  public async Task<ActionResult> LoginUser(LoginViewModel model)
  {
    // Steg 1. Hämta användaren om hen finns...
    var user = await _userManager.FindByNameAsync(model.UserName);

    // Steg 2. Kontrollera om användare fanns
    if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
    {
      return Unauthorized(new { success = false, message = "Unauthorized" });
    }

    return Ok(new { success = true, email = user.Email, token = await _tokenService.CreateToken(user) });
  }
}
