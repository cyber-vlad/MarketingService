using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSmsMarketing.Domain.Entities.Chart
{
    public class ChartInfo
    {
        public int ID { get; set; }
        public int Rejected { get; set; }
        public int FailedDelivery { get; set; }
        public int WaitingToSend { get; set; }
        public int SentThisMonth { get; set; }
        public int ReceivedThisMonth { get; set; }

    }
}
