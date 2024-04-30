using ERP.SERVICE.IRepositories.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SERVICE.Middleware
{
    public class PagePermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PagePermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserPagePermissionRepository userPagePermissionRepository)
        {
            // Check if the user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var pageId = context.Request.Path;

                // Check if the user has permission to access the page
                if (! await userPagePermissionRepository.HasPermission(userId, pageId))
                {
                    context.Response.Redirect("/AccessDenied");
                    return;
                }
            }
            await _next(context);
        }
    }

}

