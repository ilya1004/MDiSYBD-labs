using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MusicPlayerDB.API.Extensions;
using MusicPlayerDB.Application.Extensions;
using MusicPlayerDB.Application.Interfaces;
using MusicPlayerDB.Infrastructure;
using MusicPlayerDB.Persistence;
using MusicPlayerDB.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddServices();
services.AddRepositories();

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

services.AddApiAuthentication(builder.Configuration);
services.AddApiAuthorization();

services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicPlayer", Version = "v1"}));

services.AddDbContext<MusicPlayerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(MusicPlayerDbContext))));

const string MyCorsPolicy = "myCorsPolicy";

services.AddCors(options =>
{
    options.AddPolicy(
        MyCorsPolicy,
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:3000")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials();
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllerRoute("Default", "{controller=Value}/{Action=Index}");

app.UseRouting();

app.UseCors(MyCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        HttpOnly = HttpOnlyPolicy.Always,
        MinimumSameSitePolicy = SameSiteMode.Lax,
        Secure = CookieSecurePolicy.Always,
    }
);

app.Run();
