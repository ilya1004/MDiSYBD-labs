using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MusicPlayerDB.Persistence;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();

services.AddRepositories();

services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MusicPlayer", Version = "v1"}));

services.AddDbContext<MusicPlayerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(MusicPlayerDbContext))));

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

app.Run();
