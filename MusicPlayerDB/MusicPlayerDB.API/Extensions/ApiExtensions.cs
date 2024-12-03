using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicPlayerDB.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace MusicPlayerDB.API.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions!.SecretKey)
                        )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["my-cookie"];
                            return Task.CompletedTask;
                        }
                    };
                }
            );
        return services;
    }

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                AuthorizationPolicies.UserPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.UserRole)
            )
            .AddPolicy(
                AuthorizationPolicies.ArtistPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.ArtistRole)
            )
            .AddPolicy(
                AuthorizationPolicies.AdminPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.AdminRole)
            )
            .AddPolicy(
                AuthorizationPolicies.UserAndArtistPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.UserRole, ApplicationRoles.ArtistRole)
            )
            .AddPolicy(
                AuthorizationPolicies.ArtistAndAdminPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.ArtistRole, ApplicationRoles.AdminRole)
            )
            .AddPolicy(
                AuthorizationPolicies.UserAndAdminPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.UserRole, ApplicationRoles.AdminRole)
            )
            .AddPolicy(
                AuthorizationPolicies.UserAndArtistAndAdminPolicy,
                policy => policy.RequireClaim(ClaimTypes.Role, ApplicationRoles.UserRole, ApplicationRoles.ArtistRole, ApplicationRoles.AdminRole)
            );

        return services;
    }
}

