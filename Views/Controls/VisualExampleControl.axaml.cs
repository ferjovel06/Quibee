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
using Avalonia.Threading;
using Quibee.Models;
using Quibee.Services;

namespace Quibee.Views.Controls;

public partial class VisualExampleControl : UserControl
{
    private readonly List<TextBox> _answerInputs = new();
    private const string DefaultNumberImagePattern = "avares://Quibee/Assets/Images/{0}.png";
    private bool _expectsTextAnswers;
    private string? _groupKey;
    private static readonly FontFamily EmojiFontFamily = FontFamily.Parse("Segoe UI Emoji, Segoe UI Symbol");
    private static readonly FontFamily SymbolFontFamily = FontFamily.Parse("Segoe UI Symbol, Segoe UI Emoji");

    public static readonly StyledProperty<LessonContentData?> ContentDataProperty =
        AvaloniaProperty.Register<VisualExampleControl, LessonContentData?>(nameof(ContentData));

    public LessonContentData? ContentData
    {
        get => GetValue(ContentDataProperty);
        set => SetValue(ContentDataProperty, value);
    }

    public static readonly StyledProperty<LessonContent?> LessonContextProperty =
        AvaloniaProperty.Register<VisualExampleControl, LessonContent?>(nameof(LessonContext));

    public LessonContent? LessonContext
    {
        get => GetValue(LessonContextProperty);
        set => SetValue(LessonContextProperty, value);
    }

    public VisualExampleControl()
    {
        InitializeComponent();

        DetachedFromVisualTree += (_, _) => UnregisterFromGroup();

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

        if (change.Property == LessonContextProperty)
        {
            RegisterOrUpdateGroupMembership();
        }
    }

