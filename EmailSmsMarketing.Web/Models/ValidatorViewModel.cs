using EmailSmsMarketing.Web.Models.AuthModels;
using EmailSmsMarketing.Web.Models.ChangePassword;
using EmailSmsMarketing.Web.Models.ContactLists.Create;
using EmailSmsMarketing.Web.Models.ContactLists.Delete;
using EmailSmsMarketing.Web.Models.ContactLists.List;
using EmailSmsMarketing.Web.Models.ForgotPassword;
using FluentValidation;

namespace EmailSmsMarketing.Web.Models
{
    public class ValidatorViewModel
    {
        public ValidatorViewModel(IServiceCollection services)
        {
            services.AddTransient<IValidator<AuthViewModel>, AuthViewModelValidator>();
            services.AddTransient<IValidator<ForgotPasswordViewModel>, ForgotPasswordViewModelValidator>();
            services.AddTransient<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddTransient<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddTransient<IValidator<DeleteContactListViewModel>, DeleteContactListViewModelValidator>();
            services.AddTransient<IValidator<ContactListsViewModel>, ContactListsViewModelValidator>();
        
        }
    }
}
