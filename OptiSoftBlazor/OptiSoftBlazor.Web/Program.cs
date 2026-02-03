using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using OptiSoftBlazor.Shared.Data;
using OptiSoftBlazor.Shared.Services;
using OptiSoftBlazor.Web.Components;
using OptiSoftBlazor.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<OptiSoftDbContext>(options =>
    options.UseSqlServer(connectionString));

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

// REGISTRAR EL SERVICIO
builder.Services.AddScoped<IAuthService, IdentityAuthService>();

// Tus servicios existentes
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<DetCompraService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<PersonalService>();
builder.Services.AddScoped<SeteoService>();
builder.Services.AddScoped<IAuthService, IdentityAuthService>();
builder.Services.AddSingleton<IFormFactor, FormFactor>();

var app = builder.Build();

// Crear usuario admin
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<OptiSoftDbContext>();

    await dbContext.Database.EnsureCreatedAsync();

    if (await userManager.FindByNameAsync("admin") == null)
    {
        var user = new IdentityUser { UserName = "admin", Email = "admin@optisoft.com", EmailConfirmed = true };
        await userManager.CreateAsync(user, "admin");
        Console.WriteLine("✅ Usuario admin creado: admin / admin");
    }
}

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