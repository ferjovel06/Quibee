using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Quibee.Models;

namespace Quibee.Views.Controls;

public partial class TextContentControl : UserControl
{
    private static readonly Regex EmojiTokenRegex = new(@"(:[a-z0-9_]+:)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Dictionary<string, string> EmojiImageMap = new(StringComparer.OrdinalIgnoreCase)
    {
        [":cookie:"] = "avares://Quibee/Assets/Images/Cookie.png",
        [":galleta:"] = "avares://Quibee/Assets/Images/Cookie.png",
        [":star:"] = "avares://Quibee/Assets/Images/Star.png",
        [":small_star:"] = "avares://Quibee/Assets/Images/SmallStar.png"
    };

    public static readonly StyledProperty<LessonContentData?> ContentDataProperty =
        AvaloniaProperty.Register<TextContentControl, LessonContentData?>(nameof(ContentData));

    public LessonContentData? ContentData
    {
        get => GetValue(ContentDataProperty);
        set => SetValue(ContentDataProperty, value);
    }

    public TextContentControl()
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

        var textBlock = this.FindControl<TextBlock>("ContentText");
        if (textBlock == null) return;

        var contentText = ContentData.Text ?? "";
        if (ContentData.IsBulletPoint == true)
        {
            contentText = $"• {contentText}";
        }

        // Aplicar tamaño de fuente
        if (ContentData.FontSize.HasValue)
        {
            textBlock.FontSize = ContentData.FontSize.Value;
        }

        // Aplicar color
        if (!string.IsNullOrEmpty(ContentData.Color))
        {
            try
            {
                textBlock.Foreground = new SolidColorBrush(Color.Parse(ContentData.Color));
            }
            catch
            {
                // Si el color no es válido, mantener el blanco por defecto
            }
        }

        // Aplicar alineación
        if (!string.IsNullOrEmpty(ContentData.Alignment))
        {
            textBlock.TextAlignment = ContentData.Alignment.ToLower() switch
            {
                "left" => TextAlignment.Left,
                "center" => TextAlignment.Center,
                "right" => TextAlignment.Right,
                _ => TextAlignment.Left
            };
        }

        RenderTextWithEmoji(textBlock, contentText);
    }

    private static void RenderTextWithEmoji(TextBlock textBlock, string rawText)
    {
        var normalized = rawText.Replace("🍪", ":cookie:");
        var parts = EmojiTokenRegex.Split(normalized);

        textBlock.Inlines?.Clear();

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part))
            {
                continue;
            }

            if (EmojiImageMap.TryGetValue(part, out var imagePath))
            {
                var image = CreateEmojiImage(imagePath, textBlock.FontSize + 4);
                if (image != null)
                {
                    textBlock.Inlines?.Add(new InlineUIContainer { Child = image });
                    continue;
                }
            }

            textBlock.Inlines?.Add(new Run(part));
        }
    }

    private static Image? CreateEmojiImage(string imagePath, double size)
    {
        try
        {
            var bitmap = new Bitmap(AssetLoader.Open(new Uri(imagePath)));
            return new Image
            {
                Source = bitmap,
                Width = size,
                Height = size,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(1, 0)
            };
        }
        catch
        {
            return null;
        }
    }
}
