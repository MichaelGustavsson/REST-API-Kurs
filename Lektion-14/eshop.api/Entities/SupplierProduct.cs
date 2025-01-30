namespace eshop.api.Entities;

public class SupplierProduct
{
  public int ProductId { get; set; }
  public int SupplierId { get; set; }
  public string ItemNumber { get; set; }
  public decimal Price { get; set; }

  public Product Product { get; set; }
  public Supplier Supplier { get; set; }
}
