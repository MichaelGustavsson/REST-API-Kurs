namespace eshop.api.ViewModels.Account;

public record ChangePasswordViewModel
{
  public string UserName { get; set; }
  public string CurrentPassword { get; set; }
  public string NewPassword { get; set; }
}
