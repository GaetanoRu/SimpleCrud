using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SimpleCrud.BusinessLayer.MapperProfiles;
using SimpleCrud.BusinessLayer.Services;
using SimpleCrud.BusinessLayer.Validations;
using SimpleCrud.DataAccessLayer;
using SimpleCrud.SwaggerDocumentation;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleCrud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                })
                .AddFluentValidation(fluent => fluent.RegisterValidatorsFromAssemblyContaining<CustomerValidator>());

            services.AddAutoMapper(typeof(CustomerProfile).Assembly);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IDataContext, DataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<DefaultResponseOperationFilter>();
                options.ExampleFilters();


                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SimpleCrud",
                    Version = "v1",
                    Description = "Simple Crud",

                    Contact = new OpenApiContact()
                    {
                        Name = "Gaetano Russo",
                        Email = "gaetano-russo90@hotmail.it"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

            }).AddFluentValidationRulesToSwagger(options =>
            {
                options.SetNotNullableIfMinLengthGreaterThenZero = true;
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleCrud v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
