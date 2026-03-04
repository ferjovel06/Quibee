using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Quibee.Models;

namespace Quibee.Views.Controls;

public partial class HeadingContentControl : UserControl
{
    public static readonly StyledProperty<LessonContentData?> ContentDataProperty =
        AvaloniaProperty.Register<HeadingContentControl, LessonContentData?>(nameof(ContentData));

    public LessonContentData? ContentData
    {
        get => GetValue(ContentDataProperty);
        set => SetValue(ContentDataProperty, value);
    }

    public HeadingContentControl()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentDataProperty && ContentData != null)
        {
            UpdateContent();
        }
    }

    private void UpdateContent()
    {
        if (ContentData == null) return;

        var textBlock = this.FindControl<TextBlock>("HeadingText");
        var border = this.FindControl<Border>("HeadingBorder");
        
        if (textBlock == null || border == null) return;

        // Establecer texto
        textBlock.Text = ContentData.Text ?? "";

        // Aplicar nivel (h1, h2, h3)
        if (ContentData.Level.HasValue)
        {
            textBlock.FontSize = ContentData.Level.Value switch
            {
                1 => 32, // h1
                2 => 28, // h2
                3 => 24, // h3
                _ => 24
            };
        }

        // Aplicar color del texto
        if (!string.IsNullOrEmpty(ContentData.Color))
        {
            try
            {
                textBlock.Foreground = new SolidColorBrush(Color.Parse(ContentData.Color));
            }
            catch
            {
                // Mantener color por defecto
            }
        }

        // Para headings de nivel 3, usar borde más sutil
        if (ContentData.Level == 3)
        {
            var isExampleOne = (ContentData.Text ?? string.Empty)
                .StartsWith("Ejemplo 1", StringComparison.OrdinalIgnoreCase);

            border.Background = Brushes.Transparent;
            border.BorderBrush = new SolidColorBrush(Color.Parse("#7CDAF3"));
            border.BorderThickness = isExampleOne ? new Thickness(0) : new Thickness(0, 2, 0, 0);
            border.CornerRadius = new CornerRadius(0);
            border.Padding = isExampleOne ? new Thickness(0) : new Thickness(0, 10, 0, 0);
            border.Margin = isExampleOne ? new Thickness(0, 8, 0, 8) : new Thickness(0, 24, 0, 8);

            textBlock.FontSize = 22;
            if (string.IsNullOrEmpty(ContentData.Color))
            {
                textBlock.Foreground = new SolidColorBrush(Color.Parse("#7CDAF3"));
            }
        }
    }
}
