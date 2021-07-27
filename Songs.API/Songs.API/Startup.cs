using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Songs.Interfaces;
using Songs.DataPersistence.Repositories;
using Songs.Services;
using System;
using Songs.Common;
using Songs.API.Middleware.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using Songs.API.Middleware.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace Songs.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthorizationSettings>(Configuration.GetSection("AuthorizationSettings"));

            ConfigureAuthServices(services, Configuration);

            services.AddControllers();

            //v1
            services.AddSingleton<ISongsRepository, SongsRepository>();
            //services.AddTransient<ISongsService, SongsService>();

            //v2
            services.AddSingleton<ISongsRepositoryAsync, SongsRepositoryAsync>();
            services.AddTransient<ISongsServiceAsync, SongsServiceAsync>();

            //auth
            services.AddSingleton<IUsersService, UsersService>();

            //swagger
            ConfigureSwagger(services);


        }

        private static void ConfigureAuthServices(IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["AuthorizationSettings:Issuer"],
                        ValidAudience = configuration["AuthorizationSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthorizationSettings:Secret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.User, policy =>
                    policy.Requirements.Add(new PrivilegeRequirement(Policies.User))
                );
                config.AddPolicy(Policies.Admin, policy =>
                    policy.Requirements.Add(new PrivilegeRequirement(Policies.Admin))
                );
                config.AddPolicy(Policies.All, policy =>
                    policy.Requirements.Add(new PrivilegeRequirement(Policies.All))
                );
            });

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
           
            services.AddSwaggerGen(c =>
            {
                /*c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Songs API",
                    Description = "ASP.NET Core Web API"
                });*/

                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Songs API",
                    Description = "ASP.NET Core Web API (async)"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private static void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Songs V2");

                c.RoutePrefix = string.Empty;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(); //added from middleware

            app.UseHttpsRedirection();

            app.UseRouting();

            //added for auth part
            app.UseAuthentication();
            app.UseAuthorization();

            //swagger
            UseSwagger(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
