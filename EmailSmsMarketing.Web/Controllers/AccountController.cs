using EmailSmsMarketing.Domain.Entities.Authentication;
using EmailSmsMarketing.Domain.Entities.Responses;
using EmailSmsMarketing.Domain.Enum;
using EmailSmsMarketing.Web.Models.ForgotPassword;
using EmailSmsMarketing.Web.Models.AuthModels;
using EmailSmsMarketing.Web.Models.User;
using EmailSmsMarketing.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;
using EmailSmsMarketing.Domain.Entities.UserInfo;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;
using static EmailSmsMarketing.Domain.Enum.Enums;

namespace EmailSmsMarketing.Web.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel uLogin)
        {
            if (ModelState.IsValid)
            {
                var authData = new AuthData { Email = uLogin.Username, Password = uLogin.Password };
                var authResponse = await accountQuery.Authorize(authData);
                
                if(authResponse.ErrorCode == ErrorCode.NoError)
                {
                    List<Claim> userClaims = new List<Claim>();
                    userClaims.Add(new Claim("ID", authResponse.User.ID.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, authResponse.User.ID.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Email, authResponse.User.Email));
                    userClaims.Add(new Claim(ClaimTypes.Name, authResponse.User.FirstName + " " + authResponse.User.LastName));
                    //userClaims.Add(new Claim("FullName", authResponse.User.FirstName + " " + authResponse.User.LastName));
                    userClaims.Add(new Claim("FirstName", authResponse.User.FirstName));
                    userClaims.Add(new Claim("LastName", authResponse.User.LastName));
                    userClaims.Add(new Claim("Company", authResponse.User.Company));
                    userClaims.Add(new Claim("PhoneNumber", authResponse.User.PhoneNumber));
                    userClaims.Add(new Claim("UiLanguage", authResponse.User.UiLanguage.ToString()));
                    userClaims.Add(new Claim("Picture", "/assets/images/no-photo.jpg"));
                    userClaims.Add(new Claim(".AspNetCore.ESM", authResponse.Token));

                    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(authResponse.User.UiLanguage.ToString())), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else if(authResponse.ErrorCode == ErrorCode.User_name_not_found_or_incorrect_password)
                    TempData["ErrorMessage"] = "Username or password is incorrect!";


            }
            return View("~/Views/Account/Login.cshtml", uLogin);
        }
      
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View("~/Views/Account/ForgotPassword.cshtml"); 
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel data)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    ForgotPasswordData resetPassword = new ForgotPasswordData()
                    {
                        Email = data.Email.Trim()/*.ToLower()*/,
                    };

                    var response = await accountQuery.ForgotPassword(resetPassword);

                    if (response.ErrorCode == ErrorCode.Internal_error)
                        return View("~/Views/Account/ForgotPassword.cshtml", data);
                    else
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    
                }
                return View("~/Views/Account/ForgotPassword.cshtml", data);
            }
            catch (Exception ex)
            {
                BaseResponse errorResponse = new BaseResponse()
                {
                    ErrorCode = ErrorCode.Internal_error,
                    ErrorMessage = ex.Message,
                };

                return PartialView("~/Views/Shared/Error.cshtml", errorResponse);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LockScreen()
        {
            UserClaimViewModel user = new UserClaimViewModel();
            var userClaim = GetUserClaims();

            if (userClaim.Result.Id != 0)
            {
                user = new UserClaimViewModel()
                {
                    Email = userClaim.Result.Email,
                    FullName = userClaim.Result.FullName,
                    Id = userClaim.Result.Id,
                    //UiLanguage = userClaim.Result.UiLanguage,
                    Picture = userClaim.Result.Picture,
                };

                TempData["User"] = JsonConvert.SerializeObject(user);

            }
            else if (userClaim.Result.Id == 0 && TempData["User"] != null)
            {

                var tempData = JsonConvert.DeserializeObject<UserClaimViewModel>(TempData["User"].ToString());
                user = new UserClaimViewModel()
                {
                    Email = tempData.Email,
                    FullName = tempData.FullName,
                    Id = tempData.Id,
                    UiLanguage = tempData.UiLanguage,
                    Picture = tempData.Picture,
                };

                TempData["UserRewrite"] = JsonConvert.SerializeObject(user);
            }
            else if (TempData["UserRewrite"] != null)
            {
                var tempData = JsonConvert.DeserializeObject<UserClaimViewModel>(TempData["UserRewrite"].ToString());
                user = new UserClaimViewModel()
                {
                    Email = tempData.Email,
                    FullName = tempData.FullName,
                    Id = tempData.Id,
                    UiLanguage = tempData.UiLanguage,
                    Picture = tempData.Picture,
                };
            }

            await HttpContext.SignOutAsync();

            return View("~/Views/Account/LockScreen.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> LockScreen(UserClaimViewModel userClaim)
        {
            
            AuthViewModel authViewModel = new AuthViewModel()
            {
                Username = userClaim.Email.ToLower().Trim(),
                Password = userClaim.Password,
            };

            var validator = new AuthViewModelValidator();
            var validationResult = validator.Validate(authViewModel);

            if (validationResult.IsValid)
            {
                AuthData dataQuery = new AuthData()
                {
                    Email = authViewModel.Username,
                    Password = authViewModel.Password,
                };
                var response = await accountQuery.Authorize(dataQuery);
                if (response.ErrorCode == ErrorCode.NoError)
                {
                    if (response.User != null)
                    {
                        List<Claim> userClaims = new List<Claim>();
                        userClaims.Add(new Claim("ID", response.User.ID.ToString()));
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, response.User.ID.ToString()));
                        userClaims.Add(new Claim(ClaimTypes.Email, response.User.Email));
                        userClaims.Add(new Claim(ClaimTypes.Name, response.User.FirstName + " " + response.User.LastName));
                        userClaims.Add(new Claim("FullName", response.User.FirstName + " " + response.User.LastName));
                        userClaims.Add(new Claim("FirstName", response.User.FirstName));
                        userClaims.Add(new Claim("LastName", response.User.LastName));
                        userClaims.Add(new Claim("Company", response.User.Company));
                        userClaims.Add(new Claim("PhoneNumber", response.User.PhoneNumber));
                        //userClaims.Add(new Claim("UiLanguage", uiLanguage.ToString()));
                        userClaims.Add(new Claim("Picture", "/assets/images/no-photo.jpg"));
                        userClaims.Add(new Claim(".AspNetCore.ESM", response.Token));

                        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(response.User.UiLanguage.ToString())), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                        //adding claims to ClaimsIdentity
                        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                        //Adding claimsIdentity to ClaimsPrincipal
                        var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                        //SignIn to save Claims in cookie, and re-use it in refresh token, after restarting application
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                else if (response.ErrorCode == ErrorCode.User_name_not_found_or_incorrect_password)
                    TempData["ErrorMessage"] = "Username or password is incorrect!";
            }
            else TempData["ErrorMessage"] = "Invalid password!";

            return View("~/Views/Account/LockScreen.cshtml",userClaim); 
        }

        [HttpGet]
        public async Task<IActionResult> TokenLogout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
