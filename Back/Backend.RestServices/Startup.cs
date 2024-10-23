using Backend.CrossCuting.Helpers;
using Backend.Infraestructure.UnitOfWork;
using Backend.RestServices.Code.AuthEntity;
using Backend.RestServices.Code.Log;
using Backend.RestServices.Code.ServiceHelpers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;

namespace Backend.RestServices
{
    public class Startup
    {
        private readonly AppConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _configuration = new AppConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {
                        Title = "Project Initial Services",
                        Version = "v1",
                        Description = "Lista de servicios disponibles" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                        <br />Enter 'Bearer' [space] and then your token in the text input below.
                        <br />Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.ClaimsIssuer = JwtBearerDefaults.AuthenticationScheme;
                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateActor = true,
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidAudience = _configuration._AppName,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration._TokenKey)), // Add the secret key to our Jwt encryption
                    };

                });


            services.AddMemoryCache();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            /*services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);*/


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Initial RestServices v1"));
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Initial RestServices v1");
                //    c.RoutePrefix = "docs";
                //});
                //}

            app.UseCors(x =>
            {
                x.WithOrigins("http://localhost:4200")
                .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.ConfigureCustomExceptionMiddleware();
            app.UseStaticFiles(); // For the wwwroot folder.
            /*app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "public_directory")),
                RequestPath = "/public_directory",
                //EnableDirectoryBrowsing = true // Deshabilita ver archivos
            });*/
            // Para Azure - Directorio compartido - FIN
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
