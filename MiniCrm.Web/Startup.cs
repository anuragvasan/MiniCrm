using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniCrm.Application.Customer.Commands;
using MiniCrm.Application.Customer.CommandValidators;
using MiniCrm.DataModel;
using MiniCrm.Persistence.Customer.MapperProfiles;
using MiniCrm.Web.Filters;
using MiniCrm.Web.ModelInitializer;

namespace MiniCrm.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            var assemblies = new[] {
                typeof(Program).Assembly, // MiniCrm.Web
                typeof(AddCustomer).Assembly, // MiniCrm.Application
                typeof(AddCustomerProfile).Assembly, // MiniCrm.Persistence
                typeof(Customer).Assembly }; // MiniCrm.DataModel

            var mvcBuilder = services.AddControllersWithViews(o =>
            {
                o.Filters.Add<ModelInitializerFilter>(0);
                o.Filters.Add<ModelStateValidationFilter>(1);
            })
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssemblies(assemblies);
                });

            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddDbContext<CrmContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Crm")));

            services.AddMediatR(assemblies);
            services.AddAutoMapper(assemblies);

            services.Scan(scan =>
                scan.FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(typeof(IModelInitializer<>))).AsImplementedInterfaces()
                    .AddClasses(c => c.AssignableTo(typeof(IPipelineBehavior<,>))).AsImplementedInterfaces());

            // Validators used as child validators need to be registered explicitly.
            // AddFluentValidation's RegisterValidatorsFromAssembly isn't sufficient to register them.
            // .NET Core DI doesn't support binding an open generic type: https://stackoverflow.com/a/44991270
            services.AddTransient<FluentValidation.AbstractValidator<AddCustomer>, AddCustomerValidator>();

            // The following should be contextually injected (eg Ninject's .WhenInjectedInto) into AddCustomerValidator 
            // (since its possible we might have multiple contexts for validating basic types like address + phone), 
            // but .NET Core's DI doesn't support that.
            // Could introduce another underlying container that supports this.
            services.AddTransient<FluentValidation.AbstractValidator<Application.ValueObjects.Address>, AddCustomerValidator.CustomerAddressValidator>();
            services.AddTransient<FluentValidation.AbstractValidator<Application.ValueObjects.PhoneNumber>, AddCustomerValidator.CustomerPhoneNumberValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Search}/{action=Search}");
            });
        }
    }
}
