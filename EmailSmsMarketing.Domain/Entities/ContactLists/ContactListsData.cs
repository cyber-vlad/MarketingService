using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.ContactLists
{
    public class ContactListsData
    {
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int Email { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string ContactsData { get; set; }
        public string Token { get; set; }

    }
}
