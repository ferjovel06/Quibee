using System;
using Quibee.Database;
using Quibee.Services;

namespace Quibee;

/// <summary>
/// Service Locator simple para acceder a servicios en la aplicación
/// TODO: Considerar migrar a Dependency Injection con Microsoft.Extensions.DependencyInjection
/// </summary>
public static class ServiceLocator
{
    private static QuibeeDbContext? _dbContext;
    private static TopicService? _topicService;
    private static DataSeederService? _dataSeederService;
    private static LessonContentService? _lessonContentService;
    private static LessonProgressService? _lessonProgressService;

    /// <summary>
    /// Obtiene la instancia del DbContext (singleton)
    /// </summary>
    public static QuibeeDbContext GetDbContext()
    {
        if (_dbContext == null)
        {
            var factory = new QuibeeDbContextFactory();
            _dbContext = factory.CreateDbContext(Array.Empty<string>());
        }
        return _dbContext;
    }

    /// <summary>
    /// Obtiene el servicio de temas
    /// </summary>
    public static TopicService GetTopicService()
    {
        _topicService ??= new TopicService(GetDbContext());
        return _topicService;
    }

    /// <summary>
    /// Obtiene el servicio de inicialización de datos
    /// </summary>
    public static DataSeederService GetDataSeederService()
    {
        _dataSeederService ??= new DataSeederService(GetDbContext());
        return _dataSeederService;
    }

    /// <summary>
    /// Obtiene el servicio de contenido de lecciones
    /// </summary>
    public static LessonContentService GetLessonContentService()
    {
        _lessonContentService ??= new LessonContentService(GetDbContext());
        return _lessonContentService;
    }

    /// <summary>
    /// Obtiene el servicio de progreso de lecciones
    /// </summary>
    public static LessonProgressService GetLessonProgressService()
    {
        _lessonProgressService ??= new LessonProgressService(GetDbContext());
        return _lessonProgressService;
    }

    /// <summary>
    /// Libera recursos
    /// </summary>
    public static void Dispose()
    {
        _dbContext?.Dispose();
        _dbContext = null;
        _topicService = null;
        _dataSeederService = null;
        _lessonContentService = null;
        _lessonProgressService = null;
    }
}
