using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using GitLab.Api.Extender.Services;
using GitLab.Api.Extender.Infrastructure;

namespace GitLab.Api.Extender
{
    public class Startup
    {
        private const string _title = "GitLab.Api.Extender";
        private const string _description = "Api that extends gitlab ce api with functions required for CI/CD.";
        private const string _version = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var settings = Configuration.Get<Settings>();

            services
                .AddMemoryCache()
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_version, new Info { Title = _title, Description = _description, Version = _version });
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
                c.DescribeAllParametersInCamelCase();
                c.OperationFilter<AddSwaggerAccessKeyHeaderParameter>();
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{PlatformServices.Default.Application.ApplicationName}.xml"));
                c.AddFluentValidationRules();
            });

            var container = RegisterServices(services, settings);

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "swagger";
                x.SwaggerEndpoint($"/swagger/{_version}/swagger.json", _version);
            });
        }

        private IContainer RegisterServices(IServiceCollection services, Settings settings)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(settings.App.Access ?? new AccessKeySettings())
                .SingleInstance();

            builder.RegisterType<GitLabService>()
                .As<IGitLabService>()
                .WithParameter(TypedParameter.From(settings.App.Gitlab))
                .SingleInstance();

            builder.RegisterType<MimeMappingService>()
                .As<IMimeMappingService>()
                .SingleInstance();            

            builder.Populate(services);

            return builder.Build();
        }
    }
}