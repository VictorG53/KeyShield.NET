using KeyShieldAPI.Middlewares;
using KeyShieldAPI.Repositories;
using KeyShieldAPI.Services;
using KeyShieldAPI.Services.CoffreDeblocageService;
using KeyShieldDB.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o => { });

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddScoped<KeyShieldDbContext>();

builder.Services.AddSingleton<ICoffreDeblocageMemoryStore, CoffreDeblocageMemoryStore>();

builder.Services.AddScoped<GetOrCreateUtilisateurMiddleware>();
builder.Services.AddScoped<ErrorHandlerMiddleware>();

// Enregistrement des services
// Utilisateur
builder.Services.AddScoped<UtilisateurService>();
builder.Services.AddScoped<UtilisateurRepository>();

// Log
builder.Services.AddScoped<LogService>(sp =>
    new LogService(
            sp.GetRequiredService<LogRepository>(),
            sp.GetRequiredService<KeyShieldDbContext>()
        )
);

builder.Services.AddScoped<LogRepository>();

// Coffre
builder.Services.AddScoped<CoffreService>(sp =>
    new CoffreService(
        sp.GetRequiredService<CoffreRepository>(),
        sp.GetRequiredService<UtilisateurService>(),
        sp.GetRequiredService<LogService>(),
        sp.GetRequiredService<ICoffreDeblocageMemoryStore>()
    )
);
builder.Services.AddScoped<CoffreRepository>();

// Entree
builder.Services.AddScoped<EntreeService>(sp =>
    new EntreeService(
        sp.GetRequiredService<EntreeRepository>(),
        sp.GetRequiredService<UtilisateurService>(),
        sp.GetRequiredService<ICoffreDeblocageMemoryStore>(),
        sp.GetRequiredService<LogService>()
    )
);
builder.Services.AddScoped<EntreeRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.UseMiddleware<GetOrCreateUtilisateurMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    KeyShieldDbContext dbContext = scope.ServiceProvider.GetRequiredService<KeyShieldDbContext>();
    dbContext.Database.EnsureCreated();
    // Initialisation des types d'action s'ils n'existent pas déjà
    if (!dbContext.ActionTypes.Any())
    {
        dbContext.ActionTypes.AddRange(
            new List<KeyShieldDB.Models.ActionType>
            {
            new() { Identifiant = Guid.Parse("e0e77909-100a-41da-ac49-172072e05a60"), Libelle = "CREATE" },
            new() { Identifiant = Guid.Parse("01d5b1f8-ac03-46c5-9ace-0d6f61bc8480"), Libelle = "READ" },
            new() { Identifiant = Guid.Parse("b99402f2-1bed-45e7-8618-28d62d6393a3"), Libelle = "UPDATE" },
            new() { Identifiant = Guid.Parse("302627d6-d440-40c9-9df9-578cb3d69c7a"), Libelle = "DELETE" }
            }
        );
        dbContext.SaveChanges();
    }
    else
    {

    }
}

app.Run();