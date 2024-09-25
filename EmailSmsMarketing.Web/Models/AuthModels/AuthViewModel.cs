using DevExtreme.AspNet.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmailSmsMarketing.Web.Models.AuthModels
{
    public class AuthViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
