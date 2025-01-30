namespace eshop.api.Entities;

public class Ingredient
{
  public int IngredientId { get; set; }
  public string ItemNumber { get; set; }
  public string Name { get; set; }
  public List<SupplierIngredient> SupplierIngredients { get; set; }
}
