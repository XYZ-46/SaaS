using DataEntity.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace API.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AllMemberAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _ = (UserProfileModel?)context.HttpContext.Items["User"] ?? throw new HttpRequestException("Access Denied");
        }
    }
}
