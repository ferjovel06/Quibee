using System;
using System.Linq;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

public class DatabaseTestService
{
    private readonly QuibeeDbContext _context;

    public DatabaseTestService(QuibeeDbContext context)
    {
        _context = context;
    }

    public bool TestConnection()
    {
        try
        {
            // Intenta conectar
            var canConnect = _context.Database.CanConnect();
            
            if (canConnect)
            {
                Console.WriteLine("✅ Conexión exitosa a la base de datos!");
                
                // 🚀 CREAR TABLAS SI NO EXISTEN
                Console.WriteLine("🔨 Creando tablas si no existen...");
                _context.Database.EnsureCreated();
                Console.WriteLine("✅ Tablas verificadas/creadas!");
                
                // Contar tablas
                var studentCount = _context.Students.Count();
                var levelCount = _context.Levels.Count();
                
                Console.WriteLine($"📊 Estudiantes registrados: {studentCount}");
                Console.WriteLine($"📚 Niveles disponibles: {levelCount}");
                
                return true;
            }
            else
            {
                Console.WriteLine("❌ No se pudo conectar a la base de datos");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al conectar: {ex.Message}");
            return false;
        }
    }

    public void CreateTestStudent()
    {
        try
        {
            // Crear datos de prueba usando el servicio real
            var testData = new UserRegistrationData
            {
                Nombres = "Fernando",
                Apellidos = "Prueba",
                FechaNacimiento = new DateTime(2015, 5, 10),
                Genero = "Masculino",
                Grado = "Tercer grado",
                ClaveAcceso = "1234"
            };

            var studentService = new StudentService(_context);
            var student = studentService.RegisterStudentAsync(testData).Result;

            if (student != null)
            {
                Console.WriteLine("✅ Estudiante de prueba creado correctamente!");
                Console.WriteLine($"   👤 Usuario: {student.Username}");
                Console.WriteLine($"   📛 Nombre: {student.FirstName} {student.LastName}");
                Console.WriteLine($"   🎓 Grado: {student.GradeLevel}°");
                Console.WriteLine($"   🔑 ID: {student.IdStudent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al crear estudiante: {ex.Message}");
        }
    }
}
