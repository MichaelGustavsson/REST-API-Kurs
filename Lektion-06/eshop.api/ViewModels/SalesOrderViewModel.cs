namespace eshop.api.ViewModels;

// Syftet är att använda denna klass för POST...
public class SalesOrderViewModel
{
  public DateTime OrderDate { get; set; }
  public IList<ProductViewModel> Products { get; set; }
}
