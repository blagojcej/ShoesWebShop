﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.API.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductCatalog.API
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
            services.Configure<SecretSettings>(Configuration);
            services.Configure<CatalogSettings>(Configuration);
            string connectionString = Configuration["ConnectionStrings:Default"];
            services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "ShoesWebShop - Product Catalog HTTP API",
                    Version = "v1",
                    Description =
                        "The Product Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Product Catalog API"); });

            app.UseMvc();
        }
    }
}
