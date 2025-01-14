namespace eshop.api.ViewModels.Account;

public record RegisterUserViewModel : LoginViewModel
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
}
