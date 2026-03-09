using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Quibee.Database;

/// <summary>
/// Factory para crear instancias de QuibeeDbContext en tiempo de diseño
/// Esto permite que las migraciones de EF Core funcionen correctamente
/// </summary>
public class QuibeeDbContextFactory : IDesignTimeDbContextFactory<QuibeeDbContext>
{
    public QuibeeDbContext CreateDbContext(string[] args)
    {
        // Usar la carpeta del ejecutable para no depender del directorio de trabajo.
        var basePath = AppContext.BaseDirectory;

        // Cargar configuración desde appsettings
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = SqliteConnectionHelper.GetConnectionString(configuration);

        // Configurar opciones del DbContext
        var optionsBuilder = new DbContextOptionsBuilder<QuibeeDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new QuibeeDbContext(optionsBuilder.Options);
    }
}
