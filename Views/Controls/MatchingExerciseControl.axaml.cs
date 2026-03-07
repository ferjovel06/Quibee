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

public partial class MatchingExerciseControl : UserControl
{
    /// <summary>
    /// Casillas donde se sueltan los números. Clave = índice de fila.
    /// </summary>
    private readonly Dictionary<int, Border> _dropTargets = new();

    /// <summary>
    /// Valores actuales colocados en cada casilla. Clave = índice de fila.
    /// </summary>
    private readonly Dictionary<int, int?> _placedValues = new();

    /// <summary>
    /// Respuestas correctas por fila.
    /// </summary>
    private readonly Dictionary<int, int> _correctAnswers = new();
    private readonly Dictionary<int, string?> _placedSymbols = new();
    private readonly Dictionary<int, string> _correctSymbolAnswers = new();

    /// <summary>
    /// Referencia a los tiles de números para poder devolverlos.
    /// </summary>
    private readonly List<Border> _numberTiles = new();
    private readonly List<Border> _symbolTiles = new();
    private Border? _selectedOptionTile;

    private bool IsSequenceGridMode() =>
        string.Equals(ContentData?.Layout, "sequence_grid", StringComparison.OrdinalIgnoreCase);

    private bool IsPairedCountsMode() =>
        string.Equals(ContentData?.Layout, "paired_counts", StringComparison.OrdinalIgnoreCase);

    private bool IsChallengeBoardMode() =>
        string.Equals(ContentData?.Layout, "challenge_board", StringComparison.OrdinalIgnoreCase);

