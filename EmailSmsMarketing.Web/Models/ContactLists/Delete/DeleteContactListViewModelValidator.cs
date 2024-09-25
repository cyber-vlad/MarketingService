using FluentValidation;

namespace EmailSmsMarketing.Web.Models.ContactLists.Delete
{
    public class DeleteContactListViewModelValidator : AbstractValidator<DeleteContactListViewModel>
    {
        public DeleteContactListViewModelValidator()
        {
            RuleFor(x => x.ConfirmName)
           .MinimumLength(1)
           .MaximumLength(50)
           .Equal(x => x.Name).WithMessage("Titles mush match!")
           .NotNull();
        }
    }
}
