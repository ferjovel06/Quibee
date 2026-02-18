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
    private LessonSection? _currentSection;
    private ObservableCollection<LessonContent> _currentSectionContents;

    public GenericLessonViewModel(
        MainWindowViewModel? mainWindowViewModel,
        LessonData lessonData)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _lessonData = lessonData;
        _contentService = ServiceLocator.GetLessonContentService();
        _currentSectionContents = new ObservableCollection<LessonContent>();

        // Inicializar las secciones como ObservableCollection
        Sections = new ObservableCollection<LessonSection>(lessonData.Sections);
        
        // Cargar contenido dinámico
        _ = LoadLessonContentAsync();

        // Comandos
        VolverCommand = new RelayCommand(OnVolver);
        SectionClickCommand = new RelayCommand(param => OnSectionClick(param));
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
                            Content = "", // No usaremos este campo
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
                "avares://Quibee/Assets/Images/IntroduccionIconLight.png",
                "avares://Quibee/Assets/Images/IntroduccionIconDark.png"
            ),
            "analicemos" => (
                "avares://Quibee/Assets/Images/AnalicemosIconLight.png",
                "avares://Quibee/Assets/Images/AnalicemosIconDark.png"
            ),
            "ejercitemos" or "practiquemos" => (
                "avares://Quibee/Assets/Images/EjercitemosIconLight.png",
                "avares://Quibee/Assets/Images/EjercitemosIconDark.png"
            ),
            "resolvamos" => (
                "avares://Quibee/Assets/Images/ResolvamosIconLight.png",
                "avares://Quibee/Assets/Images/ResolvamosIconDark.png"
            ),
            _ => (
                "avares://Quibee/Assets/Images/DefaultIconLight.png",
                "avares://Quibee/Assets/Images/DefaultIconDark.png"
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
        
        return char.ToUpper(sectionKey[0]) + sectionKey.Substring(1).ToLower();
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

    // Handlers
    private void OnVolver()
    {
        // Volver al mapa de lecciones
        _mainWindowViewModel?.NavigateToLessonsMap(_lessonData.StudentId, _lessonData.LevelNumber);
    }

    private void OnSectionClick(object? parameter)
    {
        if (parameter is LessonSection section)
        {
            CurrentSection = section;
            
            // Cargar contenido de la nueva sección
            var sectionKey = section.Name.ToLower();
            _ = LoadSectionContentAsync(sectionKey);
        }
    }
}
