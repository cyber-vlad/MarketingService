using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSmsMarketing.Domain.Enum;
using EmailSmsMarketing.Domain.Entities.UserInfo;

namespace EmailSmsMarketing.Domain.Entities.Responses
{
    public class AuthResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }
}
