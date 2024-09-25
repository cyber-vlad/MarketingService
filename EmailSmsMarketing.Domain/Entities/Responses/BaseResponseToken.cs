using EmailSmsMarketing.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Responses
{
    public class BaseResponseToken
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