    private void UpdateContent()
    {
        if (ContentData?.Objects == null) return;

        var container = this.FindControl<StackPanel>("ObjectsContainer");
        var validationContainer = this.FindControl<StackPanel>("ValidationContainer");
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (container == null) return;

        var hasTextSingleAnswer = !string.IsNullOrWhiteSpace(ContentData.CorrectTextAnswer);
        var hasTextMultipleAnswers = ContentData.CorrectTextAnswers != null && ContentData.CorrectTextAnswers.Count > 0;
        _expectsTextAnswers = hasTextSingleAnswer || hasTextMultipleAnswers;

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

        if (string.Equals(ContentData.Layout, "columns_board", StringComparison.OrdinalIgnoreCase))
        {
            RenderColumnsBoard(container);
            return;
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

        var hasSingleAnswer = ContentData.CorrectAnswer.HasValue;
        var hasMultipleAnswers = ContentData.CorrectAnswers != null && ContentData.CorrectAnswers.Count > 0;

        if (_answerInputs.Count > 0 && (hasSingleAnswer || hasMultipleAnswers || hasTextSingleAnswer || hasTextMultipleAnswers) && validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }

        RegisterOrUpdateGroupMembership();
    }

    private bool IsSingleCheckModeForExerciseSection()
    {
        var sectionType = LessonContext?.SectionType;

        if (string.IsNullOrWhiteSpace(sectionType))
        {
            return false;
        }

        return sectionType.Equals("practiquemos", StringComparison.OrdinalIgnoreCase)
            || sectionType.Equals("ejercitemos", StringComparison.OrdinalIgnoreCase)
            || sectionType.Equals("resolvamos", StringComparison.OrdinalIgnoreCase)
            || sectionType.Equals("desafio", StringComparison.OrdinalIgnoreCase);
    }

    private void RegisterOrUpdateGroupMembership()
    {
        if (!IsSingleCheckModeForExerciseSection() || !HasEvaluableAnswers())
        {
            UnregisterFromGroup();
            return;
        }

        _groupKey = $"{LessonContext!.IdLesson}:{LessonContext.SectionType}";

        SectionCheckCoordinator.Register(
            _groupKey,
            this,
            LessonContext?.OrderIndex ?? int.MaxValue,
            EvaluateForSectionCheck,
            isHost =>
            {
                var validationContainer = this.FindControl<StackPanel>("ValidationContainer");
                if (validationContainer != null)
                {
                    validationContainer.IsVisible = isHost;
                }
            });
    }

    private bool HasEvaluableAnswers()
    {
        if (ContentData == null)
        {
            return false;
        }

        return ContentData.CorrectAnswer.HasValue
            || (ContentData.CorrectAnswers != null && ContentData.CorrectAnswers.Count > 0)
            || !string.IsNullOrWhiteSpace(ContentData.CorrectTextAnswer)
            || (ContentData.CorrectTextAnswers != null && ContentData.CorrectTextAnswers.Count > 0);
    }

    private void UnregisterFromGroup()
    {
        if (string.IsNullOrEmpty(_groupKey))
        {
            return;
        }

        SectionCheckCoordinator.Unregister(_groupKey, this);

        _groupKey = null;
    }

    private SectionCheckResult EvaluateForSectionCheck()
    {
        var result = EvaluateAnswers();
        if (result.totalCount == 0)
        {
            return new SectionCheckResult(true, 0, 0);
        }

        var isReady = _answerInputs.Count > 0 && _answerInputs.All(input => !string.IsNullOrWhiteSpace(input.Text));
        return new SectionCheckResult(isReady, result.correctCount, result.totalCount);
    }

    private (bool isCorrect, int correctCount, int totalCount) EvaluateAnswers()
    {
        if (ContentData == null)
        {
            return (false, 0, 0);
        }

        var expectedTextSingle = ContentData.CorrectTextAnswer?.Trim();
        var expectedTextMultiple = ContentData.CorrectTextAnswers?
            .Select(value => value.Trim())
            .Where(value => !string.IsNullOrEmpty(value))
            .ToList();

        var expectedMultiple = ContentData.CorrectAnswers;

        if (expectedTextMultiple != null && expectedTextMultiple.Count > 0)
        {
            var totalCount = expectedTextMultiple.Count;
            var correctCount = 0;

            for (int i = 0; i < expectedTextMultiple.Count; i++)
            {
                var inputText = i < _answerInputs.Count ? _answerInputs[i].Text?.Trim() : null;
                if (string.Equals(inputText, expectedTextMultiple[i], StringComparison.Ordinal))
                {
                    correctCount++;
                }
            }

            return (correctCount == totalCount, correctCount, totalCount);
        }

        if (!string.IsNullOrEmpty(expectedTextSingle))
        {
            var firstAnswer = _answerInputs.FirstOrDefault()?.Text?.Trim();
            var isCorrect = string.Equals(firstAnswer, expectedTextSingle, StringComparison.Ordinal);
            return (isCorrect, isCorrect ? 1 : 0, 1);
        }

        if (expectedMultiple != null && expectedMultiple.Count > 0)
        {
            var totalCount = expectedMultiple.Count;
            var correctCount = 0;

            for (int i = 0; i < expectedMultiple.Count; i++)
            {
                var inputText = i < _answerInputs.Count ? _answerInputs[i].Text?.Trim() : null;
                var valid = int.TryParse(inputText, out var enteredValue);

                if (valid && enteredValue == expectedMultiple[i])
                {
                    correctCount++;
                }
            }

            return (correctCount == totalCount, correctCount, totalCount);
        }

        if (ContentData.CorrectAnswer.HasValue)
        {
            var firstAnswer = _answerInputs.FirstOrDefault()?.Text?.Trim();
            var isValidNumber = int.TryParse(firstAnswer, out var enteredValue);
            var isCorrect = isValidNumber && enteredValue == ContentData.CorrectAnswer.Value;
            return (isCorrect, isCorrect ? 1 : 0, 1);
        }

        return (false, 0, 0);
    }

    private void RenderColumnsBoard(StackPanel container)
    {
        container.Orientation = Orientation.Horizontal;
        container.HorizontalAlignment = HorizontalAlignment.Center;
        container.VerticalAlignment = VerticalAlignment.Top;
        container.Spacing = ContentData?.Spacing ?? 16;

        if (ContentData?.Objects == null)
        {
            return;
        }

        foreach (var obj in ContentData.Objects)
        {
            var column = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Spacing = 8,
                Width = obj.Width ?? 150
            };

            var visual = CreateColumnVisual(obj);
            if (visual != null)
            {
                column.Children.Add(visual);
            }

            var label = !string.IsNullOrWhiteSpace(obj.Symbol)
                ? obj.Symbol
                : obj.Number;

            if (!string.IsNullOrWhiteSpace(label))
            {
                var labelBadge = new Border
                {
                    Background = new SolidColorBrush(Color.Parse("#F29A2E")),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(12, 4),
                    Width = obj.Width ?? 150,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Child = new TextBlock
                    {
                        Text = label,
                        FontSize = 14,
                        FontWeight = FontWeight.SemiBold,
                        Foreground = Brushes.White,
                        FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins"),
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    }
                };

                column.Children.Add(labelBadge);
            }

            container.Children.Add(column);
        }
    }

