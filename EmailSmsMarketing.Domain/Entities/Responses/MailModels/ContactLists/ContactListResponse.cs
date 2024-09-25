using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Responses.MailModels.ContactLists
{
    public class ContactListResponse
    {
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int Email { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string ContactsData { get; set; }
    }
}