    private bool IsOperatorCompareMode() =>
        string.Equals(ContentData?.Layout, "operator_compare", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(ContentData?.Layout, "operator_compare_compact", StringComparison.OrdinalIgnoreCase);

    private bool IsOperatorCompareCompactMode() =>
        string.Equals(ContentData?.Layout, "operator_compare_compact", StringComparison.OrdinalIgnoreCase);

    private bool IsOrderTokensMode() =>
        string.Equals(ContentData?.Layout, "order_tokens", StringComparison.OrdinalIgnoreCase);

    public static readonly StyledProperty<LessonContentData?> ContentDataProperty =
        AvaloniaProperty.Register<MatchingExerciseControl, LessonContentData?>(nameof(ContentData));

    public LessonContentData? ContentData
    {
        get => GetValue(ContentDataProperty);
        set => SetValue(ContentDataProperty, value);
    }

    public MatchingExerciseControl()
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
            BuildExercise();
        }
    }

    private void BuildExercise()
    {
        if (ContentData?.Rows == null || ContentData.Rows.Count == 0) return;

        var rowsContainer = this.FindControl<StackPanel>("RowsContainer");
        var numbersContainer = this.FindControl<StackPanel>("NumbersContainer");
        var numbersPanelBorder = this.FindControl<Border>("NumbersPanelBorder");
        var mainLayoutGrid = this.FindControl<Grid>("MainLayoutGrid");
        var instructionText = this.FindControl<TextBlock>("InstructionText");
        var validationContainer = this.FindControl<StackPanel>("ValidationContainer");

        if (rowsContainer == null || numbersContainer == null) return;

        var safeNumbersContainer = numbersContainer ?? throw new InvalidOperationException("NumbersContainer not found");

        rowsContainer.Children.Clear();
        numbersContainer.Children.Clear();
        _dropTargets.Clear();
        _placedValues.Clear();
        _correctAnswers.Clear();
        _placedSymbols.Clear();
        _correctSymbolAnswers.Clear();
        _numberTiles.Clear();
        _symbolTiles.Clear();

        if (IsSequenceGridMode())
        {
            BuildSequenceGridExercise(rowsContainer, instructionText, numbersPanelBorder, mainLayoutGrid);

            if (validationContainer != null)
            {
                validationContainer.IsVisible = true;
            }

            return;
        }

        if (IsOperatorCompareMode())
        {
            BuildOperatorCompareExercise(rowsContainer, numbersContainer, instructionText, numbersPanelBorder, mainLayoutGrid, validationContainer);
            return;
        }

        if (IsOrderTokensMode())
        {
            BuildOrderTokensExercise(rowsContainer, numbersContainer, instructionText, numbersPanelBorder, mainLayoutGrid, validationContainer);
            return;
        }

        if (IsPairedCountsMode())
        {
            if (instructionText != null)
            {
                instructionText.IsVisible = false;
            }

            if (numbersPanelBorder != null)
            {
                numbersPanelBorder.Background = Brushes.Transparent;
                numbersPanelBorder.CornerRadius = new CornerRadius(0);
                numbersPanelBorder.Padding = new Thickness(0);
            }

            if (numbersContainer != null)
            {
                numbersContainer.Spacing = 8;
                numbersContainer.HorizontalAlignment = HorizontalAlignment.Center;
            }

            rowsContainer.HorizontalAlignment = HorizontalAlignment.Center;
            rowsContainer.Spacing = 12;

            if (mainLayoutGrid != null)
            {
                mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*,Auto");
                mainLayoutGrid.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
        else if (IsChallengeBoardMode())
        {
            if (instructionText != null)
            {
                instructionText.IsVisible = false;
            }

            if (numbersPanelBorder != null)
            {
                numbersPanelBorder.Background = new SolidColorBrush(Color.Parse("#6B5A7E"));
                numbersPanelBorder.CornerRadius = new CornerRadius(0);
                numbersPanelBorder.Padding = new Thickness(10, 8);
            }

            numbersContainer.Spacing = 8;
            numbersContainer.HorizontalAlignment = HorizontalAlignment.Center;
            rowsContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            rowsContainer.Spacing = 8;

            if (mainLayoutGrid != null)
            {
                mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*,Auto");
                mainLayoutGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }
        else
        {
            if (instructionText != null)
            {
                instructionText.IsVisible = true;
            }

            if (numbersPanelBorder != null)
            {
                numbersPanelBorder.Background = new SolidColorBrush(Color.Parse("#4A3560"));
                numbersPanelBorder.CornerRadius = new CornerRadius(16);
                numbersPanelBorder.Padding = new Thickness(16, 20);
            }

            if (mainLayoutGrid != null)
            {
                mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*,Auto");
                mainLayoutGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            }

            rowsContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        if (numbersPanelBorder != null)
        {
            numbersPanelBorder.IsVisible = true;
        }

        // Instrucción
        if (instructionText != null && !string.IsNullOrEmpty(ContentData.Instruction))
        {
            instructionText.Text = ContentData.Instruction;
        }

        // Colores para filas alternadas
        var rowColors = new[]
        {
            "#5B4A6E", "#4A3D5E", "#6B5A7E", "#3E3450"
        };

        // Crear filas
        if (IsPairedCountsMode())
        {
            for (int i = 0; i < ContentData.Rows.Count; i += 2)
            {
                var pairPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 24,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                for (int j = i; j < Math.Min(i + 2, ContentData.Rows.Count); j++)
                {
                    var row = ContentData.Rows[j];
                    _correctAnswers[j] = row.CorrectAnswer;
                    _placedValues[j] = null;

                    var bgColor = !string.IsNullOrEmpty(row.RowColor)
                        ? row.RowColor
                        : rowColors[j % rowColors.Length];

                    var rowPanel = CreateRow(j, row, bgColor);
                    pairPanel.Children.Add(rowPanel);
                }

                rowsContainer.Children.Add(pairPanel);
            }
        }
        else
        {
            for (int i = 0; i < ContentData.Rows.Count; i++)
            {
                var row = ContentData.Rows[i];
                _correctAnswers[i] = row.CorrectAnswer;
                _placedValues[i] = null;

                var bgColor = !string.IsNullOrEmpty(row.RowColor)
                    ? row.RowColor
                    : rowColors[i % rowColors.Length];

                var rowPanel = CreateRow(i, row, bgColor);
                rowsContainer.Children.Add(rowPanel);
            }
        }

        // Crear números arrastrables (mezclados)
        var answers = ContentData!.Rows!.Select(r => r.CorrectAnswer).ToList();
        var shuffled = ShuffleList(answers);

        foreach (var number in shuffled)
        {
            var tile = CreateNumberTile(number);
            safeNumbersContainer.Children.Add(tile);
            _numberTiles.Add(tile);
        }

        // Mostrar botón de validación
        if (validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }
    }

    private void BuildSequenceGridExercise(
        StackPanel rowsContainer,
        TextBlock? instructionText,
        Border? numbersPanelBorder,
        Grid? mainLayoutGrid)
    {
        if (instructionText != null)
        {
            instructionText.IsVisible = false;
        }

        if (numbersPanelBorder != null)
        {
            numbersPanelBorder.IsVisible = false;
        }

        if (mainLayoutGrid != null)
        {
            mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*");
        }

        rowsContainer.Spacing = 14;
        rowsContainer.Margin = new Thickness(0);

        var numbersToPlace = ContentData?.Numbers?.Count > 0
            ? ContentData.Numbers
            : ContentData!.Rows!.Select(r => r.CorrectAnswer).ToList();

        var numbersRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 8
        };

        foreach (var number in numbersToPlace!)
        {
            var tile = CreateNumberTile(number);
            numbersRow.Children.Add(tile);
            _numberTiles.Add(tile);
        }

        rowsContainer.Children.Add(numbersRow);

        rowsContainer.Children.Add(new TextBlock
        {
            Text = "Números en orden:",
            FontSize = 16,
            Foreground = Brushes.White,
            FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-Regular.ttf#Poppins"),
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 4, 0, 0)
        });

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 14
        };

        var bottomRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 14
        };

        for (int i = 0; i < ContentData!.Rows!.Count; i++)
        {
            var expected = ContentData.Rows[i].CorrectAnswer;
            _correctAnswers[i] = expected;
            _placedValues[i] = null;

            var slot = CreateSequenceSlot(i);
            if (i < 5)
            {
                topRow.Children.Add(slot);
            }
            else
            {
                bottomRow.Children.Add(slot);
            }
        }

        rowsContainer.Children.Add(topRow);
        rowsContainer.Children.Add(bottomRow);
    }

    private void BuildOperatorCompareExercise(
        StackPanel rowsContainer,
        StackPanel numbersContainer,
        TextBlock? instructionText,
        Border? numbersPanelBorder,
        Grid? mainLayoutGrid,
        StackPanel? validationContainer)
    {
        var compactMode = IsOperatorCompareCompactMode();

        if (instructionText != null)
        {
            instructionText.IsVisible = false;
        }

        if (numbersPanelBorder != null)
        {
            numbersPanelBorder.IsVisible = true;
            numbersPanelBorder.Background = Brushes.Transparent;
            numbersPanelBorder.CornerRadius = new CornerRadius(0);
            numbersPanelBorder.Padding = new Thickness(0);
            numbersPanelBorder.VerticalAlignment = VerticalAlignment.Center;
        }

        if (mainLayoutGrid != null)
        {
            mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*,Auto");
            mainLayoutGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        rowsContainer.Spacing = compactMode ? 8 : 12;
        rowsContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
        rowsContainer.Margin = compactMode
            ? new Thickness(0, 0, 14, 0)
            : new Thickness(0, 0, 20, 0);

        if (ContentData?.Rows != null)
        {
            for (int i = 0; i < ContentData.Rows.Count; i++)
            {
                var row = ContentData.Rows[i];
                _placedSymbols[i] = null;
                _correctSymbolAnswers[i] = row.CorrectSymbolAnswer ?? "=";

                var rowControl = CreateOperatorCompareRow(i, row, compactMode);
                rowsContainer.Children.Add(rowControl);
            }
        }

        numbersContainer.Spacing = 12;
        numbersContainer.Orientation = Orientation.Vertical;
        numbersContainer.HorizontalAlignment = HorizontalAlignment.Center;

        var sideInstruction = new TextBlock
        {
            Text = ContentData?.Instruction ?? "Coloca el operador correcto sobre la línea.",
            FontSize = 16,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brushes.White,
            TextWrapping = TextWrapping.Wrap,
            MaxWidth = 190,
            TextAlignment = TextAlignment.Left,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        numbersContainer.Children.Add(sideInstruction);

        var symbolsRow = new StackPanel
        {
            Orientation = compactMode ? Orientation.Vertical : Orientation.Horizontal,
            Spacing = compactMode ? 8 : 10,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var options = ContentData?.Symbols != null && ContentData.Symbols.Count > 0
            ? ContentData.Symbols
            : new List<string> { "<", ">", "=" };

        var symbolOptions = compactMode
            ? options.ToList()
            : ShuffleList(options.ToList());

        foreach (var symbol in symbolOptions)
        {
            var tile = CreateSymbolTile(symbol);
            symbolsRow.Children.Add(tile);
            _symbolTiles.Add(tile);
        }

        numbersContainer.Children.Add(symbolsRow);

        if (validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }
    }

    private void BuildOrderTokensExercise(
        StackPanel rowsContainer,
        StackPanel numbersContainer,
        TextBlock? instructionText,
        Border? numbersPanelBorder,
        Grid? mainLayoutGrid,
        StackPanel? validationContainer)
    {
        var isSelectOnly = ContentData?.SelectOnly == true;

        if (instructionText != null)
        {
            instructionText.IsVisible = false;
        }

        if (numbersPanelBorder != null)
        {
            numbersPanelBorder.IsVisible = false;
            numbersPanelBorder.Background = Brushes.Transparent;
            numbersPanelBorder.CornerRadius = new CornerRadius(0);
            numbersPanelBorder.Padding = new Thickness(0);
            numbersPanelBorder.VerticalAlignment = VerticalAlignment.Center;
        }

        if (mainLayoutGrid != null)
        {
            mainLayoutGrid.ColumnDefinitions = ColumnDefinitions.Parse("*");
            mainLayoutGrid.HorizontalAlignment = HorizontalAlignment.Center;
        }

        rowsContainer.Spacing = 10;
        rowsContainer.HorizontalAlignment = HorizontalAlignment.Center;
        rowsContainer.Margin = new Thickness(0, 0, 20, 0);

        var visualRow = ContentData?.Rows?.FirstOrDefault(r =>
            (!string.IsNullOrEmpty(r.ImageUrl) && r.Count > 0) ||
            (!string.IsNullOrEmpty(r.RightImageUrl) && (r.RightCount ?? 0) > 0));

        if (visualRow != null)
        {
            var comparePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 6)
            };

            comparePanel.Children.Add(CreateRepeatedObjectsPanel(
                visualRow.ImageUrl,
                visualRow.Emoji,
                visualRow.Count,
                visualRow.ImageWidth ?? 32,
                visualRow.ImageHeight ?? 32));

            comparePanel.Children.Add(new TextBlock
            {
                Text = "",
                Width = 14
            });

            comparePanel.Children.Add(CreateRepeatedObjectsPanel(
                visualRow.RightImageUrl,
                visualRow.RightEmoji,
                visualRow.RightCount ?? 0,
                visualRow.RightImageWidth ?? visualRow.ImageWidth ?? 32,
                visualRow.RightImageHeight ?? visualRow.ImageHeight ?? 32));

            rowsContainer.Children.Add(comparePanel);
        }

        if (ContentData?.ShowOrderPrompt != false)
        {
            rowsContainer.Children.Add(new TextBlock
            {
                Text = "Ordena tu respuesta:",
                FontSize = 20,
                FontWeight = FontWeight.SemiBold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            });
        }

        var slotsRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        bool hasPromptRows = ContentData?.Rows != null && ContentData.Rows.Any(r =>
            !string.IsNullOrWhiteSpace(r.PromptText) || !string.IsNullOrWhiteSpace(r.FixedSymbol));

        if (ContentData?.Rows != null)
        {
            for (int i = 0; i < ContentData.Rows.Count; i++)
            {
                var row = ContentData.Rows[i];
                _placedSymbols[i] = null;
                _correctSymbolAnswers[i] = row.CorrectSymbolAnswer ?? string.Empty;

                if (hasPromptRows && !isSelectOnly)
                {
                    rowsContainer.Children.Add(CreateOrderTokenPromptRow(i, row));
                }
                else if (!isSelectOnly)
                {
                    slotsRow.Children.Add(CreateOrderTokenSlot(i));
                }
            }
        }

        if (!hasPromptRows && !isSelectOnly)
        {
            rowsContainer.Children.Add(slotsRow);
        }

        if (isSelectOnly && ContentData?.Rows != null && ContentData.Rows.Count > 0)
        {
            var row = ContentData.Rows[0];
            if (!string.IsNullOrWhiteSpace(row.PromptText))
            {
                rowsContainer.Children.Add(new TextBlock
                {
                    Text = row.PromptText,
                    FontSize = 20,
                    FontWeight = FontWeight.SemiBold,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 6)
                });
            }
        }

        numbersContainer.Children.Clear();

        var options = ContentData?.Symbols != null && ContentData.Symbols.Count > 0
            ? ContentData.Symbols
            : new List<string> { "3", "5", "<", ">" };

        if (isSelectOnly)
        {
            var rowsNeeded = (int)Math.Ceiling(options.Count / 2.0);
            var optionsGrid = new Grid
            {
                ColumnDefinitions = ColumnDefinitions.Parse("Auto,Auto"),
                RowDefinitions = RowDefinitions.Parse(string.Join(",", Enumerable.Repeat("Auto", Math.Max(1, rowsNeeded)))),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 6, 0, 0),
                ColumnSpacing = 16,
                RowSpacing = 12
            };

            for (var i = 0; i < options.Count; i++)
            {
                var tile = CreateSymbolTile(options[i]);
                tile.Margin = new Thickness(2);
                Grid.SetColumn(tile, i % 2);
                Grid.SetRow(tile, i / 2);
                optionsGrid.Children.Add(tile);
                _symbolTiles.Add(tile);
            }

            rowsContainer.Children.Add(optionsGrid);
        }
        else
        {
            var tokensContainer = new StackPanel
            {
                Spacing = 10,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            foreach (var token in options)
            {
                var tile = CreateSymbolTile(token);
                tokensContainer.Children.Add(tile);
                _symbolTiles.Add(tile);
            }

            rowsContainer.Children.Add(tokensContainer);
        }

        if (validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }
    }

    private Border CreateOrderTokenPromptRow(int rowIndex, MatchingRow row)
    {
        var rowBorder = new Border
        {
            Width = 420,
            BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D")),
            BorderThickness = new Thickness(1),
            Background = Brushes.Transparent,
            Padding = new Thickness(12, 8),
            Margin = new Thickness(0, 0, 0, 6)
        };

        var rowPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 10
        };

        var prompt = new TextBlock
        {
            Text = row.PromptText ?? string.Empty,
            FontSize = 16,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brushes.White,
            VerticalAlignment = VerticalAlignment.Center,
            Width = 160,
            TextAlignment = TextAlignment.Left
        };

        rowPanel.Children.Add(prompt);

        var fixedPosition = (row.FixedSymbolPosition ?? "left").ToLowerInvariant();
        var slot = CreateOrderTokenSlot(rowIndex);

        if (!string.IsNullOrWhiteSpace(row.FixedSymbol) && fixedPosition == "left")
        {
            rowPanel.Children.Add(CreateFixedToken(row.FixedSymbol!));
            rowPanel.Children.Add(slot);
        }
        else if (!string.IsNullOrWhiteSpace(row.FixedSymbol) && fixedPosition == "right")
        {
            rowPanel.Children.Add(slot);
            rowPanel.Children.Add(CreateFixedToken(row.FixedSymbol!));
        }
        else
        {
            rowPanel.Children.Add(slot);
        }

        rowBorder.Child = rowPanel;
        return rowBorder;
    }

    private Border CreateFixedToken(string symbol)
    {
        var token = new Border
        {
            Width = symbol.All(char.IsDigit) && symbol.Length > 1 ? 86 : 54,
            Height = 54,
            Background = Brushes.Transparent,
            Child = CreateSymbolContent(symbol, forDropTarget: false),
            VerticalAlignment = VerticalAlignment.Center
        };

        return token;
    }

    private Border CreateOrderTokenSlot(int rowIndex)
    {
        var slotWidth = GetOrderTokenSlotWidth();

        var dropBorder = new Border
        {
            Width = slotWidth,
            Height = 56,
            Background = new SolidColorBrush(Color.Parse("#EDEDED")),
            BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D")),
            BorderThickness = new Thickness(1.5),
            CornerRadius = new CornerRadius(2),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Tag = rowIndex
        };

        dropBorder.Child = CreateDefaultDropPlaceholder();
        DragDrop.SetAllowDrop(dropBorder, true);
        dropBorder.AddHandler(DragDrop.DropEvent, OnDrop);
        dropBorder.AddHandler(DragDrop.DragOverEvent, OnDragOver);
        dropBorder.PointerPressed += (_, _) => OnDropTargetTapped(rowIndex);

        _dropTargets[rowIndex] = dropBorder;
        return dropBorder;
    }

    private int GetOrderTokenSlotWidth()
    {
        var maxLen = ContentData?.Symbols?
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Length)
            .DefaultIfEmpty(2)
            .Max() ?? 2;

        if (maxLen >= 7) return 180;
        if (maxLen >= 5) return 140;
        if (maxLen >= 3) return 110;
        return 88;
    }

    private Border CreateOperatorCompareRow(int rowIndex, MatchingRow row, bool compactMode)
    {
        var rowBorder = new Border
        {
            Background = Brushes.Transparent,
            Padding = new Thickness(0),
            Margin = new Thickness(0, 0, 0, compactMode ? 4 : 6)
        };

        var rowPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = compactMode ? 10 : 14
        };

        rowPanel.Children.Add(CreateRepeatedObjectsPanel(
            row.ImageUrl,
            row.Emoji,
            row.Count,
            row.ImageWidth ?? 34,
            row.ImageHeight ?? 34));

        var lineWithDrop = new Grid
        {
            Width = compactMode ? 56 : 170,
            Height = 56,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var dropBorder = new Border
        {
            Width = 42,
            Height = 42,
            Background = new SolidColorBrush(Color.Parse("#EDEDED")),
            BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D")),
            BorderThickness = new Thickness(1.5),
            CornerRadius = new CornerRadius(4),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Tag = rowIndex
        };

        dropBorder.Child = CreateDefaultDropPlaceholder();
        DragDrop.SetAllowDrop(dropBorder, true);
        dropBorder.AddHandler(DragDrop.DropEvent, OnDrop);
        dropBorder.AddHandler(DragDrop.DragOverEvent, OnDragOver);
        dropBorder.PointerPressed += (_, _) => OnDropTargetTapped(rowIndex);

        _dropTargets[rowIndex] = dropBorder;
        lineWithDrop.Children.Add(dropBorder);

        rowPanel.Children.Add(lineWithDrop);

        rowPanel.Children.Add(CreateRepeatedObjectsPanel(
            row.RightImageUrl,
            row.RightEmoji,
            row.RightCount ?? 0,
            row.RightImageWidth ?? row.ImageWidth ?? 34,
            row.RightImageHeight ?? row.ImageHeight ?? 34));

        rowBorder.Child = rowPanel;
        return rowBorder;
    }

    private Control CreateRepeatedObjectsPanel(string? imageUrl, string? emoji, int count, int width, int height)
    {
        var container = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 2
        };

        var topRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 2
        };

        var bottomRow = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 2
        };

        var (topCount, bottomCount) = GetStackedDistribution(Math.Max(1, count));

        for (int index = 0; index < topCount; index++)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var img = CreateImage(imageUrl, width, height);
                if (img != null)
                {
                    img.Margin = new Thickness(1);
                    topRow.Children.Add(img);
                }
            }
            else if (!string.IsNullOrEmpty(emoji))
            {
                topRow.Children.Add(new TextBlock
                {
                    Text = emoji,
                    FontSize = 34,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1)
                });
            }
        }

        for (int index = 0; index < bottomCount; index++)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var img = CreateImage(imageUrl, width, height);
                if (img != null)
                {
                    img.Margin = new Thickness(1);
                    bottomRow.Children.Add(img);
                }
            }
            else if (!string.IsNullOrEmpty(emoji))
            {
                bottomRow.Children.Add(new TextBlock
                {
                    Text = emoji,
                    FontSize = 34,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(1)
                });
            }
        }

        container.Children.Add(topRow);
        if (bottomCount > 0)
        {
            container.Children.Add(bottomRow);
        }

        return container;
    }

    /// <summary>
    /// Crea una fila con objetos repetidos + casilla de drop.
    /// </summary>
    private Border CreateRow(int rowIndex, MatchingRow row, string bgColor)
    {
        var pairedMode = IsPairedCountsMode();
        var challengeMode = IsChallengeBoardMode();

        var rowBorder = new Border
        {
            Background = pairedMode
                ? Brushes.Transparent
                : challengeMode
                    ? new SolidColorBrush(Color.Parse(bgColor))
                : new SolidColorBrush(Color.Parse(bgColor)),
            CornerRadius = pairedMode || challengeMode ? new CornerRadius(0) : new CornerRadius(16),
            Padding = pairedMode
                ? new Thickness(0)
                : challengeMode
                    ? new Thickness(10, 8)
                    : new Thickness(20, 14),
            Margin = new Thickness(0, 0, 0, pairedMode ? 8 : challengeMode ? 6 : 0)
        };

        var grid = new Grid
        {
            ColumnDefinitions = pairedMode
                ? ColumnDefinitions.Parse("Auto,*")
                : ColumnDefinitions.Parse("*,Auto")
        };

        var objectsPanel = CreateObjectsPanel(row, pairedMode, challengeMode);

        Grid.SetColumn(objectsPanel, pairedMode ? 1 : 0);
        grid.Children.Add(objectsPanel);

        // Casilla de drop (donde se suelta el número)
        int dropSize = pairedMode || challengeMode ? 44 : (ContentData?.NumberImageWidth ?? 70);
        var dropBorder = new Border
        {
            Width = dropSize,
            Height = pairedMode || challengeMode ? 44 : (ContentData?.NumberImageHeight ?? 70),
            Background = pairedMode
                ? new SolidColorBrush(Color.Parse("#EDEDED"))
                : challengeMode
                    ? new SolidColorBrush(Color.Parse("#EDEDED"))
                : new SolidColorBrush(Color.Parse("#7A678A")),
            BorderBrush = pairedMode
                ? new SolidColorBrush(Color.Parse("#8D8D8D"))
                : challengeMode
                    ? new SolidColorBrush(Color.Parse("#8D8D8D"))
                : new SolidColorBrush(Color.Parse("#9B8AAD")),
            BorderThickness = pairedMode || challengeMode ? new Thickness(1.5) : new Thickness(2),
            CornerRadius = pairedMode || challengeMode ? new CornerRadius(2) : new CornerRadius(12),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = pairedMode
                ? new Thickness(0, 0, 12, 0)
                : challengeMode
                    ? new Thickness(12, 0, 0, 0)
                    : new Thickness(12, 0, 0, 0),
            Tag = rowIndex
        };

        dropBorder.Child = CreateDefaultDropPlaceholder();

        // Permitir soltar en la casilla
        DragDrop.SetAllowDrop(dropBorder, true);
        dropBorder.AddHandler(DragDrop.DropEvent, OnDrop);
        dropBorder.AddHandler(DragDrop.DragOverEvent, OnDragOver);

        // También permitir hacer tap en la casilla para quitar el número
        dropBorder.PointerPressed += (_, _) => OnDropTargetTapped(rowIndex);

        _dropTargets[rowIndex] = dropBorder;

        Grid.SetColumn(dropBorder, pairedMode ? 0 : 1);
        grid.Children.Add(dropBorder);

        rowBorder.Child = grid;
        return rowBorder;
    }

    private Control CreateObjectsPanel(MatchingRow row, bool pairedMode, bool challengeMode)
    {
        int imgW = row.ImageWidth ?? 60;
        int imgH = row.ImageHeight ?? 60;

        if (pairedMode && row.Count >= 3 && !string.IsNullOrEmpty(row.ImageUrl))
        {
            var (topCount, bottomCount) = GetStackedDistribution(row.Count);

            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = 2,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
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
                var img = CreateImage(row.ImageUrl, imgW, imgH);
                if (img != null) topRow.Children.Add(img);
            }

            for (int i = 0; i < bottomCount; i++)
            {
                var img = CreateImage(row.ImageUrl, imgW, imgH);
                if (img != null) bottomRow.Children.Add(img);
            }

            container.Children.Add(topRow);
            container.Children.Add(bottomRow);
            return container;
        }

        if (challengeMode)
        {
            var horizontal = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 6
            };

            for (int j = 0; j < row.Count; j++)
            {
                if (!string.IsNullOrEmpty(row.ImageUrl))
                {
                    var img = CreateImage(row.ImageUrl, imgW, imgH);
                    if (img != null)
                    {
                        horizontal.Children.Add(img);
                    }
                }
                else if (!string.IsNullOrEmpty(row.Emoji))
                {
                    horizontal.Children.Add(new TextBlock
                    {
                        Text = row.Emoji,
                        FontSize = 36,
                        VerticalAlignment = VerticalAlignment.Center
                    });
                }
            }

            return horizontal;
        }

        var wrap = new WrapPanel
        {
            HorizontalAlignment = pairedMode ? HorizontalAlignment.Center : HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center
        };

        for (int j = 0; j < row.Count; j++)
        {
            if (!string.IsNullOrEmpty(row.ImageUrl))
            {
                var img = CreateImage(row.ImageUrl, imgW, imgH);
                if (img != null)
                {
                    img.Margin = new Thickness(3);
                    wrap.Children.Add(img);
                }
            }
            else if (!string.IsNullOrEmpty(row.Emoji))
            {
                wrap.Children.Add(new TextBlock
                {
                    Text = row.Emoji,
                    FontSize = 36,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(3)
                });
            }
        }

        return wrap;
    }

    private static (int topCount, int bottomCount) GetStackedDistribution(int count)
    {
        return count switch
        {
            3 => (2, 1),
            4 => (2, 2),
            5 => (3, 2),
            6 => (3, 3),
            7 => (4, 3),
            8 => (4, 4),
            _ => ((int)Math.Ceiling(count / 2.0), count / 2)
        };
    }

    private Control CreateSequenceSlot(int rowIndex)
    {
        var wrapper = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 6
        };

        var dropBorder = new Border
        {
            Width = 54,
            Height = 54,
            BorderThickness = new Thickness(1.5),
            CornerRadius = new CornerRadius(2),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Tag = rowIndex
        };

        ApplyDefaultDropStyle(dropBorder);
        dropBorder.Child = CreateDefaultDropPlaceholder();

        DragDrop.SetAllowDrop(dropBorder, true);
        dropBorder.AddHandler(DragDrop.DropEvent, OnDrop);
        dropBorder.AddHandler(DragDrop.DragOverEvent, OnDragOver);
        dropBorder.PointerPressed += (_, _) => OnDropTargetTapped(rowIndex);

        _dropTargets[rowIndex] = dropBorder;

        var underline = new Border
        {
            Width = 32,
            Height = 2,
            Background = new SolidColorBrush(Color.Parse("#8E7AA6")),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        wrapper.Children.Add(dropBorder);
        wrapper.Children.Add(underline);
        return wrapper;
    }

    /// <summary>
    /// Crea un tile arrastrable con un número (usando imagen).
    /// </summary>
    private Border CreateNumberTile(int number)
    {
        int tileSize = ContentData?.NumberImageWidth ?? 70;
        int imgSize = (IsSequenceGridMode() || IsPairedCountsMode() || IsChallengeBoardMode()) ? tileSize : tileSize - 10;

        var tile = new Border
        {
            Width = tileSize,
            Height = ContentData?.NumberImageHeight ?? 70,
            Background = (IsSequenceGridMode() || IsPairedCountsMode() || IsChallengeBoardMode())
                ? Brushes.Transparent
                : new SolidColorBrush(Color.Parse("#B35AFF")),
            CornerRadius = (IsSequenceGridMode() || IsPairedCountsMode() || IsChallengeBoardMode()) ? new CornerRadius(0) : new CornerRadius(12),
            Cursor = new Cursor(StandardCursorType.Hand),
            Tag = number
        };

        // Intentar cargar imagen del número
        var numberImage = GetNumberImage(number, imgSize, imgSize);
        if (numberImage != null)
        {
            tile.Child = numberImage;
        }
        else
        {
            // Fallback a texto si no se encuentra la imagen
            tile.Child = new TextBlock
            {
                Text = number.ToString(),
                FontSize = 28,
                FontWeight = FontWeight.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            };
        }

        // Drag and drop
        tile.PointerPressed += OnNumberTilePointerPressed;

        return tile;
    }

    private Border CreateSymbolTile(string symbol)
    {
        var isNumericToken = symbol.All(char.IsDigit);
        var isLongTextToken = !isNumericToken && symbol.Length >= 5;

        var tile = new Border
        {
            Width = isNumericToken
                ? (symbol.Length > 1 ? 98 : 62)
                : (isLongTextToken ? Math.Min(230, Math.Max(148, 20 * symbol.Length)) : 62),
            Height = 62,
            Background = Brushes.Transparent,
            CornerRadius = new CornerRadius(0),
            Cursor = new Cursor(StandardCursorType.Hand),
            Padding = new Thickness(2),
            Tag = symbol
        };

        tile.Child = CreateSymbolContent(symbol, forDropTarget: false);

        if (ContentData?.SelectOnly != true)
        {
            tile.PointerPressed += OnNumberTilePointerPressed;
        }

        tile.PointerReleased += OnSymbolTilePointerReleased;
        return tile;
    }

    private void OnSymbolTilePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not Border tile) return;
        if (!IsOrderTokensMode()) return;
        if (!tile.IsVisible) return;
        if (tile.Tag is not string symbol) return;

        if (ContentData?.SelectOnly == true && _placedSymbols.Count > 0)
        {
            var rowIndex = _placedSymbols.Keys.Min();
            _placedSymbols[rowIndex] = symbol;

            if (_selectedOptionTile != null)
            {
                _selectedOptionTile.BorderThickness = new Thickness(0);
                _selectedOptionTile.BorderBrush = Brushes.Transparent;
            }

            tile.BorderThickness = new Thickness(2);
            tile.BorderBrush = new SolidColorBrush(Color.Parse("#65E4A3"));
            _selectedOptionTile = tile;

            ClearValidation();
            e.Handled = true;
            return;
        }

        // Modo opción múltiple: si hay una sola casilla, clic = seleccionar opción.
        if (_dropTargets.Count == 1)
        {
            var rowIndex = _dropTargets.Keys.Min();
            PlaceSymbolInRow(rowIndex, symbol, tile);
            e.Handled = true;
        }
    }

    private void PlaceSymbolInRow(int rowIndex, string symbol, Border? sourceTile)
    {
        if (!_placedSymbols.ContainsKey(rowIndex))
        {
            return;
        }

        if (!string.IsNullOrEmpty(_placedSymbols[rowIndex]))
        {
            ReturnSymbolToPool(_placedSymbols[rowIndex]!);
        }

        _placedSymbols[rowIndex] = symbol;

        if (_dropTargets.TryGetValue(rowIndex, out var dropTarget))
        {
            dropTarget.Child = CreateSymbolContent(symbol, forDropTarget: true);
            ApplyFilledDropStyle(dropTarget);
        }

        if (sourceTile != null)
        {
            sourceTile.IsVisible = false;
        }

        ClearValidation();
    }

    private Control CreateSymbolContent(string symbol, bool forDropTarget)
    {
        if (symbol.All(char.IsDigit))
        {
            var numericPanel = CreateNumericTokenPanel(
                symbol,
                forDropTarget ? 76 : 82,
                forDropTarget ? 44 : 50);

            if (numericPanel != null)
            {
                return numericPanel;
            }
        }

        var compositeTokenPanel = CreateCompositeTokenPanel(symbol, forDropTarget);
        if (compositeTokenPanel != null)
        {
            return compositeTokenPanel;
        }

        var symbolImage = GetSymbolImage(symbol, forDropTarget ? 30 : 36, forDropTarget ? 30 : 36);
        if (symbolImage != null)
        {
            return symbolImage;
        }

        var color = symbol switch
        {
            "<" => "#F29A2E",
            ">" => "#22B7E9",
            "=" => "#B5473B",
            _ => forDropTarget ? "#000000" : "#FFFFFF"
        };

        var useCompactText = symbol.Length >= 5;

        return new TextBlock
        {
            Text = symbol,
            FontSize = useCompactText ? (forDropTarget ? 22 : 24) : (forDropTarget ? 34 : 48),
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse(color)),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
        };
    }

    private Control? CreateNumericTokenPanel(string symbol, int totalWidth, int totalHeight)
    {
        if (string.IsNullOrWhiteSpace(symbol) || !symbol.All(char.IsDigit))
        {
            return null;
        }

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 2
        };

        var digitsCount = symbol.Length;
        var digitWidth = Math.Max(14, (totalWidth - ((digitsCount - 1) * 2)) / Math.Max(1, digitsCount));

        foreach (var digitChar in symbol)
        {
            var digit = digitChar - '0';
            var digitImage = GetNumberImage(digit, digitWidth, totalHeight);
            if (digitImage == null)
            {
                return null;
            }

            panel.Children.Add(digitImage);
        }

        return panel;
    }

    private Control? CreateCompositeTokenPanel(string symbol, bool forDropTarget)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return null;
        }

        var cleaned = symbol.Replace(" ", string.Empty);
        if (!cleaned.Any(char.IsDigit))
        {
            return null;
        }

        foreach (var c in cleaned)
        {
            if (!char.IsDigit(c) && c != '+' && c != '-' && c != '=')
            {
                return null;
            }
        }

        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 2
        };

        foreach (var c in cleaned)
        {
            if (char.IsDigit(c))
            {
                var digit = c - '0';
                var digitImage = GetNumberImage(digit, forDropTarget ? 16 : 22, forDropTarget ? 30 : 38);
                if (digitImage == null)
                {
                    return null;
                }

                panel.Children.Add(digitImage);
                continue;
            }

            var color = c switch
            {
                '+' => "#55C4E8",
                '-' => "#F29A2E",
                '=' => "#9B86D1",
                _ => "#FFFFFF"
            };

            panel.Children.Add(new TextBlock
            {
                Text = c.ToString(),
                FontSize = forDropTarget ? 22 : 30,
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Color.Parse(color)),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            });
        }

        return panel;
    }

    /// <summary>
    /// Obtiene la imagen de un número usando el patrón del JSON o ruta por defecto.
    /// </summary>
    private Image? GetNumberImage(int number, int width, int height)
    {
        var pattern = ContentData?.NumberImagePattern
            ?? "avares://Quibee/Assets/Images/{0}.png";

        var imageUrl = string.Format(pattern, number);
        return CreateImage(imageUrl, width, height);
    }

    private Image? GetSymbolImage(string symbol, int width, int height)
    {
        var imageUrl = symbol switch
        {
            "<" => "avares://Quibee/Assets/Images/LessThan.png",
            ">" => "avares://Quibee/Assets/Images/GreaterThan.png",
            "=" => "avares://Quibee/Assets/Images/Equal.png",
            _ => null
        };

        if (string.IsNullOrEmpty(imageUrl))
        {
            return null;
        }

        return CreateImage(imageUrl, width, height);
    }

    // ─── Drag & Drop ───

    private async void OnNumberTilePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border tile) return;

        // Si el tile ya fue colocado (está oculto), ignorar
        if (!tile.IsVisible) return;

        var dataObject = new DataObject();

        if (tile.Tag is int number)
        {
            dataObject.Set("Number", number);
        }
        else if (tile.Tag is string symbol)
        {
            dataObject.Set("Symbol", symbol);
        }
        else
        {
            return;
        }

        dataObject.Set("SourceTile", tile);

        // Iniciar drag
        var result = await DragDrop.DoDragDrop(e, dataObject, DragDropEffects.Move);

        // Si el drag no se completó, no hacer nada
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        e.DragEffects = (e.Data.Contains("Number") || e.Data.Contains("Symbol"))
            ? DragDropEffects.Move
            : DragDropEffects.None;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        if (sender is not Border dropTarget) return;
        if (dropTarget.Tag is not int rowIndex) return;
        var sourceTile = e.Data.Get("SourceTile") as Border;

        if (e.Data.Contains("Symbol") && _placedSymbols.ContainsKey(rowIndex))
        {
            var symbol = (string)e.Data.Get("Symbol")!;

            PlaceSymbolInRow(rowIndex, symbol, sourceTile);
            return;
        }

        if (!e.Data.Contains("Number")) return;
        var number = (int)e.Data.Get("Number")!;

        // Si ya hay un número en esta casilla, devolver el anterior
        if (_placedValues[rowIndex].HasValue)
        {
            ReturnNumberToPool(_placedValues[rowIndex]!.Value);
        }

        // Colocar el número
        _placedValues[rowIndex] = number;

        // Actualizar visual de la casilla con imagen del número
        int dropImgSize = IsSequenceGridMode()
            ? 40
            : IsPairedCountsMode()
                ? 30
                : IsChallengeBoardMode()
                    ? 34
                : (ContentData?.NumberImageWidth ?? 70) - 16;
        var numberImg = GetNumberImage(number, dropImgSize, dropImgSize);
        if (numberImg != null)
        {
            numberImg.HorizontalAlignment = HorizontalAlignment.Center;
            numberImg.VerticalAlignment = VerticalAlignment.Center;
            dropTarget.Child = numberImg;
        }
        else
        {
            // Fallback: texto
            dropTarget.Child = new TextBlock
            {
                Text = number.ToString(),
                FontSize = 28,
                FontWeight = FontWeight.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            };
        }

        ApplyFilledDropStyle(dropTarget);

        // Ocultar el tile fuente
        if (sourceTile != null)
        {
            sourceTile.IsVisible = false;
        }

        // Limpiar estado de validación
        ClearValidation();
    }

    /// <summary>
    /// Al tocar una casilla ocupada, devuelve el número al pool.
    /// </summary>
    private void OnDropTargetTapped(int rowIndex)
    {
        if (_placedSymbols.ContainsKey(rowIndex) && !string.IsNullOrEmpty(_placedSymbols[rowIndex]))
        {
            var symbolValue = _placedSymbols[rowIndex]!;
            _placedSymbols[rowIndex] = null;

            if (_dropTargets.TryGetValue(rowIndex, out var symbolDrop))
            {
                symbolDrop.Child = CreateDefaultDropPlaceholder();
                ApplyDefaultDropStyle(symbolDrop);
            }

            ReturnSymbolToPool(symbolValue);
            ClearValidation();
            return;
        }

        if (!_placedValues[rowIndex].HasValue) return;

        var value = _placedValues[rowIndex]!.Value;
        _placedValues[rowIndex] = null;

        // Restaurar visual de la casilla con "?"
        if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
        {
            dropBorder.Child = CreateDefaultDropPlaceholder();
            ApplyDefaultDropStyle(dropBorder);
        }

        // Devolver tile al pool
        ReturnNumberToPool(value);

        ClearValidation();
    }

    /// <summary>
    /// Hace visible de nuevo un tile con el valor dado.
    /// </summary>
    private void ReturnNumberToPool(int value)
    {
        // Buscar el primer tile oculto con ese número
        var tile = _numberTiles.FirstOrDefault(t =>
            !t.IsVisible && t.Tag is int n && n == value);

        if (tile != null)
        {
            tile.IsVisible = true;
        }
    }

    private void ReturnSymbolToPool(string value)
    {
        var tile = _symbolTiles.FirstOrDefault(t =>
            !t.IsVisible && t.Tag is string symbol && symbol == value);

        if (tile != null)
        {
            tile.IsVisible = true;
        }
    }

    // ─── Validación ───

    private void OnCheckButtonClick(object? sender, RoutedEventArgs e)
    {
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (validationMessage == null) return;

        if (_correctSymbolAnswers.Count > 0)
        {
            bool allFilledSymbols = _placedSymbols.All(kv => !string.IsNullOrEmpty(kv.Value));
            if (!allFilledSymbols)
            {
                validationMessage.Text = "Coloca todos los operadores";
                validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FFD166"));
                return;
            }

            if (IsOrderTokensMode() && ContentData?.AllowAnyOrder == true)
            {
                var expectedSymbols = _correctSymbolAnswers.Values
                    .Where(value => !string.IsNullOrEmpty(value))
                    .ToList();

                var placedSymbols = _placedSymbols.Values
                    .Where(value => !string.IsNullOrEmpty(value))
                    .Cast<string>()
                    .ToList();

                var expectedCounts = expectedSymbols
                    .GroupBy(value => value)
                    .ToDictionary(group => group.Key, group => group.Count());

                var placedCounts = placedSymbols
                    .GroupBy(value => value)
                    .ToDictionary(group => group.Key, group => group.Count());

                bool unorderedAllCorrectSymbols = expectedCounts.Count == placedCounts.Count
                    && expectedCounts.All(kv => placedCounts.TryGetValue(kv.Key, out var count) && count == kv.Value);

                int unorderedCorrectSymbols = expectedCounts.Sum(kv =>
                {
                    return placedCounts.TryGetValue(kv.Key, out var placedCount)
                        ? Math.Min(kv.Value, placedCount)
                        : 0;
                });

                foreach (var kv in _dropTargets)
                {
                    var rowIndex = kv.Key;
                    var dropBorder = kv.Value;
                    var placed = _placedSymbols.GetValueOrDefault(rowIndex);
                    var isExpected = !string.IsNullOrEmpty(placed) && expectedCounts.ContainsKey(placed);

                    if (unorderedAllCorrectSymbols || isExpected)
                    {
                        dropBorder.Background = new SolidColorBrush(Color.Parse("#2ECC71"));
                        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#27AE60"));
                    }
                    else
                    {
                        dropBorder.Background = new SolidColorBrush(Color.Parse("#E74C3C"));
                        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#C0392B"));
                    }
                }

                if (unorderedAllCorrectSymbols)
                {
                    validationMessage.Text = "¡Excelente! Respuestas correctas 🎉";
                    validationMessage.Foreground = new SolidColorBrush(Color.Parse("#65E4A3"));
                }
                else
                {
                    validationMessage.Text = $"{unorderedCorrectSymbols}/{_correctSymbolAnswers.Count} correctas. ¡Intenta de nuevo!";
                    validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FF8A8A"));
                }

                ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
                {
                    CorrectCount = unorderedCorrectSymbols,
                    TotalCount = _correctSymbolAnswers.Count,
                    PointsEarned = unorderedCorrectSymbols * 10,
                    SectionType = "practiquemos"
                });

                return;
            }

            bool allCorrectSymbols = true;
            int correctSymbols = 0;

            foreach (var kv in _correctSymbolAnswers)
            {
                var rowIndex = kv.Key;
                var expected = kv.Value;
                var placed = _placedSymbols.GetValueOrDefault(rowIndex);

                bool isCorrect = string.Equals(placed, expected, StringComparison.Ordinal);
                if (isCorrect)
                {
                    correctSymbols++;
                    if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
                    {
                        dropBorder.Background = new SolidColorBrush(Color.Parse("#2ECC71"));
                        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#27AE60"));
                    }
                }
                else
                {
                    allCorrectSymbols = false;
                    if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
                    {
                        dropBorder.Background = new SolidColorBrush(Color.Parse("#E74C3C"));
                        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#C0392B"));
                    }
                }
            }

            if (allCorrectSymbols)
            {
                validationMessage.Text = IsOrderTokensMode()
                    ? "¡Excelente! Todas las respuestas son correctas 🎉"
                    : "¡Excelente! Todos los operadores son correctos 🎉";
                validationMessage.Foreground = new SolidColorBrush(Color.Parse("#65E4A3"));
            }
            else
            {
                validationMessage.Text = $"{correctSymbols}/{_correctSymbolAnswers.Count} correctas. ¡Intenta de nuevo!";
                validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FF8A8A"));
            }

            ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
            {
                CorrectCount = correctSymbols,
                TotalCount = _correctSymbolAnswers.Count,
                PointsEarned = correctSymbols * 10,
                SectionType = "practiquemos"
            });

            return;
        }

        // Verificar si todas las casillas están llenas
        bool allFilled = _placedValues.All(kv => kv.Value.HasValue);
        if (!allFilled)
        {
            validationMessage.Text = "Coloca todos los números";
            validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FFD166"));
            return;
        }

        // Verificar respuestas
        bool allCorrect = true;
        int correctCount = 0;

        foreach (var kv in _correctAnswers)
        {
            int rowIndex = kv.Key;
            int expected = kv.Value;
            int? placed = _placedValues.GetValueOrDefault(rowIndex);

            bool isCorrect = placed.HasValue && placed.Value == expected;

            if (isCorrect)
            {
                correctCount++;
                // Marcar casilla en verde
                if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
                {
                    dropBorder.Background = new SolidColorBrush(Color.Parse("#2ECC71"));
                    dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#27AE60"));
                }
            }
            else
            {
                allCorrect = false;
                // Marcar casilla en rojo
                if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
                {
                    dropBorder.Background = new SolidColorBrush(Color.Parse("#E74C3C"));
                    dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#C0392B"));
                }
            }
        }

        if (allCorrect)
        {
            validationMessage.Text = "¡Excelente! Todas las respuestas son correctas 🎉";
            validationMessage.Foreground = new SolidColorBrush(Color.Parse("#65E4A3"));
        }
        else
        {
            validationMessage.Text = $"{correctCount}/{_correctAnswers.Count} correctas. ¡Intenta de nuevo!";
            validationMessage.Foreground = new SolidColorBrush(Color.Parse("#FF8A8A"));
        }

        // Notificar al ViewModel
        int points = correctCount * 10;
        ExerciseMessenger.NotifyExerciseCompleted(new ExerciseCompletedEventArgs
        {
            CorrectCount = correctCount,
            TotalCount = _correctAnswers.Count,
            PointsEarned = points,
            SectionType = "desafio"
        });
    }

    private void ClearValidation()
    {
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (validationMessage != null)
        {
            validationMessage.Text = string.Empty;
        }

        // Restaurar colores de casillas que no están vacías
        foreach (var kv in _dropTargets)
        {
            if (_placedValues.GetValueOrDefault(kv.Key).HasValue || !string.IsNullOrEmpty(_placedSymbols.GetValueOrDefault(kv.Key)))
            {
                ApplyFilledDropStyle(kv.Value);
            }
        }
    }

    private Control CreateDefaultDropPlaceholder()
    {
        if (IsSequenceGridMode() || IsPairedCountsMode() || IsChallengeBoardMode())
        {
            return new TextBlock
            {
                Text = string.Empty,
                Width = 1,
                Height = 1
            };
        }

        if (IsOperatorCompareMode())
        {
            return new TextBlock
            {
                Text = string.Empty,
                Width = 1,
                Height = 1
            };
        }

        return new TextBlock
        {
            Text = "?",
            FontSize = 28,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#C0B0D0")),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
        };
    }

    private void ApplyDefaultDropStyle(Border dropBorder)
    {
        if (IsSequenceGridMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#EDEDED"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D"));
            return;
        }

        if (IsPairedCountsMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#EDEDED"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D"));
            return;
        }

        if (IsChallengeBoardMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#EDEDED"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D"));
            return;
        }

        if (IsOperatorCompareMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#EDEDED"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#8D8D8D"));
            return;
        }

        dropBorder.Background = new SolidColorBrush(Color.Parse("#7A678A"));
        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#9B8AAD"));
    }

    private void ApplyFilledDropStyle(Border dropBorder)
    {
        if (IsSequenceGridMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#F8F8F8"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#666666"));
            return;
        }

        if (IsPairedCountsMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#F8F8F8"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#666666"));
            return;
        }

        if (IsChallengeBoardMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#F8F8F8"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#666666"));
            return;
        }

        if (IsOperatorCompareMode())
        {
            dropBorder.Background = new SolidColorBrush(Color.Parse("#F8F8F8"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#666666"));
            return;
        }

        dropBorder.Background = new SolidColorBrush(Color.Parse("#9B6FD0"));
        dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#B35AFF"));
    }

    // ─── Helpers ───

    private Image? CreateImage(string imageUrl, int width, int height)
    {
        try
        {
            var uri = new Uri(imageUrl);
            var bitmap = new Bitmap(Avalonia.Platform.AssetLoader.Open(uri));
            return new Image
            {
                Source = bitmap,
                Width = width,
                Height = height,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        catch
        {
            return null;
        }
    }

    private static List<T> ShuffleList<T>(List<T> list)
    {
        var rng = new Random();
        var shuffled = new List<T>(list);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled;
    }
}
