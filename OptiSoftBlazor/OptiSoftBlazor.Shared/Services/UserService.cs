using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OptiSoftBlazor.Shared.Data;

namespace OptiSoftBlazor.Shared.Services;

public class UsuarioService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UsuarioService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<OptiUsers?> ValidarCredenciales(string username, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OptiSoftDbContext>();

        var usuario = await context.OptiUsers
            .FirstOrDefaultAsync(u => u.UserName == username && u.Activo);

        if (usuario == null)
            return null;

        bool passwordValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);

        return passwordValido ? usuario : null;
    }

    public async Task<bool> CrearUsuario(string username, string password)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OptiSoftDbContext>();

        var existe = await context.OptiUsers
            .AnyAsync(u => u.UserName == username);

        if (existe)
            return false;

        var usuario = new OptiUsers
        {
            UserName = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Activo = true,
            CreatedAt = DateTime.Now
        };

        context.OptiUsers.Add(usuario);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CambiarPassword(int userId, string passwordAnterior, string passwordNueva)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OptiSoftDbContext>();

        var usuario = await context.OptiUsers.FindAsync(userId);
        if (usuario == null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(passwordAnterior, usuario.PasswordHash))
            return false;

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordNueva);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivarDesactivar(int userId, bool activo)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OptiSoftDbContext>();

        var usuario = await context.OptiUsers.FindAsync(userId);
        if (usuario == null)
            return false;

        usuario.Activo = activo;
        await context.SaveChangesAsync();
        return true;
    }
}