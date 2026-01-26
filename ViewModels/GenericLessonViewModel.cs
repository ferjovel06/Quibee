using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Quibee.Models;

namespace Quibee.ViewModels;

/// <summary>
/// ViewModel genérico y reutilizable para cualquier lección
/// </summary>
public class GenericLessonViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly LessonData _lessonData;
    private LessonSection? _currentSection;

    public GenericLessonViewModel(
        MainWindowViewModel? mainWindowViewModel,
        LessonData lessonData)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _lessonData = lessonData;

        // Inicializar las secciones como ObservableCollection
        Sections = new ObservableCollection<LessonSection>(lessonData.Sections);
        
        // Seleccionar la primera sección por defecto
        if (Sections.Count > 0)
        {
            CurrentSection = Sections[0];
            CurrentSection.IsSelected = true;
        }

        // Comandos
        VolverCommand = new RelayCommand(OnVolver);
        SectionClickCommand = new RelayCommand(param => OnSectionClick(param));
    }

    // Propiedades de la lección
    public string LessonTitle => _lessonData.Title;
    public int LessonNumber => _lessonData.LessonNumber;
    public ObservableCollection<LessonSection> Sections { get; }

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
        _mainWindowViewModel?.NavigateToLessonsMap(_lessonData.StudentId, _lessonData.GradeLevel);
    }

    private void OnSectionClick(object? parameter)
    {
        if (parameter is LessonSection section)
        {
            CurrentSection = section;
        }
    }
}