    private Control? CreateColumnVisual(VisualObject obj)
    {
        if (!string.IsNullOrEmpty(obj.ImageUrl) && obj.Count.HasValue && obj.Count > 1)
        {
            var (topCount, bottomCount) = GetRowDistribution(obj.Count.Value);
            return CreateTwoRowImagePattern(obj.ImageUrl, obj.Width, obj.Height, topCount, bottomCount);
        }

        if (!string.IsNullOrEmpty(obj.Emoji) && obj.Count.HasValue && obj.Count > 1)
        {
            var (topCount, bottomCount) = GetRowDistribution(obj.Count.Value);
            return CreateTwoRowEmojiPatternFitted(
                obj.Emoji,
                obj.FontSize,
                topCount,
                bottomCount,
                obj.Width ?? 150);
        }

        if (!string.IsNullOrEmpty(obj.ImageUrl))
        {
            return CreateImage(obj.ImageUrl, obj.Width, obj.Height);
        }

        if (!string.IsNullOrEmpty(obj.Emoji))
        {
            return CreateEmojiTextBlock(obj.Emoji, obj.FontSize);
        }

        return null;
    }

    private Control CreateTwoRowEmojiPatternFitted(
        string emoji,
        int? fontSize,
        int topCount,
        int bottomCount,
        int columnWidth)
    {
        const int rowSpacing = 2;
        var maxCount = Math.Max(topCount, bottomCount);
        var availableWidth = Math.Max(1, columnWidth - ((maxCount - 1) * rowSpacing));
        var cellWidth = Math.Max(22, availableWidth / Math.Max(1, maxCount));
        var effectiveFontSize = Math.Min(fontSize ?? 34, cellWidth + 4);

        var verticalPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 3,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = rowSpacing,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var bottomRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = rowSpacing,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        for (int i = 0; i < topCount; i++)
        {
            topRow.Children.Add(CreateEmojiCell(emoji, effectiveFontSize, cellWidth));
        }

        for (int i = 0; i < bottomCount; i++)
        {
            bottomRow.Children.Add(CreateEmojiCell(emoji, effectiveFontSize, cellWidth));
        }

        verticalPanel.Children.Add(topRow);
        verticalPanel.Children.Add(bottomRow);
        return verticalPanel;
    }

    private static Control CreateEmojiCell(string emoji, int fontSize, int width)
    {
        return new Border
        {
            Width = width,
            Background = Brushes.Transparent,
            Child = new TextBlock
            {
                Text = emoji,
                FontSize = fontSize,
                FontFamily = EmojiFontFamily,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            }
        };
    }

