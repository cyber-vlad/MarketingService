using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSmsMarketing.Domain.Enum;
using static EmailSmsMarketing.Domain.Enum.Enums;

namespace EmailSmsMarketing.Domain.Entities.UserInfo
{
    public class User
    {
        public string Company { get; set; }
        public int CompanyID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int ID { get; set; }
        public DateTime LastAuthorize { get; set; }
        public string LastAuthorizeIP { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Picture { get; set; }
        public EUserStatus Status { get; set; }
        public EnUiLanguage UiLanguage { get; set; }
    }
}
