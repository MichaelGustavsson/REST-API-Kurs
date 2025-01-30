namespace eshop.api.ViewModels;

public class SupplierProductPostViewModel
{
  public int IngredientId { get; set; }
  public int SupplierId { get; set; }
  public string ItemNumber { get; set; }
  public int Size { get; set; }
  public string Unit { get; set; }
  public decimal Price { get; set; }
}
