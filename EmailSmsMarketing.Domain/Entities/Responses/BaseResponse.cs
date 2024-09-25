
using EmailSmsMarketing.Domain.Enum;

namespace EmailSmsMarketing.Domain.Entities.Responses
{
    public class BaseResponse
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
