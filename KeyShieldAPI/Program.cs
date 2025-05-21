using KeyShieldDB.Context;
using KeyShieldAPI.Services;
using Microsoft.Identity.Web;
using KeyShieldAPI.Middlewares;
using KeyShieldAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o => { });

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddScoped<KeyShieldDbContext>();

builder.Services.AddScoped<GetOrCreateUtilisateurMiddleware>();
builder.Services.AddScoped<ErrorHandlerMiddleware>();

// Enregistrement des services
// Utilisateur
builder.Services.AddScoped<UtilisateurService>();
builder.Services.AddScoped<UtilisateurRepository>();
// Coffre
builder.Services.AddScoped<CoffreService>(sp =>
    new CoffreService(
        sp.GetRequiredService<CoffreRepository>(),
        sp.GetRequiredService<UtilisateurService>()
    )
);
builder.Services.AddScoped<CoffreRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GetOrCreateUtilisateurMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<KeyShieldDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();