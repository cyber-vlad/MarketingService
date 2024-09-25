using EmailSmsMarketing.Web.Models.ForgotPassword;
using FluentValidation;

namespace EmailSmsMarketing.Web.Models.ChangePassword
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
           .MinimumLength(5)
           .MaximumLength(25)
           .NotNull();

            RuleFor(x => x.NewPassword)
           .MinimumLength(5)
           .MaximumLength(25)
           .NotNull();

            RuleFor(x => x.ConfirmNewPassword)
           .MinimumLength(5)
           .MaximumLength(25)
           .Equal(x => x.NewPassword)
           .NotNull();
        }
    }
}
