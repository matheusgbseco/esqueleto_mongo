using Common.Domain.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.API.Extensions
{
    public class RequestTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, CurrentUser currentUser)
        {
            var token = context.Request.Headers["Authorization"];
            if (!token.IsNullOrEmpaty())
            {
                var tokenClear = token.ToString().Replace("Bearer ", "");
                var jwt = new JwtSecurityTokenHandler();
                var canRead = jwt.CanReadToken(tokenClear);
                if (canRead)
                {
                    try
                    {
                        var claims = GetClaimsFromUserPrincipal(context);

                        var claimsDictonary = new Dictionary<string, object>();
                        if (claims.IsAny())
                        {
                            foreach (var item in claims
                                .Select(_ => new KeyValuePair<string, object>(_.Type, _.Value)))
                            {
                                if (!claimsDictonary.ContainsKey(item.Key))
                                    claimsDictonary.Add(item.Key, item.Value);
                            }
                        }

                        var userId = context.Request.Headers["User-Id"];
                        if (!userId.IsNullOrEmpaty())
                            claimsDictonary.Add("sub", Guid.Parse(userId));

                        this.ConfigClaims(currentUser, tokenClear, claimsDictonary);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            await this._next.Invoke(context);
        }

        protected virtual void ConfigClaims(CurrentUser currentUser, string tokenClear, Dictionary<string, object> claimsDictonary)
        {
            currentUser.Init(tokenClear, claimsDictonary);
        }

        private static IEnumerable<Claim> GetClaimsFromUserPrincipal(HttpContext context)
        {
            var caller = context.User;
            var claims = caller.Claims;
            return claims;
        }

    }

    public static class RequestTokenMiddlewareExtension
    {
        public static IApplicationBuilder AddTokenMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestTokenMiddleware>();
        }
    }
}
