using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Responses
{
    public class QueryDataPost
    {
        public string URL { get; set; }
        public string JSON { get; set; }
        public string Credentials { get; set; }
    }
}
