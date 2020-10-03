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
using MiniCrm.Persistence.Customer.CommandHandlers;
using MiniCrm.Persistence.Customer.MapperProfiles;
using MiniCrm.Web.Filters;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews(o =>
            {
                o.Filters.Add<ModelInitializerFilter>(0);
                o.Filters.Add<ModelStateValidationFilter>(1);
                })
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssembly(typeof(Startup).Assembly); // MiniCrm.Web
                    c.RegisterValidatorsFromAssembly(typeof(AddCustomerValidator).Assembly); // MiniCrm.Application
                });

            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddMediatR(
                typeof(PersistCustomer).Assembly, // MiniCrm.Persistence
                typeof(Application.Customer.Commands.AddCustomer).Assembly); // MiniCrm.Application
            services.AddAutoMapper(typeof(AddCustomerProfile).Assembly); // MiniCrm.Application

            services.AddDbContext<CrmContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Crm")));

            // since its possible we might have multiple contexts for validating basic types like address + phone, 
            // ideally these would be bound using something like Ninject's .WhenInjectedInto, but .NET Core's DI doesn't support contextual binding.
            // could introduce Ninject as the underlying container
            services.AddTransient<FluentValidation.AbstractValidator<Application.ValueObjects.Address>, AddCustomerValidator.CustomerAddressValidator>();
            services.AddTransient<FluentValidation.AbstractValidator<Application.ValueObjects.PhoneNumber>, AddCustomerValidator.CustomerPhoneNumberValidator>();

            // it seems that validators used as child validators need to be registered explicitly - AddFluentValidation's RegisterValidatorsFromAssembly isn't sufficient
            services.AddTransient<FluentValidation.AbstractValidator<AddCustomer>, AddCustomerValidator>();

            // this feels like a MediatR bug, but IRequestHandler<AddCustomer> is not automatically registered in DI.
            // however, IRequestHandler<AddCustomer, Unit> (which represents a void return type) is.
            services.AddTransient<IRequestHandler<AddCustomer>, PersistCustomer>();

            services.AddTransient<IModelInitializer<AddCustomerModel>, AddCustomerModelInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Customer}/{action=Search}");
            });
        }
    }
}
