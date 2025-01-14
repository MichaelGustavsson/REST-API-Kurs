namespace eshop.api.ViewModels;

public record PatchSalesOrderViewModel
{
  public int ProductId { get; set; }
  public double Price { get; set; }
}
