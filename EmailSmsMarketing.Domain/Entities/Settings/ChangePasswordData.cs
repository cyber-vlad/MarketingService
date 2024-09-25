using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Settings
{
    public class ChangePasswordData
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string Token { get; set; }
    }
}
