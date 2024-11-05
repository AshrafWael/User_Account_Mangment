using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using UserAccountMangment.DbHelper;
using UserAccountMangment.Models;

namespace UserAccountMangment.Authentication
{
    public class PermissionBasedAuthenticationFilter: IAuthorizationFilter
    {
        private readonly AccountContext _dbContext;

        public PermissionBasedAuthenticationFilter(AccountContext dbContext)
        {
           _dbContext = dbContext;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attrebute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata
                .FirstOrDefault (x => x is CheckPermissionAttribute);
            if (attrebute != null)
            {
                var CliamIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (CliamIdentity == null || !CliamIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else 
                {
                    var userId = Convert.ToString(CliamIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var hasPermission  = _dbContext.Set<UserPermission>()
                     .Any(x=> x.ApplicationUser.Id == userId && x.PermissionId == attrebute.Permission);
                    if (!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }
                }

                

            }
        }
    }
}