    private Control? CreateVisualElement(VisualObject obj)
    {
        // Si es una imagen repetida
        if (!string.IsNullOrEmpty(obj.ImageUrl) && obj.Count.HasValue && obj.Count > 1)
        {
            // Para cantidades >= 3, usar bloques en dos filas para evitar layouts demasiado lineales.
            if (obj.Count.Value >= 3)
            {
                var (topCount, bottomCount) = GetRowDistribution(obj.Count.Value);
                return CreateTwoRowImagePattern(obj.ImageUrl, obj.Width, obj.Height, topCount, bottomCount);
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
            if (obj.Count.Value >= 3)
            {
                var (topCount, bottomCount) = GetRowDistribution(obj.Count.Value);
                return CreateTwoRowEmojiPattern(obj.Emoji, obj.FontSize, topCount, bottomCount);
            }

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
            var isLongText = obj.Symbol.Length > 4;
            var isParagraphLike = obj.Symbol.Length > 20;
            var isArrow = obj.Symbol == "→";

            if (isParagraphLike)
            {

                var paragraph = new TextBlock
                {
                    Text = obj.Symbol,
                    FontSize = Math.Min(obj.FontSize ?? 36, 42),
                    FontWeight = FontWeight.Bold,
                    FontFamily = SymbolFontFamily,
                    Foreground = !string.IsNullOrEmpty(obj.Color)
                        ? new SolidColorBrush(Color.Parse(obj.Color))
                        : Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Left,
                    MaxWidth = 620,
                    LineHeight = 44
                };

                return new Border
                {
                    Background = Brushes.Transparent,
                    Margin = new Thickness(6, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = paragraph
                };
            }

            return new TextBlock
            {
                Text = obj.Symbol,
                FontSize = obj.FontSize ?? 36,
                FontWeight = FontWeight.Bold,
                FontFamily = SymbolFontFamily,
                Foreground = !string.IsNullOrEmpty(obj.Color)
                    ? new SolidColorBrush(Color.Parse(obj.Color))
                    : Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = isLongText
                    ? new Thickness(2, 0)
                    : isArrow
                        ? new Thickness(4, 0)
                        : new Thickness(6, 0),
                TextWrapping = TextWrapping.NoWrap
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

            var answerImagesPanel = new StackPanel
            {
                IsVisible = false,
                IsHitTestVisible = false,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 2
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

            answerInput.TextChanged += (_, _) =>
            {
                UpdateAnswerBoxVisual(answerInput, answerImagesPanel, obj.Width ?? 80, obj.Height ?? 80);
            };

            answerInput.KeyUp += (_, _) =>
            {
                UpdateAnswerBoxVisual(answerInput, answerImagesPanel, obj.Width ?? 80, obj.Height ?? 80);
            };

            answerInput.PropertyChanged += (_, e) =>
            {
                if (e.Property == TextBox.TextProperty)
                {
                    Dispatcher.UIThread.Post(() => UpdateAnswerBoxVisual(answerInput, answerImagesPanel, obj.Width ?? 80, obj.Height ?? 80));
                }
            };

            // Permitir solo números cuando no se esperan operadores/texto.
            answerInput.AddHandler(TextInputEvent, (_, e) =>
            {
                if (_expectsTextAnswers)
                {
                    return;
                }

                if (e is TextInputEventArgs args &&
                    !string.IsNullOrEmpty(args.Text) &&
                    args.Text.Any(c => !char.IsDigit(c)))
                {
                    args.Handled = true;
                }
            }, RoutingStrategies.Tunnel);

            _answerInputs.Add(answerInput);

            var inputLayer = new Grid();
            inputLayer.Children.Add(answerInput);
            inputLayer.Children.Add(answerImagesPanel);

            border.Child = inputLayer;
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
            Spacing = 4
        };

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 6,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var bottomRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 6,
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

    private Control CreateTwoRowEmojiPattern(
        string emoji,
        int? fontSize,
        int topCount,
        int bottomCount)
    {
        var verticalPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 3
        };

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var bottomRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        for (int i = 0; i < topCount; i++)
        {
            topRow.Children.Add(CreateEmojiTextBlock(emoji, fontSize));
        }

        for (int i = 0; i < bottomCount; i++)
        {
            bottomRow.Children.Add(CreateEmojiTextBlock(emoji, fontSize));
        }

        verticalPanel.Children.Add(topRow);
        verticalPanel.Children.Add(bottomRow);
        return verticalPanel;
    }

    private static (int topCount, int bottomCount) GetRowDistribution(int count)
    {
        return count switch
        {
            3 => (2, 1),
            4 => (2, 2),
            5 => (3, 2),
            6 => (3, 3),
            7 => (4, 3),
            8 => (4, 4),
            9 => (5, 4),
            10 => (5, 5),
            _ => ((int)Math.Ceiling(count / 2.0), count / 2)
        };
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
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(0, 2, 0, 0)
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
            FontFamily = EmojiFontFamily,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private void UpdateAnswerBoxVisual(TextBox answerInput, StackPanel answerImagesPanel, int boxWidth, int boxHeight)
    {
        if (_expectsTextAnswers)
        {
            answerImagesPanel.IsVisible = false;
            answerImagesPanel.Children.Clear();
            answerInput.Foreground = new SolidColorBrush(Color.Parse("#FFFFFF"));
            answerInput.CaretBrush = new SolidColorBrush(Color.Parse("#FFFFFF"));
            return;
        }

        var text = answerInput.Text?.Trim();

        if (!string.IsNullOrEmpty(text) && text.All(char.IsDigit))
        {
            answerImagesPanel.Children.Clear();

            var digitCount = text.Length;
            var targetWidth = Math.Max(18, (boxWidth - 14 - ((digitCount - 1) * 2)) / Math.Max(1, digitCount));
            var targetHeight = Math.Max(22, boxHeight - 16);

            foreach (var ch in text)
            {
                var number = ch - '0';
                var imageUrl = string.Format(DefaultNumberImagePattern, number);
                var numberImage = CreateImage(imageUrl, targetWidth, targetHeight);

                if (numberImage != null)
                {
                    answerImagesPanel.Children.Add(numberImage);
                }
            }

            if (answerImagesPanel.Children.Count > 0)
            {
                answerImagesPanel.IsVisible = true;
                answerInput.Foreground = Brushes.Transparent;
                answerInput.CaretBrush = Brushes.Transparent;
                return;
            }
        }

        answerImagesPanel.IsVisible = false;
        answerImagesPanel.Children.Clear();
        answerInput.Foreground = new SolidColorBrush(Color.Parse("#FFFFFF"));
        answerInput.CaretBrush = new SolidColorBrush(Color.Parse("#FFFFFF"));
    }

    private void OnCheckButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (validationMessage == null || ContentData == null)
        {
            return;
        }

        var sectionType = LessonContext?.SectionType ?? "resolvamos";

        if (IsSingleCheckModeForExerciseSection())
        {
            if (string.IsNullOrEmpty(_groupKey) || !SectionCheckCoordinator.IsHost(_groupKey, this))
            {
                return;
            }

            var groupResult = SectionCheckCoordinator.EvaluateGroup(_groupKey);
            if (!groupResult.AllReady)
            {
                validationMessage.Text = "Completa todos los ejercicios antes de comprobar";
                validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FFD166"));
                return;
            }

            var groupTotalCount = groupResult.TotalCount;
            var groupCorrectCount = groupResult.CorrectCount;

            var allCorrect = groupTotalCount > 0 && groupCorrectCount == groupTotalCount;
            validationMessage.Text = allCorrect
                ? "¡Excelente! Todas las respuestas son correctas"
                : $"{groupCorrectCount}/{groupTotalCount} correctas. Intenta de nuevo";
            validationMessage.Foreground = allCorrect
                ? new SolidColorBrush(Color.Parse("#65E4A3"))
                : new SolidColorBrush(Color.Parse("#FF8A8A"));

            ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
            {
                CorrectCount = groupCorrectCount,
                TotalCount = groupTotalCount,
                PointsEarned = groupCorrectCount * 10,
                SectionType = sectionType,
                ContentId = LessonContext?.IdContent ?? 0,
                IsSectionAggregate = true
            });

            return;
        }

        var resultSingle = EvaluateAnswers();
        var isCorrect = resultSingle.isCorrect;
        var correctCount = resultSingle.correctCount;
        var totalCount = resultSingle.totalCount;

        if (totalCount == 0)
        {
            return;
        }

        validationMessage.Text = isCorrect ? "Correcto" : "Intenta de nuevo";
        validationMessage.Foreground = isCorrect
            ? new SolidColorBrush(Color.Parse("#65E4A3"))
            : new SolidColorBrush(Color.Parse("#FF8A8A"));

        // Notificar al ViewModel
        ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
        {
            CorrectCount = correctCount,
            TotalCount = totalCount,
            PointsEarned = correctCount * 10,
            SectionType = sectionType,
            ContentId = LessonContext?.IdContent ?? 0,
            IsSectionAggregate = false
        });
    }
}
