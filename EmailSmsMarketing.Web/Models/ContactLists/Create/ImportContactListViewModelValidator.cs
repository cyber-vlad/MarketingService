using EmailSmsMarketing.Web.Models.ChangePassword;
using FluentValidation;

namespace EmailSmsMarketing.Web.Models.ContactLists.Create
{
    public class ImportContactListViewModelValidator : AbstractValidator<ImportContactListViewModel>
    {
        public ImportContactListViewModelValidator()
        {
            RuleFor(x => x.Name)
           .MinimumLength(1)
           .MaximumLength(50)
           .NotNull();
        }
    }
}
