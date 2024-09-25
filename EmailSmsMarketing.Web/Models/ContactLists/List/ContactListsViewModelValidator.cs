using FluentValidation;
namespace EmailSmsMarketing.Web.Models.ContactLists.List
{
    public class ContactListsViewModelValidator : AbstractValidator<ContactListsViewModel>
    {
        public ContactListsViewModelValidator()
        {
           RuleFor(x => x.Name)
          .MinimumLength(1)
          .MaximumLength(50)
          .NotNull();
        }
    }
}
