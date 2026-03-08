using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.Extensions.Configuration;
using Velopack;
using Velopack.Sources;

namespace Quibee.Services;

public sealed class AppUpdateService
{
    private readonly IConfigurationRoot _configuration;

    public AppUpdateService()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
    }

    public async Task CheckForUpdatesAsync(Window? owner)
    {
        var section = _configuration.GetSection("AutoUpdate");
        var enabled = bool.TryParse(section["Enabled"], out var enabledValue) && enabledValue;
        var checkOnStartup = !bool.TryParse(section["CheckOnStartup"], out var checkValue) || checkValue;
        var githubRepoUrl = section["GithubRepoUrl"];

        if (!enabled || !checkOnStartup || string.IsNullOrWhiteSpace(githubRepoUrl))
        {
            return;
        }

        try
        {
            var manager = new UpdateManager(new GithubSource(githubRepoUrl, null, false, null));
            var update = await manager.CheckForUpdatesAsync();

            if (update == null || owner == null)
            {
                return;
            }

            var shouldUpdate = await AskForConfirmationAsync(owner);
            if (!shouldUpdate)
            {
                return;
            }

            await manager.DownloadUpdatesAsync(update);
            manager.ApplyUpdatesAndRestart(update);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("not installed", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Console.WriteLine($"⚠️ Error al comprobar actualizaciones: {ex.Message}");
        }
    }

    private static Task<bool> AskForConfirmationAsync(Window owner)
    {
        var task = new TaskCompletionSource<bool>();

        var message = new TextBlock
        {
            Text = "Hay una actualización disponible. ¿Deseas descargarla e instalarla ahora?",
            TextWrapping = TextWrapping.Wrap,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 0, 0, 16)
        };

        var updateButton = new Button
        {
            Content = "Actualizar",
            MinWidth = 110,
            HorizontalAlignment = HorizontalAlignment.Right
        };

        var laterButton = new Button
        {
            Content = "Más tarde",
            MinWidth = 110,
            HorizontalAlignment = HorizontalAlignment.Right
        };

        var buttons = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10,
            Children = { laterButton, updateButton }
        };

        var content = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 8,
            Children = { message, buttons }
        };

        var dialog = new Window
        {
            Title = "Actualización disponible",
            Width = 500,
            Height = 180,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            Content = content,
            Background = new SolidColorBrush(Color.Parse("#311B42"))
        };

        updateButton.Click += (_, _) =>
        {
            task.TrySetResult(true);
            dialog.Close();
        };

        laterButton.Click += (_, _) =>
        {
            task.TrySetResult(false);
            dialog.Close();
        };

        dialog.Closed += (_, _) => task.TrySetResult(false);

        _ = dialog.ShowDialog(owner);
        return task.Task;
    }
}