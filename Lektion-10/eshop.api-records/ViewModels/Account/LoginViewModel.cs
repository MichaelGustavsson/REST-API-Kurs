namespace eshop.api.ViewModels.Account;

public record LoginViewModel
{
  public string UserName { get; set; }
  public string Password { get; set; }
}
