using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using Entity = iot.solution.entity;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace host.iot.solution.Middleware
{
    public static class HeaderkeyAuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseHeaderkeyAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeaderkeyAuthorizationMiddleware>();
        }
    }

    public class HeaderkeyAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public HeaderkeyAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                if (context.Request.Path.Value.Equals("/")
                    || context.Request.Path.Value.Contains("/api/account/FilePath")
                    || context.Request.Path.Value.Contains("/api/account/refreshtoken")
                    || context.Request.Path.Value.Contains("/api/subscriber")
                    || context.Request.Path.Value.Contains("/api/account/login")
                    || context.Request.Path.Value.StartsWith("/wwwroot/")
                    || context.Request.Path.Value.Contains("/api/alert/addiotalert")
                    || context.Request.Path.Value.Contains("/api/account/adminlogin")) // Nikunj
                {
                    await _next.Invoke(context);
                    return;
                }

                component.helper.SolutionConfiguration.CompanyId = Guid.Parse(context.Request.Headers["company-id"]);
                if (component.helper.SolutionConfiguration.CompanyId == null || component.helper.SolutionConfiguration.CompanyId == Guid.Empty)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Header key is missing!");
                }

                if (context.User != null && context.User.Claims != null)
                {
                    component.helper.SolutionConfiguration.BearerToken = context.User.Claims.Where(c => c.Type == "IOT_CONNECT").FirstOrDefault().Value;
                    if (!string.IsNullOrWhiteSpace(component.helper.SolutionConfiguration.BearerToken) && component.helper.SolutionConfiguration.BearerToken.Equals("AdminUser"))
                    {
                        component.helper.SolutionConfiguration.CurrentUserId = Guid.Parse(context.User.Claims.Where(c => c.Type == "CURRENT_USERID").FirstOrDefault().Value);
                    }
                    else
                    {
                        JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                        var tokenS = hand.ReadJwtToken(component.helper.SolutionConfiguration.BearerToken);
                        var jsonValue = tokenS.Claims?.SingleOrDefault(p => p.Type == "user")?.Value;
                        Entity.UserDetail userDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<Entity.UserDetail>(jsonValue);
                        component.helper.SolutionConfiguration.CurrentUserId = Guid.Parse(userDetail.Id);
                        component.helper.SolutionConfiguration.SolutionId = Guid.Parse(userDetail.SolutionGuid);
                        component.helper.SolutionConfiguration.EntityGuid = Guid.Parse(userDetail.EntityGuid);
                    }
                }

                await _next.Invoke(context);
                return;

            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
    }
}
