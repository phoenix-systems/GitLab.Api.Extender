using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GitLab.Api.Extender.Infrastructure
{
    public class AddSwaggerAccessKeyHeaderParameter : IOperationFilter
    {
        void IOperationFilter.Apply(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out MethodInfo methodinfo);

            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(f => f.Filter).Any(f => f is AccessKeyAttribute);
            var authorizationRequired = methodinfo.GetCustomAttributes<AccessKeyAttribute>().Any();

            if (isAuthorized && authorizationRequired)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "key",
                    In = "header",
                    Description = "Access Key",
                    Required = true,
                    Type = "string"
                });
            }
        }
    }
}