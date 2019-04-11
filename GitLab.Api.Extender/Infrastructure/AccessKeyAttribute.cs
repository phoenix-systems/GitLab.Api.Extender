using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GitLab.Api.Extender.Infrastructure
{
    public class AccessKeyAttribute : ActionFilterAttribute
    {
        private readonly string _headerName = "key";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var key = "";
            var access = context.HttpContext.RequestServices.GetService<AccessKeySettings>();
            if (access == null)
            {
                throw new ArgumentException($"{nameof(AccessKeySettings)} are not injected");
            }
            if (string.IsNullOrEmpty(access.Key))
            {
                throw new ArgumentException($"{nameof(AccessKeySettings)}{nameof(access.Key)} can not be null or empty");
            }

            if (context.HttpContext.Request.Headers.ContainsKey(_headerName))
            {
                var headers = context.HttpContext.Request.Headers[_headerName];
                key = headers[0];
            }

            if (!access.Key.Equals(key))
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }
    }
}