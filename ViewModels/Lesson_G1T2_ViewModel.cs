using System.Windows.Input;

namespace Quibee.ViewModels;

/// <summary>
/// ViewModel para la lección: Relacionemos números y objetos (Grado 1, Tema 2)
/// </summary>
public class Lesson_G1T2_ViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    private readonly int _studentId;

    public Lesson_G1T2_ViewModel(
        MainWindowViewModel? mainWindowViewModel,
        int studentId)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _studentId = studentId;

        // Comandos
        VolverCommand = new RelayCommand(OnVolver);
        SiguienteCommand = new RelayCommand(OnSiguiente);
    }

    // Comandos
    public ICommand VolverCommand { get; }
    public ICommand SiguienteCommand { get; }

    // Contenido de la lección
    public string LessonTitle => "Lección: Relacionemos números y objetos";
    
    public string IntroText => 
        "¡Bienvenidos a esta lección de matemáticas donde aprenderemos a relacionar números con " +
        "cantidades de objetos! En nuestra vida diaria, vemos muchos objetos que representan " +
        "cantidades en el mundo real. En esta lección, exploraremos cómo vincular números con " +
        "grupos de objetos y practicaremos para que seamos muy buenos en esto.";

    public string WhatDoesItMeanTitle => "¿Qué significa relacionar números con objetos?";
    
    public string WhatDoesItMeanText =>
        "Relacionar números con objetos significa contar la cantidad de cosas que hay en un grupo y " +
        "encontrar el número que representa esa cantidad. Por ejemplo, si tenemos 3 manzanas en una " +
        "canasta, el número 3 es el que representa el total de manzanas. En palabras más sencillas, " +
        "\"relacionar\" quiere decir unir el número correcto con la cantidad de objetos que vemos.";

    public string AnalicemosTitle => "Analicemos";
    
    public string AnalicemosText =>
        "Imagina que estás contando las galletas en un plato. Cada galleta representa un objeto, y al " +
        "contarlas, obtienes un número que dice cuántas galletas hay en total.";

    public string ExampleQuestion => "Galletas en el plato: 🍪🍪🍪🍪🍪 (5 galletas)";
    
    public string ExampleAnswer => 
        "En este ejemplo, el número 5 nos dice cuántas galletas hay en total.";

    // Handlers
    private void OnVolver()
    {
        // Volver al mapa de lecciones
        _mainWindowViewModel?.NavigateToLessonsMap(_studentId, gradeLevel: 1);
    }

    private void OnSiguiente()
    {
        System.Console.WriteLine($"🎮 Avanzando a la siguiente sección de la lección");
        // TODO: Navegar a ejercicios prácticos o siguiente sección
    }
}
