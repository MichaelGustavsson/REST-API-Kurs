namespace eshop.api.ViewModels;

public record ProductGetViewModel
{
  public int ProductId { get; set; }
  public string ProductName { get; set; }
  public double Price { get; set; }
  public int Quantity { get; set; }
}
