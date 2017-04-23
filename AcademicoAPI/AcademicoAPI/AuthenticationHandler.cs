using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace AcademicoAPI
{
    public class AuthenticationHandler : JwtBearerEvents
    {
        public override Task TokenValidated(TokenValidatedContext context)
        {
            var userName = context.Ticket.Principal.Identity.Name;
            var valid = context.SecurityToken.ValidTo;


            //var dbContext = context.HttpContext.RequestServices.GetService(typeof(EFContext)) as EFContext;

            //if (!dbContext.Users.Any(u => u.UserName == userName))
            //{
            //## context.Response.StatusCode = 401;
            //## context.SkipToNextMiddleware();
            // }

            return Task.FromResult(0);
        }
    }
}
