using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Data.Tenant;
using OptiSoftBlazor.Shared.Services;
using OptiSoftBlazor.Web.Components;
using OptiSoftBlazor.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// DB Central para tabla Tenants
builder.Services.AddDbContext<TenantDbContext>(options =>
    options.UseSqlServer(connectionString));

// Servicio de Tenant - guarda el tenant actual
builder.Services.AddScoped<ITenantService, TenantService>();

// DbContextFactory - para los servicios que ya lo usan
builder.Services.AddDbContextFactory<OptiSoftDbContext>(options =>
    options.UseSqlServer(connectionString));

// DbContext que cambia según el tenant
builder.Services.AddScoped<OptiSoftDbContext>(sp =>
{
    var tenantService = sp.GetRequiredService<ITenantService>();
    var conn = tenantService.CurrentConnectionString ?? connectionString;

    var options = new DbContextOptionsBuilder<OptiSoftDbContext>()
        .UseSqlServer(conn)
        .Options;

    return new OptiSoftDbContext(options);
});

// IDENTITY
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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// Auth service multi-tenant
builder.Services.AddScoped<IAuthService, MultiTenantAuthService>();

// Tus servicios (NO TOCAR)
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<DetCompraService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<PersonalService>();
builder.Services.AddSingleton<IFormFactor, FormFactor>();

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