using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Validators;
using DataAccess.Entities;
using DataAccess;

namespace WebAPI
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentityCore<Player>(setupAction =>
                {
                    setupAction.User.AllowedUserNameCharacters = "";
                    setupAction.User.RequireUniqueEmail = true;
                    setupAction.Password.RequireNonAlphanumeric = false;
                    setupAction.Password.RequireUppercase = false;
                    setupAction.Password.RequireLowercase = false;
                    setupAction.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IGameService, GameService>()
                .AddScoped<IPlayerService, PlayerService>()
                .AddTransient<IJwtService, JwtService>();
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication(configureOptions =>
                {
                    configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configureOptions =>
                {
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

                    configureOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    configureOptions.MapInboundClaims = false;
                });

            return services;
        }

        public static IServiceCollection AddAPIControllers(this IServiceCollection services)
        {
            services
                .AddControllers(configure =>
                {
                    configure.Filters.Add(typeof(ExceptionFilter));
                })
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var validationProblemDetails = new ValidationProblemDetails(actionContext.ModelState);

                        return new UnprocessableEntityObjectResult(validationProblemDetails);
                    };
                });

            return services;
        }

        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<SignUpPlayerRequestValidator>();

            return services.AddFluentValidationAutoValidation();
        }
    }
}