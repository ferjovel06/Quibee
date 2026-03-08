using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Quibee.Database;

public static class SqliteConnectionHelper
{
    public static string GetConnectionString(IConfiguration? configuration)
    {
        var configured = configuration?.GetConnectionString("QuibeeDb")
            ?? configuration?.GetConnectionString("QuibeeDatabase");

        // Force offline-first SQLite if legacy MySQL config is present or empty.
        if (string.IsNullOrWhiteSpace(configured) ||
            configured.Contains("server=", StringComparison.OrdinalIgnoreCase) ||
            !configured.Contains("data source=", StringComparison.OrdinalIgnoreCase))
        {
            configured = "Data Source=quibee.db";
        }

        var dataSource = ExtractDataSource(configured);
        if (string.IsNullOrWhiteSpace(dataSource))
        {
            dataSource = "quibee.db";
        }

        if (!Path.IsPathRooted(dataSource))
        {
            var appDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Quibee");
            Directory.CreateDirectory(appDataDir);
            dataSource = Path.Combine(appDataDir, dataSource);
        }

        return $"Data Source={dataSource}";
    }

    private static string? ExtractDataSource(string connectionString)
    {
        var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var token = part.Trim();
            if (token.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
            {
                return token.Substring("Data Source=".Length).Trim().Trim('"');
            }

            if (token.StartsWith("DataSource=", StringComparison.OrdinalIgnoreCase))
            {
                return token.Substring("DataSource=".Length).Trim().Trim('"');
            }
        }

        return null;
    }
}
