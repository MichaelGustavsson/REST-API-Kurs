namespace eshop.api.Entities;

public class Supplier
{
  public int SupplierId { get; set; }
  public string Name { get; set; }
  public List<SupplierIngredient> SupplierIngredients { get; set; }
}
