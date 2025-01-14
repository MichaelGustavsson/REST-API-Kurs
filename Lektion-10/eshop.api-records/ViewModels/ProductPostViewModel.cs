namespace eshop.api.ViewModels;

public record ProductPostViewModel
{
  public string ItemNumber { get; set; }
  public string ProductName { get; set; }
  public double Price { get; set; }
  public string Description { get; set; }
  public string Image { get; set; }
}
