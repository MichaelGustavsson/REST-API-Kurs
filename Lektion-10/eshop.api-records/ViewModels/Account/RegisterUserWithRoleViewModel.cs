namespace eshop.api.ViewModels.Account;

public record RegisterUserWithRoleViewModel : RegisterUserViewModel
{
  public string RoleName { get; set; }
}
