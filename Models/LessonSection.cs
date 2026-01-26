namespace Quibee.Models;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

/// <summary>
/// Representa una sección de una lección (Introducción, Analicemos, Ejercitemos, etc.)
/// </summary>
public class LessonSection : INotifyPropertyChanged
{
    private string _iconPathLight = string.Empty;
    private string _iconPathDark = string.Empty;
    private bool _isSelected;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Nombre de la sección (ej: "Introducción", "Analicemos")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Icono Light de la sección (usado cuando está seleccionada)
    /// </summary>
    public string IconPathLight 
    { 
        get => _iconPathLight;
        set
        {
            _iconPathLight = value;
            LoadIcon();
        }
    }

    /// <summary>
    /// Icono Dark de la sección (usado cuando NO está seleccionada)
    /// </summary>
    public string IconPathDark 
    { 
        get => _iconPathDark;
        set
        {
            _iconPathDark = value;
            LoadIcon();
        }
    }

    /// <summary>
    /// Imagen bitmap del icono actual (cambia según IsSelected)
    /// </summary>
    public Bitmap? Icon { get; private set; }

    /// <summary>
    /// Contenido de texto de la sección
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Indica si esta sección está actualmente seleccionada
    /// </summary>
    public bool IsSelected 
    { 
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                LoadIcon(); // Cambia el icono según el estado
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Carga el icono correcto según el estado de selección
    /// </summary>
    private void LoadIcon()
    {
        string path = IsSelected ? IconPathLight : IconPathDark;
        
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                var uri = new Uri(path);
                Icon = new Bitmap(AssetLoader.Open(uri));
                OnPropertyChanged(nameof(Icon));
            }
            catch
            {
                // Si falla, Icon quedará null
                Icon = null;
            }
        }
    }

    /// <summary>
    /// Color de fondo cuando está seleccionada
    /// </summary>
    public string SelectedColor { get; set; } = "#00D9FF";

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
