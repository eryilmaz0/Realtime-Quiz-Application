using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Quiz.App.DbContext;
using Quiz.App.Entities;
using Newtonsoft.Json;
using Quiz.App.JWT;
using TokenOptions = Quiz.App.JWT.TokenOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quiz.App.Hubs;

namespace Quiz.App
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
            //IDENTITY CONFIGURATION
            services.AddIdentity<User, Role>(identityOptions =>
            {
                //PASSWORD CONFIGURATIONS
                identityOptions.Password.RequireDigit = false; //DIGIT
                identityOptions.Password.RequireLowercase = false; //LOWER CASE
                identityOptions.Password.RequireNonAlphanumeric = false; //NON ALPHA NUMERIC
                identityOptions.Password.RequireUppercase = false; //UPPER CASE
                identityOptions.Password.RequiredLength = 6; //MÝN LENGTH

                //USER CONFIGURATIONS
                identityOptions.User.AllowedUserNameCharacters =
                    "abcçdefgðhýijklmnoöpqrstuüvwxyzABCÇDEFGÐHÝIJKLMNOÖPQRSTÜUVWXYZ0123456789 -.@_";
                identityOptions.User.RequireUniqueEmail = true; //EÞSÝZ MAÝL

                identityOptions.SignIn.RequireConfirmedEmail = false;

                identityOptions.ClaimsIdentity.RoleClaimType = "user_role";
                identityOptions.ClaimsIdentity.UserIdClaimType = "userId";
                identityOptions.ClaimsIdentity.UserNameClaimType = "email";


            }).AddEntityFrameworkStores<QuizDbContext>().AddDefaultTokenProviders();
               


            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            //JWT CONFIGURATION
            services.AddAuthentication(o =>
                {
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };

                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/connectionHub")))
                            {

                                context.Token = accessToken;
                            }

                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/matchHub")))
                            {

                                context.Token = accessToken;
                            }
                            
                            return Task.CompletedTask;
                        }
                    };

                });


            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    //AUTO VALIDATION DISABLED
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(o =>
                {
                    //INCLUDE
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz.App", Version = "v1" });
            });


            //.NET CORE BUILT IN IOC CONTAINER 
            services.AddSingleton<ITokenHelper,JwtHelper>();
            services.AddScoped<QuizDbContext>();


            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed((x) => true)
                            .AllowCredentials();
                    });
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("_myAllowSpecificOrigins");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz.App v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ConnectionHub>("/connectionHub");
                endpoints.MapHub<MatchHub>("/matchHub");
            });


        }
    }
}
