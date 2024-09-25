using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Responses.MailModels.ContactLists
{
    public class ContactListByIdResponse : BaseResponse
    {
        public ContactListResponse ContactsList { get; set; }
    }
}
