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

    /// <summary>
    /// Referencia a los tiles de números para poder devolverlos.
    /// </summary>
    private readonly List<Border> _numberTiles = new();

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
        var instructionText = this.FindControl<TextBlock>("InstructionText");
        var validationContainer = this.FindControl<StackPanel>("ValidationContainer");

        if (rowsContainer == null || numbersContainer == null) return;

        rowsContainer.Children.Clear();
        numbersContainer.Children.Clear();
        _dropTargets.Clear();
        _placedValues.Clear();
        _correctAnswers.Clear();
        _numberTiles.Clear();

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

    /// <summary>
    /// Crea una fila con objetos repetidos + casilla de drop.
    /// </summary>
    private Border CreateRow(int rowIndex, MatchingRow row, string bgColor)
    {
        var rowBorder = new Border
        {
            Background = new SolidColorBrush(Color.Parse(bgColor)),
            CornerRadius = new CornerRadius(16),
            Padding = new Thickness(20, 14),
            Margin = new Thickness(0)
        };

        var grid = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse("*,Auto")
        };

        // Panel de objetos
        var objectsPanel = new WrapPanel
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center
        };

        int imgW = row.ImageWidth ?? 60;
        int imgH = row.ImageHeight ?? 60;

        for (int j = 0; j < row.Count; j++)
        {
            if (!string.IsNullOrEmpty(row.ImageUrl))
            {
                var img = CreateImage(row.ImageUrl, imgW, imgH);
                if (img != null)
                {
                    img.Margin = new Thickness(3);
                    objectsPanel.Children.Add(img);
                }
            }
            else if (!string.IsNullOrEmpty(row.Emoji))
            {
                objectsPanel.Children.Add(new TextBlock
                {
                    Text = row.Emoji,
                    FontSize = 36,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(3)
                });
            }
        }

        Grid.SetColumn(objectsPanel, 0);
        grid.Children.Add(objectsPanel);

        // Casilla de drop (donde se suelta el número)
        int dropSize = ContentData?.NumberImageWidth ?? 70;
        var dropBorder = new Border
        {
            Width = dropSize,
            Height = ContentData?.NumberImageHeight ?? 70,
            Background = new SolidColorBrush(Color.Parse("#7A678A")),
            BorderBrush = new SolidColorBrush(Color.Parse("#9B8AAD")),
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(12),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(12, 0, 0, 0),
            Tag = rowIndex
        };

        var dropText = new TextBlock
        {
            Text = "?",
            FontSize = 28,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#C0B0D0")),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
        };

        dropBorder.Child = dropText;

        // Permitir soltar en la casilla
        DragDrop.SetAllowDrop(dropBorder, true);
        dropBorder.AddHandler(DragDrop.DropEvent, OnDrop);
        dropBorder.AddHandler(DragDrop.DragOverEvent, OnDragOver);

        // También permitir hacer tap en la casilla para quitar el número
        dropBorder.PointerPressed += (_, _) => OnDropTargetTapped(rowIndex);

        _dropTargets[rowIndex] = dropBorder;

        Grid.SetColumn(dropBorder, 1);
        grid.Children.Add(dropBorder);

        rowBorder.Child = grid;
        return rowBorder;
    }

    /// <summary>
    /// Crea un tile arrastrable con un número (usando imagen).
    /// </summary>
    private Border CreateNumberTile(int number)
    {
        int tileSize = ContentData?.NumberImageWidth ?? 70;
        int imgSize = tileSize - 10; // Imagen un poco menor que el borde

        var tile = new Border
        {
            Width = tileSize,
            Height = ContentData?.NumberImageHeight ?? 70,
            Background = new SolidColorBrush(Color.Parse("#B35AFF")),
            CornerRadius = new CornerRadius(12),
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

    // ─── Drag & Drop ───

    private async void OnNumberTilePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border tile || tile.Tag is not int number) return;

        // Si el tile ya fue colocado (está oculto), ignorar
        if (!tile.IsVisible) return;

        var dataObject = new DataObject();
        dataObject.Set("Number", number);
        dataObject.Set("SourceTile", tile);

        // Iniciar drag
        var result = await DragDrop.DoDragDrop(e, dataObject, DragDropEffects.Move);

        // Si el drag no se completó, no hacer nada
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        e.DragEffects = e.Data.Contains("Number")
            ? DragDropEffects.Move
            : DragDropEffects.None;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        if (sender is not Border dropTarget) return;
        if (dropTarget.Tag is not int rowIndex) return;
        if (!e.Data.Contains("Number")) return;

        var number = (int)e.Data.Get("Number")!;
        var sourceTile = e.Data.Get("SourceTile") as Border;

        // Si ya hay un número en esta casilla, devolver el anterior
        if (_placedValues[rowIndex].HasValue)
        {
            ReturnNumberToPool(_placedValues[rowIndex]!.Value);
        }

        // Colocar el número
        _placedValues[rowIndex] = number;

        // Actualizar visual de la casilla con imagen del número
        int dropImgSize = (ContentData?.NumberImageWidth ?? 70) - 16;
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

        dropTarget.Background = new SolidColorBrush(Color.Parse("#9B6FD0"));
        dropTarget.BorderBrush = new SolidColorBrush(Color.Parse("#B35AFF"));

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
        if (!_placedValues[rowIndex].HasValue) return;

        var value = _placedValues[rowIndex]!.Value;
        _placedValues[rowIndex] = null;

        // Restaurar visual de la casilla con "?"
        if (_dropTargets.TryGetValue(rowIndex, out var dropBorder))
        {
            dropBorder.Child = new TextBlock
            {
                Text = "?",
                FontSize = 28,
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Color.Parse("#C0B0D0")),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FontFamily.Parse("avares://Quibee/Assets/Fonts/Poppins-SemiBold.ttf#Poppins")
            };
            dropBorder.Background = new SolidColorBrush(Color.Parse("#7A678A"));
            dropBorder.BorderBrush = new SolidColorBrush(Color.Parse("#9B8AAD"));
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

    // ─── Validación ───

    private void OnCheckButtonClick(object? sender, RoutedEventArgs e)
    {
        var validationMessage = this.FindControl<TextBlock>("ValidationMessage");
        if (validationMessage == null) return;

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
            if (_placedValues.GetValueOrDefault(kv.Key).HasValue)
            {
                kv.Value.Background = new SolidColorBrush(Color.Parse("#9B6FD0"));
                kv.Value.BorderBrush = new SolidColorBrush(Color.Parse("#B35AFF"));
            }
        }
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
