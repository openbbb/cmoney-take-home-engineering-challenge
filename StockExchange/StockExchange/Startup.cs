using AppPofile.Utility.ModelValidateion;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using StockExchange.Repository.Models;
using StockExchange.Utility;
using StockExchange.Utility.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace StockExchange
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.Filters.Add<ModelValidationAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    options.GroupNameFormat = "'v'VVV";

                    options.SubstituteApiVersionInUrl = true;
                });
                
            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    }

                    options.IncludeXmlComments(XmlCommentsFilePath);
                });


            services.AddDbContext<CMDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CMDBConnection"));
            });
    
            services.AddHttpClient();

            services.AddMvc().AddControllersAsServices();

            var builder = new ContainerBuilder();

            var domains = Assembly.Load("StockExchange.Domain");
            builder.RegisterAssemblyTypes(domains).AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            var repositories = Assembly.Load("StockExchange.Repository");
            builder.RegisterAssemblyTypes(repositories).AsImplementedInterfaces();

            builder.RegisterType<ModelValidationAttribute>();
            builder.RegisterType<GUIDVerifyAndRecreate>();

            builder.Populate(services);

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider, CMDBContext dbContext)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            dbContext.Database.EnsureCreated();

            app.ConfigureGlobalExceptionMiddleware();
            app.ConfigureSetupMiddleware();

            app.UseMvcWithDefaultRoute();

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();

            if (env.EnvironmentName == "Development")
            {
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });
            }
            else
            {
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/StockExchange/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        }
                    });
            }

        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            { 
                Title = $"Device WebService{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "cmoney-engineering-take-home-challenge",
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
