namespace eshop.api.Entities;

public class SalesOrder
{
  public int SalesOrderId { get; set; }
  public DateTime OrderDate { get; set; }

  // Navigational property...
  public IList<OrderItem> OrderItems { get; set; }
}
