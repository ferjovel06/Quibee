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
        var answers = ContentData.Rows.Select(r => r.CorrectAnswer).ToList();
        var shuffled = ShuffleList(answers);

        foreach (var number in shuffled)
        {
            var tile = CreateNumberTile(number);
            numbersContainer.Children.Add(tile);
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

        rowsContainer.Children.Add(new TextBlock
        {
            Text = "Ordena tu respuesta:",
            FontSize = 20,
            FontWeight = FontWeight.SemiBold,
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center
        });

        var slotsRow = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        if (ContentData?.Rows != null)
        {
            for (int i = 0; i < ContentData.Rows.Count; i++)
            {
                var row = ContentData.Rows[i];
                _placedSymbols[i] = null;
                _correctSymbolAnswers[i] = row.CorrectSymbolAnswer ?? string.Empty;
                slotsRow.Children.Add(CreateOrderTokenSlot(i));
            }
        }

        rowsContainer.Children.Add(slotsRow);

        numbersContainer.Children.Clear();

        var options = ContentData?.Symbols != null && ContentData.Symbols.Count > 0
            ? ContentData.Symbols
            : new List<string> { "3", "5", "<", ">" };

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

        if (validationContainer != null)
        {
            validationContainer.IsVisible = true;
        }
    }

    private Border CreateOrderTokenSlot(int rowIndex)
    {
        var dropBorder = new Border
        {
            Width = 56,
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
        var tile = new Border
        {
            Width = 54,
            Height = 54,
            Background = Brushes.Transparent,
            CornerRadius = new CornerRadius(0),
            Cursor = new Cursor(StandardCursorType.Hand),
            Tag = symbol
        };

        if (int.TryParse(symbol, out var numericToken))
        {
            var numberImage = GetNumberImage(numericToken, 38, 54);
            if (numberImage != null)
            {
                tile.Child = numberImage;
            }
            else
            {
                tile.Child = new TextBlock
                {
                    Text = symbol,
                    FontSize = 44,
                    FontWeight = FontWeight.Bold,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
                };
            }
        }
        else
        {
            var color = symbol switch
            {
                "<" => "#F29A2E",
                ">" => "#22B7E9",
                "=" => "#B5473B",
                _ => "#FFFFFF"
            };

            tile.Child = new TextBlock
            {
                Text = symbol,
                FontSize = 48,
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Color.Parse(color)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            };
        }

        tile.PointerPressed += OnNumberTilePointerPressed;
        return tile;
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

            if (!string.IsNullOrEmpty(_placedSymbols[rowIndex]))
            {
                ReturnSymbolToPool(_placedSymbols[rowIndex]!);
            }

            _placedSymbols[rowIndex] = symbol;

            dropTarget.Child = new TextBlock
            {
                Text = symbol,
                FontSize = 34,
                FontWeight = FontWeight.Bold,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            };

            ApplyFilledDropStyle(dropTarget);

            if (sourceTile != null)
            {
                sourceTile.IsVisible = false;
            }

            ClearValidation();
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
                validationMessage.Text = "¡Excelente! Todos los operadores son correctos 🎉";
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
