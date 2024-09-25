using EmailSmsMarketing.Domain.Entities.UserInfo;
using EmailSmsMarketing.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmailSmsMarketing.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            RefreshToken();
            return View("~/Views/Home/Index.cshtml");

            //var smsData = new List<ChartData>
            //{
            //    new ChartData { Category = "Rejected", Value = 0 },
            //    new ChartData { Category = "Failed delivery", Value = 0 },
            //    new ChartData { Category = "Waiting to send", Value = 0 },
            //    new ChartData { Category = "Sent this month", Value = 0 },
            //    new ChartData { Category = "Received this month", Value = 0 }
            //};

            //var emailData = new List<ChartData>
            //{
            //    new ChartData { Category = "Rejected", Value = 0 },
            //    new ChartData { Category = "Failed delivery", Value = 0 },
            //    new ChartData { Category = "Waiting to send", Value = 0 },
            //    new ChartData { Category = "Sent this month", Value = 0 },
            //    new ChartData { Category = "Received this month", Value = 0 }
            //};

            //ViewData["SmsData"] = smsData;
            //ViewData["EmailData"] = emailData;

            //return View();
        }



    }
}
