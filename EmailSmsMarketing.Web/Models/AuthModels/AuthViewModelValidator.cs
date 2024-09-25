using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
namespace EmailSmsMarketing.Web.Models.AuthModels
{
    public class AuthViewModelValidator : AbstractValidator<AuthViewModel>
    {
        public AuthViewModelValidator()
        {
            RuleFor(x => x.Username)
                .MinimumLength(5)
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .MinimumLength(5);
        }
    }
}
