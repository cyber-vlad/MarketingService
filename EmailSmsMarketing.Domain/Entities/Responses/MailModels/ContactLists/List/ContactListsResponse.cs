using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSmsMarketing.Domain.Entities.ContactLists;

namespace EmailSmsMarketing.Domain.Entities.Responses.MailModels.ContactLists.List
{
    public class ContactListsResponse : BaseResponse
    {
        public List<ContactListsData> ContactsLists { get; set; }
    }
}
