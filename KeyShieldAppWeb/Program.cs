using KeyShieldAppWeb.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string apiEndpoint = builder.Configuration.GetValue<string>("WebAPI:Endpoint") ?? throw new InvalidOperationException("WebAPI endpoint is not configured");
string apiScopes = builder.Configuration.GetValue<string>("WebAPI:Scope") ?? throw new InvalidOperationException("WebAPI scope is not configured");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi([apiScopes])
    .AddDownstreamApi("KeyShieldAPI", options =>
    {
        options.BaseUrl = apiEndpoint;
        options.Scopes = [apiScopes];
    })
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();
