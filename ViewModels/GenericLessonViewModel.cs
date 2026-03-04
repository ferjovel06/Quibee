using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using Quibee.Models;
using Quibee.Services;

namespace Quibee.ViewModels;

/// <summary>
/// ViewModel genérico y reutilizable para cualquier lección
/// </summary>
public class GenericLessonViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly LessonData _lessonData;
    private readonly LessonContentService _contentService;
    private readonly LessonProgressService _progressService;
    private LessonSection? _currentSection;
    private ObservableCollection<LessonContent> _currentSectionContents;

    // Popup de progreso
    private bool _showProgressPopup;
    private string _popupTitle = "";
    private string _popupMessage = "";
    private string _popupPoints = "";
    private bool _popupIsSuccess;

    public GenericLessonViewModel(
        MainWindowViewModel? mainWindowViewModel,
        LessonData lessonData)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _lessonData = lessonData;
        _contentService = ServiceLocator.GetLessonContentService();
        _progressService = ServiceLocator.GetLessonProgressService();
        _currentSectionContents = new ObservableCollection<LessonContent>();

        // Inicializar las secciones como ObservableCollection
        Sections = new ObservableCollection<LessonSection>(lessonData.Sections);
        
        // Cargar contenido dinámico
        _ = LoadLessonContentAsync();

        // Comandos
        VolverCommand = new RelayCommand(OnVolver);
        SectionClickCommand = new RelayCommand(param => OnSectionClick(param));
        ClosePopupCommand = new RelayCommand(_ => ShowProgressPopup = false);

        // Escuchar eventos de ejercicios completados
        ExerciseMessenger.ExerciseCompleted += OnExerciseCompleted;
    }

    /// <summary>
    /// Carga el contenido de la lección desde la base de datos
    /// </summary>
    private async Task LoadLessonContentAsync()
    {
        try
        {
            var contents = await _contentService.GetLessonContentAsync(_lessonData.LessonNumber);

            if (contents != null && contents.Count > 0)
            {
                // Obtener las secciones únicas del contenido
                var sections = await _contentService.GetLessonSectionsAsync(_lessonData.LessonNumber);

                if (sections != null && sections.Count > 0)
                {
                    // Reemplazar las secciones estáticas con las dinámicas
                    Sections.Clear();
                    foreach (var sectionName in sections)
                    {
                        // Mapear nombres de secciones a iconos
                        var (lightIcon, darkIcon) = GetSectionIcons(sectionName);
                        
                        Sections.Add(new LessonSection
                        {
                            Name = CapitalizeSectionName(sectionName),
                            Content = sectionName, // Clave real de la sección en BD
                            IconPathLight = lightIcon,
                            IconPathDark = darkIcon,
                            IsSelected = false
                        });
                    }
                    
                    // Seleccionar la primera sección
                    if (Sections.Count > 0)
                    {
                        CurrentSection = Sections[0];
                        CurrentSection.IsSelected = true;
                        await LoadSectionContentAsync(sections[0]);
                        
                        // Registrar visita a la primera sección
                        try
                        {
                            await _progressService.RecordSectionVisitAsync(
                                _lessonData.StudentId,
                                _lessonData.LessonNumber,
                                sections[0],
                                Sections.Count);
                        }
                        catch (Exception)
                        {
                            // Error silencioso
                        }
                    }
                }
            }
        }
        catch (System.Exception)
        {
            // Error silencioso: la vista queda sin contenido si falla la carga.
        }
    }

    /// <summary>
    /// Carga el contenido de una sección específica
    /// </summary>
    private async Task LoadSectionContentAsync(string sectionKey)
    {
        try
        {
            var contents = await _contentService.GetLessonContentBySectionAsync(_lessonData.LessonNumber, sectionKey);

            CurrentSectionContents.Clear();
            if (contents != null)
            {
                foreach (var content in contents)
                {
                    CurrentSectionContents.Add(content);
                }
            }
        }
        catch (System.Exception)
        {
            // Error silencioso al cargar sección.
        }
    }

    /// <summary>
    /// Mapea nombres de secciones a rutas de iconos (light y dark)
    /// </summary>
    private (string light, string dark) GetSectionIcons(string sectionKey)
    {
        return sectionKey.ToLower() switch
        {
            "introduccion" => (
                "avares://Quibee/Assets/Images/Icons/BulbLight.png",
                "avares://Quibee/Assets/Images/Icons/BulbDark.png"
            ),
            var key when key.StartsWith("analicemos") => (
                "avares://Quibee/Assets/Images/Icons/BrainLight.png",
                "avares://Quibee/Assets/Images/Icons/BrainDark.png"
            ),
            "ejercitemos" or "practiquemos" => (
                "avares://Quibee/Assets/Images/Icons/BookLight.png",
                "avares://Quibee/Assets/Images/Icons/BookDark.png"
            ),
            "resolvamos" => (
                "avares://Quibee/Assets/Images/Icons/PencilLight.png",
                "avares://Quibee/Assets/Images/Icons/PencilDark.png"
            ),
            "desafio" => (
                "avares://Quibee/Assets/Images/Icons/FlagLight.png",
                "avares://Quibee/Assets/Images/Icons/FlagDark.png"
            ),
            _ => (
                "avares://Quibee/Assets/Images/Icons/StarLight.png",
                "avares://Quibee/Assets/Images/Icons/StarLight.png"
            )
        };
    }

    /// <summary>
    /// Capitaliza el nombre de la sección para mostrar
    /// </summary>
    private string CapitalizeSectionName(string sectionKey)
    {
        if (string.IsNullOrEmpty(sectionKey))
            return "";

        var normalized = sectionKey.Replace('_', ' ').ToLower();
        return char.ToUpper(normalized[0]) + normalized.Substring(1);
    }

    // Propiedades de la lección
    public string LessonTitle => _lessonData.Title;
    public int LessonNumber => _lessonData.LessonNumber;
    public ObservableCollection<LessonSection> Sections { get; }

    /// <summary>
    /// Colección de contenidos de la sección actual
    /// </summary>
    public ObservableCollection<LessonContent> CurrentSectionContents
    {
        get => _currentSectionContents;
        set
        {
            _currentSectionContents = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Sección actualmente seleccionada
    /// </summary>
    public LessonSection? CurrentSection
    {
        get => _currentSection;
        set
        {
            if (_currentSection != value)
            {
                // Deseleccionar la anterior
                if (_currentSection != null)
                {
                    _currentSection.IsSelected = false;
                }

                _currentSection = value;
                
                // Seleccionar la nueva
                if (_currentSection != null)
                {
                    _currentSection.IsSelected = true;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentSectionContent));
            }
        }
    }

    /// <summary>
    /// Contenido de la sección actual
    /// </summary>
    public string CurrentSectionContent => CurrentSection?.Content ?? "";

    // Comandos
    public ICommand VolverCommand { get; }
    public ICommand SectionClickCommand { get; }
    public ICommand ClosePopupCommand { get; }

    // ─── Propiedades del Popup de progreso ───

    public bool ShowProgressPopup
    {
        get => _showProgressPopup;
        set { _showProgressPopup = value; OnPropertyChanged(); }
    }

    public string PopupTitle
    {
        get => _popupTitle;
        set { _popupTitle = value; OnPropertyChanged(); }
    }

    public string PopupMessage
    {
        get => _popupMessage;
        set { _popupMessage = value; OnPropertyChanged(); }
    }

    public string PopupPoints
    {
        get => _popupPoints;
        set { _popupPoints = value; OnPropertyChanged(); }
    }

    public bool PopupIsSuccess
    {
        get => _popupIsSuccess;
        set { _popupIsSuccess = value; OnPropertyChanged(); }
    }

    // Handlers
    private void OnVolver()
    {
        Cleanup();
        // Volver al mapa de lecciones
        _mainWindowViewModel?.NavigateToLessonsMap(_lessonData.StudentId, _lessonData.LevelNumber);
    }

    private void OnSectionClick(object? parameter)
    {
        if (parameter is LessonSection section)
        {
            CurrentSection = section;
            
            // Cargar contenido de la nueva sección y registrar la visita
            var sectionKey = string.IsNullOrWhiteSpace(section.Content)
                ? section.Name.ToLower().Replace(' ', '_')
                : section.Content;
            _ = LoadSectionContentAndTrackAsync(sectionKey);
        }
    }

    /// <summary>
    /// Carga el contenido de la sección y registra la visita para el progreso.
    /// </summary>
    private async Task LoadSectionContentAndTrackAsync(string sectionKey)
    {
        await LoadSectionContentAsync(sectionKey);
        
        try
        {
            await _progressService.RecordSectionVisitAsync(
                _lessonData.StudentId,
                _lessonData.LessonNumber,
                sectionKey,
                Sections.Count);
        }
        catch (Exception)
        {
            // Error silencioso: no bloquear la navegación si falla el tracking
        }
    }

    // ─── Progreso y Popup ───

    private async void OnExerciseCompleted(object? sender, ExerciseCompletedEventArgs e)
    {
        // Registrar la sección del ejercicio como visitada
        try
        {
            await _progressService.RecordSectionVisitAsync(
                _lessonData.StudentId,
                _lessonData.LessonNumber,
                e.SectionType,
                Sections.Count);
        }
        catch (Exception)
        {
            // Error silencioso
        }

        // Mostrar popup en el hilo de UI
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (e.AllCorrect)
            {
                PopupIsSuccess = true;
                PopupTitle = "🎉 ¡Excelente trabajo!";
                PopupMessage = $"Respondiste correctamente {e.CorrectCount}/{e.TotalCount}";
                PopupPoints = $"+{e.PointsEarned} puntos";
            }
            else
            {
                PopupIsSuccess = false;
                PopupTitle = "💪 ¡Sigue intentando!";
                PopupMessage = $"Respondiste {e.CorrectCount}/{e.TotalCount} correctamente";
                PopupPoints = e.PointsEarned > 0 ? $"+{e.PointsEarned} puntos" : "";
            }

            ShowProgressPopup = true;
        });
    }

    /// <summary>
    /// Limpieza al destruir el ViewModel.
    /// </summary>
    public void Cleanup()
    {
        ExerciseMessenger.ExerciseCompleted -= OnExerciseCompleted;
    }
}
