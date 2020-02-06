using dl.wm.suite.common.dtos.Vms.Accounts;
using FluentValidation;

namespace dl.wm.suite.auth.api.Validators
{
  public class UserForRegistrationValidator : AbstractValidator<UserForRegistrationUiModel>
  {
    public UserForRegistrationValidator()
    {
      RuleFor(x => x.Login).EmailAddress();
      RuleFor(x => x.Password).Length(4, 12);
    }
  }
}