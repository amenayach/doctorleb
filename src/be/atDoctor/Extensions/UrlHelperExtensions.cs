using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atDoctor.Controllers;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        /// <summary>
        /// Get user id for the logged user
        /// </summary>
        /// <param name="principal">The user principal</param>
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal?.Claims?.FirstOrDefault(m => m.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                return Guid.Parse(userId);
            }

            return Guid.Empty;
        }
    }
}
