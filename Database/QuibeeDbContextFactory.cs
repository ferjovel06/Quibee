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
        // Cargar configuración desde appsettings
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("QuibeeDb");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró el connection string 'QuibeeDb' en appsettings.json");
        }

        // Configurar opciones del DbContext
        var optionsBuilder = new DbContextOptionsBuilder<QuibeeDbContext>();
        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure()
        );

        return new QuibeeDbContext(optionsBuilder.Options);
    }
}
