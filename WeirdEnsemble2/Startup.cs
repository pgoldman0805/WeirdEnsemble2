using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
//Add these to your using block to set up IdentityFramework in Owin Startup:
using Owin;
using WeirdEnsemble2;
// This is an assembly decorator - it adds some metadata
// to the DLL to indicate what the startup class is called
[assembly: OwinStartup(typeof(Startup))]
namespace WeirdEnsemble2
{
    public class Startup
    {
        // My startup class contains a configuration method
        // which will "set-up" authentication for my app.
        public void Configuration(Owin.IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/SignIn")
            });
            app.CreatePerOwinContext(() =>
            {
                UserStore<IdentityUser> store = new UserStore<IdentityUser>();
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store)
                {
                    UserTokenProvider = new EmailTokenProvider<IdentityUser>(),
                    PasswordValidator = new PasswordValidator
                    {
                        RequiredLength = 4,
                        RequireDigit = false,
                        RequireUppercase = false,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false
                    }
                };
                return manager;
            });
        }
    }
}