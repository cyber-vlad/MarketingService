using static EmailSmsMarketing.Domain.Enum.Enums;

namespace EmailSmsMarketing.Web.Models.User
{
    public class UserClaimViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public EnUiLanguage UiLanguage { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Password { get; set; }
        public bool IsManaged { get; set; }
    }
}
