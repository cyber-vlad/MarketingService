using static EmailSmsMarketing.Domain.Enum.Enums;

namespace EmailSmsMarketing.Web.Common.Service
{
    public interface ICurrentUserService
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Picture { get; set; }
        public EnUiLanguage EnUiLanguage { get; set; }
        public bool IsAuthenticated { get; }
        public int UserId { get; }
        public bool IsAdministrator { get; }
    }
}
