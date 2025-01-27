namespace eshop.api.ViewModels;

public class CustomerPostViewModel
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
  public string DeliveryAddress { get; set; }
  public string DeliveryPostalCode { get; set; }
  public string DeliveryCity { get; set; }
  public string InvoiceAddress { get; set; }
  public string InvoicePostalCode { get; set; }
  public string InvoiceCity { get; set; }
}
