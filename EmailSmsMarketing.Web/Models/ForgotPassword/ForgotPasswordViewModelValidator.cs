using FluentValidation;

namespace EmailSmsMarketing.Web.Models.ForgotPassword
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(x => x.Email)
            .MinimumLength(5)
            .NotNull()
            .EmailAddress();
            //.WithName(Localization.Email);
        }
    }
}
