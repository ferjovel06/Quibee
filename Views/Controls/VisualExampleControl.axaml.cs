using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Quibee.Models;
using Quibee.Services;

namespace Quibee.Views.Controls;

public partial class VisualExampleControl : UserControl
{
    private readonly List<TextBox> _answerInputs = new();

    public static readonly StyledProperty<LessonContentData?> ContentDataProperty =
        AvaloniaProperty.Register<VisualExampleControl, LessonContentData?>(nameof(ContentData));

    public LessonContentData? ContentData
    {
        get => GetValue(ContentDataProperty);
        set => SetValue(ContentDataProperty, value);
    }

    public VisualExampleControl()
    {
        InitializeComponent();

        var checkButton = this.FindControl<Button>("CheckButton");
        if (checkButton != null)
        {
            checkButton.Click += OnCheckButtonClick;
        }
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
        if (ContentData?.Objects == null) return;

        var container = this.FindControl<StackPanel>("ObjectsContainer");
        var validationContainer = this.FindControl<StackPanel>("ValidationContainer");
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (container == null) return;

        // Limpiar contenedor
        container.Children.Clear();
        _answerInputs.Clear();
        if (validationContainer != null)
        {
            validationContainer.IsVisible = false;
        }
        if (validationMessage != null)
        {
            validationMessage.Text = string.Empty;
        }

        // Configurar orientación
        if (ContentData.Layout == "vertical")
        {
            container.Orientation = Orientation.Vertical;
        }

        // Configurar spacing
        if (ContentData.Spacing.HasValue)
        {
            container.Spacing = ContentData.Spacing.Value;
        }

        // Renderizar cada objeto
        foreach (var obj in ContentData.Objects)
        {
            var element = CreateVisualElement(obj);
            if (element != null)
            {
                container.Children.Add(element);
            }
        }

        if (_answerInputs.Count > 0 && ContentData.CorrectAnswer.HasValue && validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }
    }

    private Control? CreateVisualElement(VisualObject obj)
    {
        // Si es una imagen repetida
        if (!string.IsNullOrEmpty(obj.ImageUrl) && obj.Count.HasValue && obj.Count > 1)
        {
            // Patrones visuales específicos para aproximar el diseño de ejercicios.
            if (obj.Count.Value == 4)
            {
                return CreateTwoRowImagePattern(obj.ImageUrl, obj.Width, obj.Height, 2, 2);
            }

            if (obj.Count.Value == 5)
            {
                return CreateTwoRowImagePattern(obj.ImageUrl, obj.Width, obj.Height, 2, 3);
            }

            if (obj.Count.Value == 6)
            {
                return CreateTwoRowImagePattern(obj.ImageUrl, obj.Width, obj.Height, 3, 3);
            }

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8
            };

            for (int i = 0; i < obj.Count.Value; i++)
            {
                var image = CreateImage(obj.ImageUrl, obj.Width, obj.Height);
                if (image != null)
                {
                    stackPanel.Children.Add(image);
                }
            }

            return stackPanel;
        }

        // Si es una imagen única
        if (!string.IsNullOrEmpty(obj.ImageUrl))
        {
            return CreateImage(obj.ImageUrl, obj.Width, obj.Height);
        }

