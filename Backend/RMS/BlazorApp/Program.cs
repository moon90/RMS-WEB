using Blazored.LocalStorage;
using BlazorApp.Components;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddHttpClient("RMSApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7083/");
});
builder.Services.AddDataProtection();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped(sp => new Lazy<ProtectedLocalStorage>(() => sp.GetRequiredService<ProtectedLocalStorage>()));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CustomAuth";
    options.DefaultChallengeScheme = "CustomAuth";
}).AddCookie("CustomAuth", options =>
{
    options.Cookie.Name = "CustomAuthCookie";
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/AccessDenied";
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<UserApiClientService>();
builder.Services.AddScoped<RoleApiClientService>();
builder.Services.AddScoped<AuditLogApiClientService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
