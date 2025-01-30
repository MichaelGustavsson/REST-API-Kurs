using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.api.Entities;

public class SupplierIngredient
{
  public int SupplierId { get; set; }
  public int IngredientId { get; set; }
  public string ItemNumber { get; set; }
  public int Size { get; set; }
  public string Unit { get; set; }
  public decimal Price { get; set; }
  public Ingredient Ingredient { get; set; }
  public Supplier Supplier { get; set; }
}
