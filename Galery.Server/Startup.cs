using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Galery.Server.DAL;
using Galery.Server.DAL.Identity;
using Galery.Server.DAL.Models;
using Galery.Server.Interfaces;
using Galery.Server.JWT;
using Galery.Server.Mapping;
using Galery.Server.Middleware;
using Galery.Server.Service.Interfaces;
using Galery.Server.Service.Services;
using Galery.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Galery.Server
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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthTokenOptions.ISSUER,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthTokenOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true
                        };
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Galery API", Version = "v1" });

                var basePath = AppContext.BaseDirectory;
                //var xmlPath = Path.Combine(basePath, "Galery.Server.xml");
                //c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() }
                });
            });

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PictureProfile>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IRoleStore<Role>, RoleStore>();           
            services.AddSingleton<DbProviderFactory>(SqlClientFactory.Instance);
            services.AddIdentity<User, Role>().AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPictureService, PicturesService>();
            services.AddTransient<IFileWorkService, FileWorkService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ISubscribeService, SubscribeService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                c.DocExpansion(DocExpansion.None);
            });

            app.UseStaticFiles();

            app.UseMiddleware(typeof(ErrorHandlerMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultApi",
                    template: "api/{controller}/{id?}"
                    );

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
