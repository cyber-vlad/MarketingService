using static EmailSmsMarketing.Domain.Enum.Enums;
using System.Security.Claims;

namespace EmailSmsMarketing.Web.Common.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            string userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isValidGuid = Int32.TryParse(userId, out int parserUserId);
            bool value = false;
            bool.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("IsAdministrator"), out value);

            if (isValidGuid)
            {
                UserId = parserUserId;
            }

            FullName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            FirstName = httpContextAccessor.HttpContext?.User?.FindFirstValue("FirstName");
            LastName = httpContextAccessor.HttpContext?.User?.FindFirstValue("LastName");
            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            Company = httpContextAccessor.HttpContext?.User?.FindFirstValue("Company");
            //EnUiLanguage = Enum.Parse<EnUiLanguage>(httpContextAccessor.HttpContext?.User?.FindFirstValue("UiLanguage"));
            Picture = httpContextAccessor.HttpContext?.User?.FindFirstValue("Picture");
            IsAdministrator = value;

        }

        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Picture { get; set; }
        public EnUiLanguage EnUiLanguage { get; set; }
        public bool IsAuthenticated { get; }
        public int UserId { get; }
        public bool IsAdministrator { get; }
    }
}
