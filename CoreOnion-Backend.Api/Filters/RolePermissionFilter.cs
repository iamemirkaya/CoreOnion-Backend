using CoreOnion_Backend.Application.CustomAttributes;
using CoreOnion_Backend.Application.Interfaces.UserInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Security.Claims;

namespace CoreOnion_Backend.Api.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {

        readonly IUserReadRepository _userService;

        public RolePermissionFilter(IUserReadRepository userService)
        {
            _userService = userService;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;
                var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute.ActionType}.{attribute.Definition.Replace(" ", "")}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(userId, code);

                if (!hasRole)
                    context.Result = new UnauthorizedResult();
                else
                    await next();

            }
            else
            {
                await next();
            }
        }
    }
}
