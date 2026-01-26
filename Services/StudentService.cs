using System;
using System.Linq;
using System.Threading.Tasks;
using Quibee.Database;
using Quibee.Models;

namespace Quibee.Services;

/// <summary>
/// Servicio para gestionar operaciones de estudiantes (registro, login, etc.)
/// </summary>
public class StudentService
{
    private readonly QuibeeDbContext _context;

    public StudentService(QuibeeDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registra un nuevo estudiante en la base de datos
    /// </summary>
    /// <param name="userData">Datos del formulario de registro</param>
    /// <returns>El estudiante creado con su ID asignado, o null si hubo error</returns>
    public async Task<Student?> RegisterStudentAsync(UserRegistrationData userData)
    {
        try
        {
            // Validar que el nombre de usuario no exista
            var username = GenerateUsername(userData.Nombres, userData.Apellidos);
            var existingUser = await Task.Run(() => 
                _context.Students.FirstOrDefault(s => s.Username == username));

            if (existingUser != null)
            {
                Console.WriteLine($"❌ El usuario {username} ya existe");
                return null;
            }

            // Mapear género de español a inglés
            var gender = MapGender(userData.Genero);

            // Extraer número de grado
            var gradeLevel = ExtractGradeLevel(userData.Grado);

            // Crear entidad Student
            var student = new Student
            {
                Username = username,
                FirstName = userData.Nombres,
                LastName = userData.Apellidos,
                DateOfBirth = userData.FechaNacimiento ?? DateTime.Now,
                GradeLevel = gradeLevel,
                Gender = gender,
                AccessCode = userData.ClaveAcceso,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            // Guardar en la base de datos
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Estudiante registrado: {student.Username} (ID: {student.IdStudent})");

            // Crear estadísticas iniciales
            await CreateInitialStatsAsync(student.IdStudent);

            return student;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al registrar estudiante: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Valida el login de un estudiante
    /// </summary>
    /// <param name="fullName">Nombre completo (Nombres + Apellidos)</param>
    /// <param name="accessCode">PIN de 4 dígitos</param>
    /// <returns>El estudiante si las credenciales son correctas, null si no</returns>
    public async Task<Student?> LoginAsync(string fullName, string accessCode)
    {
        try
        {
            // Buscar estudiante por nombre completo y PIN
            var student = await Task.Run(() => 
                _context.Students.FirstOrDefault(s => 
                    (s.FirstName + " " + s.LastName).ToLower() == fullName.ToLower() &&
                    s.AccessCode == accessCode &&
                    s.IsActive));

            if (student != null)
            {
                Console.WriteLine($"✅ Login exitoso: {student.Username}");
                
                // Actualizar última actividad
                if (student.Stats != null)
                {
                    student.Stats.LastActivityDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                Console.WriteLine($"❌ Login fallido: Credenciales incorrectas");
            }

            return student;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en login: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Obtiene un estudiante por su ID
    /// </summary>
    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        return await Task.Run(() => 
            _context.Students.FirstOrDefault(s => s.IdStudent == studentId));
    }

    /// <summary>
    /// Verifica si un nombre de usuario ya existe
    /// </summary>
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await Task.Run(() => 
            _context.Students.Any(s => s.Username == username));
    }

    // ===== MÉTODOS PRIVADOS DE UTILIDAD =====

    private async Task CreateInitialStatsAsync(int studentId)
    {
        var stats = new StudentStats
        {
            IdStudent = studentId,
            TotalPoints = 0,
            LessonsCompleted = 0,
            ExercisesCompleted = 0,
            CurrentStreak = 0,
            LongestStreak = 0,
            LastActivityDate = DateTime.Now
        };

        _context.StudentStats.Add(stats);
        await _context.SaveChangesAsync();
    }

    private string GenerateUsername(string firstName, string lastName)
    {
        // Generar username: primera letra del nombre + apellido + número random
        var username = $"{firstName[0]}{lastName}".ToLower();
        username = RemoveAccents(username);
        
        // Agregar número aleatorio si ya existe
        var random = new Random();
        var suffix = random.Next(100, 999);
        return $"{username}{suffix}";
    }

    private string MapGender(string generoEspanol)
    {
        return generoEspanol.ToLower() switch
        {
            "masculino" => "male",
            "femenino" => "female",
            _ => "prefer_not_to_say"
        };
    }

    private int ExtractGradeLevel(string grado)
    {
        if (grado.Contains("Primer") || grado.Contains("1")) return 1;
        if (grado.Contains("Segundo") || grado.Contains("2")) return 2;
        if (grado.Contains("Tercer") || grado.Contains("3")) return 3;
        return 1; // Default
    }

    private string RemoveAccents(string text)
    {
        var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var c in normalized)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
}
