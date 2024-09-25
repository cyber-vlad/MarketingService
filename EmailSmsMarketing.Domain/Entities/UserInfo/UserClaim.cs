using static EmailSmsMarketing.Domain.Enum.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.UserInfo
{
    public class UserClaim
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
