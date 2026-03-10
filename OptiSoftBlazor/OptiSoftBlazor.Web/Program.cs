using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Data.Tenant;
using OptiSoftBlazor.Shared.Services;
using OptiSoftBlazor.Web.Components;
using OptiSoftBlazor.Web.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<OptiSoftDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<OptiSoftDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<TenantDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<OptiSoftDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IAuthService, MultiTenantAuthService>();
builder.Services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<DetCompraService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<PersonalService>();
builder.Services.AddScoped<SeteoService>();
builder.Services.AddScoped<ConfirmService>();
builder.Services.AddScoped<UsuariosService>();

builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddLocalization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(OptiSoftBlazor.Shared._Imports).Assembly);

app.Run();
