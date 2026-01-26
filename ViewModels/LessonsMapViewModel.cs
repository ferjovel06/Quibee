using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private readonly int _gradeLevel;
    private readonly TopicService _topicService;

    public LessonsMapViewModel(
        MainWindowViewModel? mainWindowViewModel,
        int studentId,
        int gradeLevel,
        TopicService topicService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _studentId = studentId;
        _gradeLevel = gradeLevel;
        _topicService = topicService;

        // Inicializar colección vacía
        Themes = new ObservableCollection<ThemeData>();

        // Comandos para cada tema
        Tema1Command = new RelayCommand(OnTema1Click);
        Tema2Command = new RelayCommand(OnTema2Click);
        Tema3Command = new RelayCommand(OnTema3Click);
        Tema4Command = new RelayCommand(OnTema4Click);
        Tema5Command = new RelayCommand(OnTema5Click);

        // Navegación
        InicioCommand = new RelayCommand(OnInicio);
        ManualCommand = new RelayCommand(OnManual);
        UsuarioCommand = new RelayCommand(OnUsuario);

        // Cargar temas desde BD de forma asíncrona
        _ = LoadThemesFromDatabaseAsync();
    }

    // Colección de temas dinámicos
    public ObservableCollection<ThemeData> Themes { get; }
    
    // Propiedades individuales para acceso directo en XAML
    public ThemeData Tema1 => Themes.Count > 0 ? Themes[0] : null!;
    public ThemeData Tema2 => Themes.Count > 1 ? Themes[1] : null!;
    public ThemeData Tema3 => Themes.Count > 2 ? Themes[2] : null!;
    public ThemeData Tema4 => Themes.Count > 3 ? Themes[3] : null!;
    public ThemeData Tema5 => Themes.Count > 4 ? Themes[4] : null!;

    // Propiedades para mostrar el grado
    public string TituloLecciones => $"Lecciones: {GradoTexto}";
    
    private string GradoTexto => _gradeLevel switch
    {
        1 => "Primer grado",
        2 => "Segundo grado",
        3 => "Tercer grado",
        _ => "Primer grado"
    };

    // Comandos de temas
    public ICommand Tema1Command { get; }
    public ICommand Tema2Command { get; }
    public ICommand Tema3Command { get; }
    public ICommand Tema4Command { get; }
    public ICommand Tema5Command { get; }

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
            var topics = await _topicService.GetTopicsByLevelAsync(_gradeLevel);
            
            // Mapear a ThemeData con posiciones según el grado
            var themesData = topics.Select(topic => MapTopicToThemeData(topic, topic.OrderIndex, _gradeLevel)).ToList();
            
            // Actualizar colección observable
            Themes.Clear();
            foreach (var theme in themesData)
            {
                Themes.Add(theme);
            }
            
            // Notificar que las propiedades individuales han cambiado
            OnPropertyChanged(nameof(Tema1));
            OnPropertyChanged(nameof(Tema2));
            OnPropertyChanged(nameof(Tema3));
            OnPropertyChanged(nameof(Tema4));
            OnPropertyChanged(nameof(Tema5));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error cargando temas desde BD: {ex.Message}");
            
            // Fallback: cargar datos estáticos si falla la BD
            var fallbackThemes = LoadThemesForGrade(_gradeLevel);
            foreach (var theme in fallbackThemes)
            {
                Themes.Add(theme);
            }
            
            // Notificar que las propiedades individuales han cambiado
            OnPropertyChanged(nameof(Tema1));
            OnPropertyChanged(nameof(Tema2));
            OnPropertyChanged(nameof(Tema3));
            OnPropertyChanged(nameof(Tema4));
            OnPropertyChanged(nameof(Tema5));
        }
    }

    /// <summary>
    /// Mapea un Topic de BD a ThemeData con posiciones visuales
    /// </summary>
    private ThemeData MapTopicToThemeData(Topic topic, int themeNumber, int gradeLevel)
    {
        var iconPath = topic.Icon ?? "avares://Quibee/Assets/Images/SmallStar.png";
        
        // Ajustar posición X cuando el texto está a la izquierda
        // El texto agrega ancho al botón, moviendo el contenido hacia la derecha
        var adjustedX = topic.PositionX;
        if (topic.TextOnLeft)
        {
            adjustedX -= 50; // Compensar el ancho del texto
        }
        
        return new ThemeData
        {
            TopicId = topic.IdTopic,
            ThemeNumber = themeNumber,
            Title = $"Tema {themeNumber}:",
            Description = topic.Description ?? topic.TopicName,
            ImagePath = iconPath,
            ImageWidth = topic.IconWidth,
            ImageHeight = topic.IconHeight,
            PositionX = adjustedX,
            PositionY = topic.PositionY,
            UseRightAlignment = false, // Siempre usar Canvas.Left
            TextOnLeft = topic.TextOnLeft,
            TextOnRight = topic.TextOnRight,
            RotationAngle = topic.RotationAngle
        };
    }

    /// <summary>
    /// FALLBACK: Carga los temas específicos según el grado (datos hardcodeados)
    /// Se usa como red de seguridad si falla la carga desde BD
    /// </summary>
    private List<ThemeData> LoadThemesForGrade(int grade)
    {
        return grade switch
        {
            1 => GetGrade1Themes(),
            2 => GetGrade2Themes(),
            3 => GetGrade3Themes(),
            _ => GetGrade1Themes() // Por defecto primer grado
        };
    }

    /// <summary>
    /// Temas de Primer Grado
    /// </summary>
    private List<ThemeData> GetGrade1Themes()
    {
        return new List<ThemeData>
        {
            new ThemeData
            {
                ThemeNumber = 1,
                Title = "Tema 1:",
                Description = "Conozcamos y escribamos\nlos números del 1 al 10",
                ImagePath = "avares://Quibee/Assets/Images/SmallStar.png",
                ImageWidth = 80,
                ImageHeight = 80,
                PositionX = 740,
                PositionY = 120,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 2,
                Title = "Tema 2:",
                Description = "Relacionemos\nnúmeros y objetos",
                ImagePath = "avares://Quibee/Assets/Images/LilacPlanet2.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 580,
                PositionY = 220,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 3,
                Title = "Tema 3:",
                Description = "Números cardinales del\n1° al 10°",
                ImagePath = "avares://Quibee/Assets/Images/Meteor.png",
                ImageWidth = 90,
                ImageHeight = 90,
                PositionX = 550,
                PositionY = 400,
                TextOnRight = true
            },
            new ThemeData
            {
                ThemeNumber = 4,
                Title = "Tema 4:",
                Description = "Suma y resta de\nnúmeros",
                ImagePath = "avares://Quibee/Assets/Images/Earth2.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 270,
                PositionY = 330,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 5,
                Title = "Tema 5:",
                Description = "Unidades y\ndecenas",
                ImagePath = "avares://Quibee/Assets/Images/Saturn2.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 280,
                PositionY = 520,
                TextOnRight = true
            }
        };
    }

    /// <summary>
    /// Temas de Segundo Grado
    /// </summary>
    private List<ThemeData> GetGrade2Themes()
    {
        return new List<ThemeData>
        {
            new ThemeData
            {
                ThemeNumber = 1,
                Title = "Tema 1:",
                Description = "Sumas y restas",
                ImagePath = "avares://Quibee/Assets/Images/Calculator.png",
                ImageWidth = 80,
                ImageHeight = 80,
                PositionX = 780,
                PositionY = 120,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 2,
                Title = "Tema 2:",
                Description = "Pictogramas",
                ImagePath = "avares://Quibee/Assets/Images/WhiteRocket.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 600,
                PositionY = 235,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 3,
                Title = "Tema 3:",
                Description = "Suma horizontal y vertical",
                ImagePath = "avares://Quibee/Assets/Images/SkyBlueAlien.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 550,
                PositionY = 400,
                TextOnRight = true
            },
            new ThemeData
            {
                ThemeNumber = 4,
                Title = "Tema 4:",
                Description = "Multiplicación",
                ImagePath = "avares://Quibee/Assets/Images/Star2.png",
                ImageWidth = 90,
                ImageHeight = 90,
                PositionX = 285,
                PositionY = 330,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 5,
                Title = "Tema 5:",
                Description = "Longitud",
                ImagePath = "avares://Quibee/Assets/Images/Neptune.png",
                ImageWidth = 70,
                ImageHeight = 70,
                PositionX = 280,
                PositionY = 520,
                TextOnRight = true
            }
        };
    }

    /// <summary>
    /// Temas de Tercer Grado
    /// </summary>
    private List<ThemeData> GetGrade3Themes()
    {
        return new List<ThemeData>
        {
            new ThemeData
            {
                ThemeNumber = 1,
                Title = "Tema 1:",
                Description = "Cuerpos geométricos",
                ImagePath = "avares://Quibee/Assets/Images/LilacPlanet.png",
                ImageWidth = 80,
                ImageHeight = 80,
                PositionX = 780,
                PositionY = 120,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 2,
                Title = "Tema 2:",
                Description = "Fracciones",
                ImagePath = "avares://Quibee/Assets/Images/Star2.png",
                ImageWidth = 80,
                ImageHeight = 80,
                PositionX = 640,
                PositionY = 220,
                UseRightAlignment = true,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 3,
                Title = "Tema 3:",
                Description = "Números decimales",
                ImagePath = "avares://Quibee/Assets/Images/SquirrelRocket.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 540,
                PositionY = 400,
                TextOnRight = true
            },
            new ThemeData
            {
                ThemeNumber = 4,
                Title = "Tema 4:",
                Description = "Operaciones combinadas",
                ImagePath = "avares://Quibee/Assets/Images/AlienFullBody.png",
                ImageWidth = 100,
                ImageHeight = 100,
                PositionX = 210,
                PositionY = 340,
                TextOnLeft = true
            },
            new ThemeData
            {
                ThemeNumber = 5,
                Title = "Tema 5:",
                Description = "Medidas de capacidad",
                ImagePath = "avares://Quibee/Assets/Images/GreenPlanet.png",
                ImageWidth = 70,
                ImageHeight = 70,
                PositionX = 280,
                PositionY = 520,
                TextOnRight = true
            }
        };
    }

    // Handlers de temas
    private void OnTema1Click()
    {
        System.Console.WriteLine($"🎮 Abriendo Tema 1 para estudiante {_studentId}");
        // TODO: Navegar a las lecciones del Tema 1
    }

    private void OnTema2Click()
    {
        System.Console.WriteLine($"🎮 Abriendo Tema 2 para estudiante {_studentId}");
        
        // Navegar según el grado
        if (_gradeLevel == 1)
        {
            // Grado 1, Tema 2: Relacionemos números y objetos
            var lessonData = CreateLesson_G1T2();
            _mainWindowViewModel?.NavigateToGenericLesson(lessonData);
        }
        else
        {
            // TODO: Implementar lecciones para otros grados
            System.Console.WriteLine($"⚠️ Lección no implementada para grado {_gradeLevel}, tema 2");
        }
    }

    /// <summary>
    /// Crea los datos para la lección Grado 1, Tema 2
    /// </summary>
    private LessonData CreateLesson_G1T2()
    {
        return new LessonData
        {
            Title = "Lección 1: Relacionemos números y objetos",
            LessonNumber = 1,
            ThemeName = "Relacionemos números y objetos",
            StudentId = _studentId,
            GradeLevel = 1,
            ThemeNumber = 2,
            Sections = new List<LessonSection>
            {
                new LessonSection
                {
                    Name = "Introducción",
                    IconPathLight = "avares://Quibee/Assets/Images/Icons/BulbLight.png",
                    IconPathDark = "avares://Quibee/Assets/Images/Icons/BulbDark.png",
                    Content = "¡Bienvenidos a esta lección de matemáticas donde aprenderemos a relacionar números con cantidades de objetos!\n\nEn nuestra vida diaria, vemos muchos objetos que representan cantidades en el mundo real. En esta lección, exploraremos cómo vincular números con grupos de objetos y practicaremos para que seamos muy buenos en esto.\n\n¿Qué significa relacionar números con objetos?\n\nRelacionar números con objetos significa contar la cantidad de cosas que hay en un grupo y encontrar el número que representa esa cantidad. Por ejemplo, si tenemos 3 manzanas en una canasta, el número 3 es el que representa el total de manzanas. En palabras más sencillas, 'relacionar' quiere decir unir el número correcto con la cantidad de objetos que vemos.",
                    IsSelected = true
                },
                new LessonSection
                {
                    Name = "Analicemos",
                    IconPathLight = "avares://Quibee/Assets/Images/Icons/BrainLight.png",
                    IconPathDark = "avares://Quibee/Assets/Images/Icons/BrainDark.png",
                    Content = "Imagina que estás contando las galletas en un plato. Cada galleta representa un objeto, y al contarlas, obtienes un número total que describe cuántas galletas hay.\n\n• Galletas en el plato: 🍪🍪🍪🍪🍪 (5 galletas)\n\nEn este ejemplo, el número 5 nos dice cuántas galletas hay en total.\n\n\n\n                      🍪  🍪\n                  🍪  🍪  🍪    →    5\n\n\n"
                },
                new LessonSection
                {
                    Name = "Ejercitemos",
                    IconPathLight = "avares://Quibee/Assets/Images/Icons/BookLight.png",
                    IconPathDark = "avares://Quibee/Assets/Images/Icons/BookDark.png",
                    Content = "Ahora es tu turno de practicar. Aquí encontrarás ejercicios para relacionar números con objetos.\n\n(Los ejercicios interactivos se agregarán en la siguiente fase)"
                },
                new LessonSection
                {
                    Name = "Resolvamos",
                    IconPathLight = "avares://Quibee/Assets/Images/Icons/PencilLight.png",
                    IconPathDark = "avares://Quibee/Assets/Images/Icons/PencilDark.png",
                    Content = "Vamos a resolver problemas juntos. Aquí verás cómo aplicar lo que aprendiste en situaciones del día a día.\n\n(Los problemas se agregarán en la siguiente fase)"
                },
                new LessonSection
                {
                    Name = "Desafío",
                    IconPathLight = "avares://Quibee/Assets/Images/Icons/FlagLight.png",
                    IconPathDark = "avares://Quibee/Assets/Images/Icons/FlagDark.png",
                    Content = "¿Estás listo para el desafío final? Pon a prueba todo lo que has aprendido.\n\n(El desafío se agregará en la siguiente fase)"
                }
            }
        };
    }

    private void OnTema3Click()
    {
        System.Console.WriteLine($"🎮 Abriendo Tema 3 para estudiante {_studentId}");
        // TODO: Navegar a las lecciones del Tema 3
    }

    private void OnTema4Click()
    {
        System.Console.WriteLine($"🎮 Abriendo Tema 4 para estudiante {_studentId}");
        // TODO: Navegar a las lecciones del Tema 4
    }

    private void OnTema5Click()
    {
        System.Console.WriteLine($"🎮 Abriendo Tema 5 para estudiante {_studentId}");
        // TODO: Navegar a las lecciones del Tema 5
    }

    // Handlers de navegación
    private void OnInicio()
    {
        _mainWindowViewModel?.NavigateToWelcome();
    }

    private void OnManual()
    {
        System.Console.WriteLine("📖 Abriendo Manual");
        // TODO: Navegar al manual
    }

    private void OnUsuario()
    {
        System.Console.WriteLine("👤 Abriendo perfil de usuario");
        // TODO: Navegar al perfil del usuario
    }
}
