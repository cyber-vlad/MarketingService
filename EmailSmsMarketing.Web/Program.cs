using EmailSmsMarketing.Web.Common.Service;
using EmailSmsMarketing.Web.Common;
using EmailSmsMarketing.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using FluentValidation.AspNetCore;

namespace EmailSmsMarketing.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDataProtection();
            builder.Services.AddRazorPages()
                .AddViewLocalization()
                    .AddSessionStateTempDataProvider();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            string relativeRedirectUri = new Uri(context.RedirectUri).PathAndQuery;

                            if (Utils.IsAjaxRequest(context.Request))
                            {
                                context.Response.Headers["Location"] = relativeRedirectUri;
                                context.Response.StatusCode = 401;
                            }
                            else
                            {
                                context.Response.Redirect(relativeRedirectUri);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddControllers().AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Program>();
                options.LocalizationEnabled = true;
            });

            ValidatorViewModel validatorViewModel = new ValidatorViewModel(builder.Services);

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
