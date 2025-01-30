using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  [HttpGet("{id}")]
  public async Task<ActionResult> GetIngredient(int id)
  {
    var ingredient = await _context.Ingredients.SingleOrDefaultAsync(c => c.IngredientId == id);
    return Ok(new { success = true, data = ingredient });
  }

  [HttpPost()]
  public async Task<ActionResult> AddIngredient(IngredientPostViewModel model)
  {
    var ingredient = new Ingredient
    {
      ItemNumber = model.ItemNumber,
      Name = model.Name
    };

    await _context.Ingredients.AddAsync(ingredient);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.IngredientId }, ingredient);
  }
}
