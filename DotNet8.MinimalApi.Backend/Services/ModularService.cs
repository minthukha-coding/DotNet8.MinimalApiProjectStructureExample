using System.Text;
using ClassLibrary1DotNet8.MinimalApi.Shared.Services;
using DotNet8.MinimalApiProjectStructureExample.Backend.Modules.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DotNet8.MinimalApiProjectStructureExampleBackend.Services;

public static class ModularService
{
    public static WebApplicationBuilder AddModularService(this WebApplicationBuilder builder)
    {
        return builder
            .AddJwtAuthorizationService()
            .AddDbContextService()
            .AddCommonService()
            .AddEmailService()
            .AddDataAcccessLayer()
            .AddBusinessLogicLayer();
    } 

    public static WebApplicationBuilder AddDbContextService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetSection("DbConnection").Value ??
                                   throw new AggregateException("connectionString is null");
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            opt.UseSqlServer(connectionString);
        },ServiceLifetime.Transient,ServiceLifetime.Transient);
        return builder;
    }
    
    private static WebApplicationBuilder AddBusinessLogicLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<BlogService>();
        return builder;
    } 
    
    private static WebApplicationBuilder AddCommonService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<JwtTokenService>();
        return builder;
    }

    private static WebApplicationBuilder AddDataAcccessLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AuthRepository>();
        builder.Services.AddScoped<BlogRepository>();
        return builder;
    }
  
    private static WebApplicationBuilder AddEmailService(this WebApplicationBuilder builder)
    {
        var fromEmail = builder.Configuration.GetSection("EmailSender").Value;
        var smtpMail = builder.Configuration.GetSection("EmailHost").Value;
        var appPwd = builder.Configuration.GetSection("EmailSenderAppPassword").Value;
        var port = builder.Configuration.GetSection("EmailPort").Value;

        builder.Services.AddFluentEmail(fromEmail)
                        .AddSmtpSender(smtpMail, Convert.ToInt32(port), fromEmail, appPwd);
        return builder;
    }
    
    private static WebApplicationBuilder AddJwtAuthorizationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "BLOGSITE", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new List<string> ()
                }
            });
        });

        var key = Encoding.ASCII.GetBytes("HQfsdfQ@C1"); // Use a secret key for encoding the token

        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddAuthorization();
        return builder;
    }
}
