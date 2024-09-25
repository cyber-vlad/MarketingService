using EmailSmsMarketing.BusinessLogic.Queries;
using EmailSmsMarketing.Domain.Entities.Authentication;
using EmailSmsMarketing.Domain.Entities.Responses;
using EmailSmsMarketing.Domain.Entities.Settings;
using EmailSmsMarketing.Domain.Enum;
using EmailSmsMarketing.Web.Models.ChangePassword;
using EmailSmsMarketing.Web.Models.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Formats.Asn1.AsnWriter;

namespace EmailSmsMarketing.Web.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Settings/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = GetToken();

                    ChangePasswordData changePassword = new ChangePasswordData()
                    {
                        OldPassword = data.OldPassword,
                        NewPassword = data.NewPassword,
                        Token = token  
                    };

                    var response = await accountQuery.ChangePasswordAsync(changePassword);

                    if (response.ErrorCode == ErrorCode.Expired_token)
                    {
                        if (RefreshToken())
                        {
                            return await ChangePassword(data);
                        }
                    }
                    else if(response.ErrorCode == ErrorCode.Invalid_old_password)
                    {
                        TempData["ErrorMsg"] = "Old password is incorrect!";

                    }
                    else if(response.ErrorCode == ErrorCode.NoError)
                        TempData["SuccessMsg"] = "Password was successfully changed!";
                }              
                return View("~/Views/Settings/Index.cshtml");

            }
            catch
            {
                return View("~/Views/Settings/Index.cshtml");
            }
        }

    }
}
