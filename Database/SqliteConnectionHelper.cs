using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Quibee.Database;

public static class SqliteConnectionHelper
{
    private const string DefaultDbFileName = "quibee.db";
    private const string StableDataFolderName = "QuibeeData";
    private const string LegacyDataFolderName = "Quibee";

    public static string GetConnectionString(IConfiguration? configuration)
    {
        var configured = configuration?.GetConnectionString("QuibeeDb")
            ?? configuration?.GetConnectionString("QuibeeDatabase");

        // Force offline-first SQLite if legacy MySQL config is present or empty.
        if (string.IsNullOrWhiteSpace(configured) ||
            configured.Contains("server=", StringComparison.OrdinalIgnoreCase) ||
            !configured.Contains("data source=", StringComparison.OrdinalIgnoreCase))
        {
            configured = $"Data Source={DefaultDbFileName}";
        }

        var dataSource = ExtractDataSource(configured);
        if (string.IsNullOrWhiteSpace(dataSource))
        {
            dataSource = DefaultDbFileName;
        }

        if (!Path.IsPathRooted(dataSource))
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var stableDataDir = Path.Combine(localAppData, StableDataFolderName);
            Directory.CreateDirectory(stableDataDir);

            var fileName = Path.GetFileName(dataSource);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = DefaultDbFileName;
            }

            var stableDbPath = Path.Combine(stableDataDir, fileName);
            TryRecoverLegacyDatabase(localAppData, stableDbPath, fileName);
            dataSource = stableDbPath;
        }
        else
        {
            var dbDirectory = Path.GetDirectoryName(dataSource);
            if (!string.IsNullOrWhiteSpace(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }
        }

        return $"Data Source={dataSource}";
    }

    private static void TryRecoverLegacyDatabase(string localAppData, string stableDbPath, string dbFileName)
    {
        if (File.Exists(stableDbPath))
        {
            return;
        }

        try
        {
            var legacyBaseDir = Path.Combine(localAppData, LegacyDataFolderName);
            var candidatePaths = new List<string>
            {
                Path.Combine(legacyBaseDir, dbFileName),
                Path.Combine(legacyBaseDir, "current", dbFileName),
                Path.Combine(legacyBaseDir, "app", dbFileName)
            };

            var packagesDir = Path.Combine(legacyBaseDir, "packages");
            if (Directory.Exists(packagesDir))
            {
                candidatePaths.AddRange(Directory.EnumerateFiles(packagesDir, dbFileName, SearchOption.AllDirectories));
            }

            var sourceDb = candidatePaths
                .Where(File.Exists)
                .Select(path => new FileInfo(path))
                .Where(info => info.Length > 0)
                .OrderByDescending(info => info.LastWriteTimeUtc)
                .FirstOrDefault();

            if (sourceDb == null)
            {
                return;
            }

            File.Copy(sourceDb.FullName, stableDbPath, overwrite: false);

            var sourceWal = sourceDb.FullName + "-wal";
            var sourceShm = sourceDb.FullName + "-shm";
            if (File.Exists(sourceWal))
            {
                File.Copy(sourceWal, stableDbPath + "-wal", overwrite: false);
            }

            if (File.Exists(sourceShm))
            {
                File.Copy(sourceShm, stableDbPath + "-shm", overwrite: false);
            }
        }
        catch
        {
            // No bloquear inicio si no se puede recuperar automáticamente la base legacy.
        }
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