        // Si es un emoji repetido
        if (!string.IsNullOrEmpty(obj.Emoji) && obj.Count.HasValue && obj.Count > 1)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5
            };

            for (int i = 0; i < obj.Count.Value; i++)
            {
                stackPanel.Children.Add(CreateEmojiTextBlock(obj.Emoji, obj.FontSize));
            }

            return stackPanel;
        }

        // Si es un emoji único
        if (!string.IsNullOrEmpty(obj.Emoji))
        {
            return CreateEmojiTextBlock(obj.Emoji, obj.FontSize);
        }

        // Si es un número
        if (!string.IsNullOrEmpty(obj.Number))
        {
            return new TextBlock
            {
                Text = obj.Number,
                FontSize = obj.FontSize ?? 48,
                FontWeight = FontWeight.Bold,
                Foreground = !string.IsNullOrEmpty(obj.Color) 
                    ? new SolidColorBrush(Color.Parse(obj.Color))
                    : Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        // Si es un símbolo (+, -, =, →)
        if (!string.IsNullOrEmpty(obj.Symbol))
        {
            return new TextBlock
            {
                Text = obj.Symbol,
                FontSize = obj.FontSize ?? 36,
                FontWeight = FontWeight.Bold,
                Foreground = !string.IsNullOrEmpty(obj.Color)
                    ? new SolidColorBrush(Color.Parse(obj.Color))
                    : Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0)
            };
        }

        // Si es una caja de respuesta
        if (obj.AnswerBox == true)
        {
            var border = new Border
            {
                Width = obj.Width ?? 80,
                Height = obj.Height ?? 80,
                Background = new SolidColorBrush(Color.Parse("#7A678A")),
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(10, 0)
            };

            var answerInput = new TextBox
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Foreground = new SolidColorBrush(Color.Parse("#FFFFFF")),
                FontSize = 34,
                FontWeight = FontWeight.Bold,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins"),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                MaxLength = 3
            };

            // Permitir solo números.
            answerInput.AddHandler(TextInputEvent, (_, e) =>
            {
                if (e is TextInputEventArgs args &&
                    !string.IsNullOrEmpty(args.Text) &&
                    args.Text.Any(c => !char.IsDigit(c)))
                {
                    args.Handled = true;
                }
            }, RoutingStrategies.Tunnel);

            _answerInputs.Add(answerInput);
            border.Child = answerInput;
            return border;
        }

        return null;
    }

    private Control CreateTwoRowImagePattern(
        string imageUrl,
        int? width,
        int? height,
        int topCount,
        int bottomCount)
    {
        var verticalPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 6
        };

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var bottomRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        for (int i = 0; i < topCount; i++)
        {
            var image = CreateImage(imageUrl, width, height);
            if (image != null) topRow.Children.Add(image);
        }

        for (int i = 0; i < bottomCount; i++)
        {
            var image = CreateImage(imageUrl, width, height);
            if (image != null) bottomRow.Children.Add(image);
        }

        verticalPanel.Children.Add(topRow);
        verticalPanel.Children.Add(bottomRow);
        return verticalPanel;
    }

    private Image? CreateImage(string imageUrl, int? width, int? height)
    {
        try
        {
            var uri = new Uri(imageUrl);
            var bitmap = new Bitmap(Avalonia.Platform.AssetLoader.Open(uri));

            return new Image
            {
                Source = bitmap,
                Width = width ?? bitmap.PixelSize.Width,
                Height = height ?? bitmap.PixelSize.Height,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        catch
        {
            return null;
        }
    }

    private TextBlock CreateEmojiTextBlock(string emoji, int? fontSize = null)
    {
        return new TextBlock
        {
            Text = emoji,
            FontSize = fontSize ?? 48,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private void OnCheckButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (validationMessage == null || ContentData?.CorrectAnswer == null)
        {
            return;
        }

        var firstAnswer = _answerInputs.FirstOrDefault()?.Text?.Trim();
        var isValidNumber = int.TryParse(firstAnswer, out var enteredValue);
        var isCorrect = isValidNumber && enteredValue == ContentData.CorrectAnswer.Value;

        validationMessage.Text = isCorrect ? "Correcto" : "Intenta de nuevo";
        validationMessage.Foreground = isCorrect
            ? new SolidColorBrush(Color.Parse("#65E4A3"))
            : new SolidColorBrush(Color.Parse("#FF8A8A"));

        // Notificar al ViewModel
        ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
        {
            CorrectCount = isCorrect ? 1 : 0,
            TotalCount = 1,
            PointsEarned = isCorrect ? 10 : 0,
            SectionType = "resolvamos"
        });
    }
}
