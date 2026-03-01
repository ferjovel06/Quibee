using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Quibee.Models;
using Quibee.Services;

namespace Quibee.ViewModels;

public class LessonsMapViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly int _studentId;
    private readonly int _levelNumber;
    private readonly TopicService _topicService;
    
    // Cache para búsqueda rápida de nodos por ID (O(1) en lugar de O(n))
    // Crítico con ~30 nodos por nivel
    private readonly Dictionary<string, ThemeData> _nodeCache = new();

    public LessonsMapViewModel(
        MainWindowViewModel? mainWindowViewModel,
        int studentId,
        int levelNumber,
        TopicService topicService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _studentId = studentId;
        _levelNumber = levelNumber;
        _topicService = topicService;

        // Inicializar colecciones
        Nodes = new ObservableCollection<ThemeData>();
        Edges = new ObservableCollection<TemaEdge>();

        // Navegación
        InicioCommand = new RelayCommand(OnInicio);
        ManualCommand = new RelayCommand(OnManual);
        UsuarioCommand = new RelayCommand(OnUsuario);

        // Cargar temas desde BD de forma asíncrona
        _ = LoadThemesFromDatabaseAsync();
    }

    // ===== NUEVAS COLECCIONES DATA-DRIVEN =====
    /// <summary>
    /// Colección de nodos (temas) para renderizado dinámico con ItemsControl
    /// </summary>
    public ObservableCollection<ThemeData> Nodes { get; }

    /// <summary>
    /// Colección de conexiones entre nodos para renderizado dinámico
    /// </summary>
    public ObservableCollection<TemaEdge> Edges { get; }

    /// <summary>
    /// Alto del canvas en base a la posición y tamaño de los nodos
    /// </summary>
    public double CanvasHeight { get; private set; } = 700;

    private string _tituloLecciones = "NIVEL";
    public string TituloLecciones
    {
        get => _tituloLecciones;
        private set
        {
            _tituloLecciones = value;
            OnPropertyChanged(nameof(TituloLecciones));
        }
    }

    private string _levelDescription = string.Empty;
    public string LevelDescription
    {
        get => _levelDescription;
        private set
        {
            _levelDescription = value;
            OnPropertyChanged(nameof(LevelDescription));
        }
    }

    private string _featuredTopicTitle = string.Empty;
    public string FeaturedTopicTitle
    {
        get => _featuredTopicTitle;
        private set
        {
            _featuredTopicTitle = value;
            OnPropertyChanged(nameof(FeaturedTopicTitle));
        }
    }

    private string _featuredTopicDescription = string.Empty;
    public string FeaturedTopicDescription
    {
        get => _featuredTopicDescription;
        private set
        {
            _featuredTopicDescription = value;
            OnPropertyChanged(nameof(FeaturedTopicDescription));
        }
    }

    private int _featuredProgressPercentage;
    public int FeaturedProgressPercentage
    {
        get => _featuredProgressPercentage;
        private set
        {
            _featuredProgressPercentage = Math.Clamp(value, 0, 100);
            OnPropertyChanged(nameof(FeaturedProgressPercentage));
            OnPropertyChanged(nameof(FeaturedProgressWidth));
        }
    }

    public double FeaturedProgressWidth => 240 * (FeaturedProgressPercentage / 100.0);

    // Comandos de navegación
    public ICommand InicioCommand { get; }
    public ICommand ManualCommand { get; }
    public ICommand UsuarioCommand { get; }

    /// <summary>
    /// Carga los temas desde la base de datos y los mapea a ThemeData con posiciones
    /// </summary>
    private async Task LoadThemesFromDatabaseAsync()
    {
        try
        {
            // Cargar temas desde BD
            var topics = await _topicService.GetTopicsByLevelAsync(_levelNumber);
            var progressPercentage = await _topicService.GetStudentLevelProgressPercentageAsync(_studentId, _levelNumber);
            var firstLessonIds = topics
                .Select(t => t.Lessons
                    .Where(l => l.IsActive)
                    .OrderBy(l => l.OrderIndex)
                    .Select(l => l.IdLesson)
                    .FirstOrDefault())
                .Where(id => id > 0)
                .Distinct()
                .ToList();
            var lessonProgressMap = await _topicService
                .GetStudentLessonProgressByLessonIdsAsync(_studentId, firstLessonIds);

            // Mapear a ThemeData con posiciones según el nivel
            var themesData = topics
                .Select(topic => MapTopicToThemeData(topic, topic.OrderIndex, lessonProgressMap))
                .ToList();

            // ===== Actualizar colección NODES (nueva arquitectura) =====
            Nodes.Clear();
            _nodeCache.Clear(); // Limpiar cache antes de repoblar
            
            foreach (var theme in themesData)
            {
                // Asignar comando al nodo
                theme.Command = new RelayCommand(() => _ = OnTemaClickAsync(theme));
                Nodes.Add(theme);
                
                // Agregar al cache para búsqueda O(1)
                _nodeCache[theme.NodeId] = theme;
            }

            // ===== Generar EDGES automáticamente (conexiones secuenciales) =====
            Edges.Clear();
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                var fromNode = Nodes[i];
                var toNode = Nodes[i + 1];
                Edges.Add(new TemaEdge(fromNode.NodeId, toNode.NodeId));
            }

            if (topics.Count > 0 && topics[0].Level != null)
            {
                TituloLecciones = topics[0].Level!.LevelName.ToUpper(CultureInfo.InvariantCulture);
                LevelDescription = topics[0].Level!.Description ?? string.Empty;
            }
            else
            {
                TituloLecciones = $"NIVEL {_levelNumber}";
                LevelDescription = string.Empty;
            }

            FeaturedProgressPercentage = progressPercentage ?? 0;
            UpdateFeaturedTopic();
            
            RecalculateCanvasHeight();
            
        }
        catch (Exception)
        {
            Nodes.Clear();
            Edges.Clear();
            _nodeCache.Clear();
            FeaturedTopicTitle = string.Empty;
            FeaturedTopicDescription = string.Empty;
            FeaturedProgressPercentage = 0;
            TituloLecciones = $"NIVEL {_levelNumber}";
            LevelDescription = string.Empty;

            RecalculateCanvasHeight();
        }
    }

    /// <summary>
    /// Mapea un Topic de BD a ThemeData con posiciones visuales
    /// </summary>
    private ThemeData MapTopicToThemeData(
        Topic topic,
        int themeNumber,
        IReadOnlyDictionary<int, int> lessonProgressMap)
    {
        var iconPath = topic.Icon ?? "avares://Quibee/Assets/Images/SmallStar.png";
        var (positionX, positionY) = GetPatternPosition(themeNumber);
        var firstLessonId = topic.Lessons
            .Where(l => l.IsActive)
            .OrderBy(l => l.OrderIndex)
            .Select(l => l.IdLesson)
            .FirstOrDefault();
        lessonProgressMap.TryGetValue(firstLessonId, out var lessonProgressPercentage);
        
        return new ThemeData
        {
            TopicId = topic.IdTopic,
            ThemeNumber = themeNumber,
            Title = topic.TopicName,
            Description = topic.Description ?? string.Empty,
            ImagePath = iconPath,
            ImageWidth = topic.IconWidth,
            ImageHeight = topic.IconHeight,
            PositionX = positionX,
            PositionY = positionY,
            UseRightAlignment = false, // Siempre usar Canvas.Left
            TextOnLeft = false,
            TextOnRight = true,
            LessonId = firstLessonId,
            LessonProgressPercentage = lessonProgressPercentage
        };
    }

    private static (double X, double Y) GetPatternPosition(int themeNumber)
    {
        if (themeNumber <= 0)
        {
            return (190, 12);
        }

        const double leftX = 190;
        const double rightX = 430;
        const double firstY = 12;
        const double verticalStep = 116;

        var index = themeNumber - 1;
        var x = index % 2 == 0 ? leftX : rightX;
        var y = firstY + (index * verticalStep);

        return (x, y);
    }

    private void UpdateFeaturedTopic()
    {
        var firstTopic = Nodes
            .OrderBy(n => n.ThemeNumber)
            .FirstOrDefault();

        if (firstTopic == null)
        {
            FeaturedTopicTitle = string.Empty;
            FeaturedTopicDescription = string.Empty;
            return;
        }

        FeaturedTopicTitle = firstTopic.Title;
        FeaturedTopicDescription = string.IsNullOrWhiteSpace(firstTopic.Description)
            ? "Contenido disponible"
            : firstTopic.Description;
    }

    /// <summary>
    /// Busca un nodo por su ID usando cache para O(1) lookup
    /// (Crítico con ~30 nodos por nivel)
    /// </summary>
    public ThemeData? GetNodeById(string nodeId)
    {
        _nodeCache.TryGetValue(nodeId, out var node);
        return node;
    }

    private void RecalculateCanvasHeight()
    {
        const double bottomPadding = 260;
        var maxBottom = Nodes.Count == 0
            ? 700
            : Nodes.Max(n => n.PositionY + n.ImageHeight);

        CanvasHeight = Math.Max(700, maxBottom + bottomPadding);
        OnPropertyChanged(nameof(CanvasHeight));
    }

    // ===== NUEVO: Handler genérico para cualquier tema =====
    /// <summary>
    /// Handler genérico para clicks en temas. Recibe el nodo clickeado.
    /// </summary>
    private async Task OnTemaClickAsync(ThemeData theme)
    {
        var topic = await _topicService.GetTopicByIdAsync(theme.TopicId);
        if (topic == null)
        {
            return;
        }

        var lesson = topic.Lessons
            .Where(l => l.IsActive)
            .OrderBy(l => l.OrderIndex)
            .FirstOrDefault();

        if (lesson == null)
        {
            return;
        }

        var lessonData = new LessonData
        {
            Title = lesson.Title,
            LessonNumber = lesson.IdLesson,
            ThemeName = topic.TopicName,
            StudentId = _studentId,
            LevelNumber = _levelNumber,
            ThemeNumber = theme.ThemeNumber,
            Sections = new List<LessonSection>()
        };

        _mainWindowViewModel?.NavigateToGenericLesson(lessonData);
    }

    // Handlers de navegación
    private void OnInicio()
    {
        _mainWindowViewModel?.NavigateToWelcome();
    }

    private void OnManual()
    {
        // TODO: Navegar al manual
    }

    private void OnUsuario()
    {
        // TODO: Navegar al perfil del usuario
    }
}
