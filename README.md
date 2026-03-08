# 🚀 Quibee - Plataforma Educativa Interactiva

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square)
![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC?style=flat-square)
![SQLite](https://img.shields.io/badge/SQLite-Embedded-003B57?style=flat-square&logo=sqlite&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?style=flat-square&logo=csharp&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

Aplicación educativa de escritorio desarrollada con **Avalonia UI** y **.NET 9**, diseñada para enseñar matemáticas de forma interactiva a estudiantes de primaria mediante un sistema de lecciones dinámicas y gamificadas.


## 🛠️ Tecnologías

### Frontend
- **[Avalonia UI 11.3.6](https://avaloniaui.net/)** - Framework UI multiplataforma
- **XAML** - Lenguaje de marcado declarativo
- **C# 12** - Lenguaje de programación

### Backend
- **[.NET 9.0](https://dotnet.microsoft.com/)** - Runtime y SDK
- **[Entity Framework Core](https://docs.microsoft.com/ef/)** - ORM para acceso a datos
- **[Microsoft.EntityFrameworkCore.Sqlite](https://learn.microsoft.com/ef/core/providers/sqlite/)** - Provider SQLite embebido

### Base de Datos
- **SQLite embebida (archivo local)**

### Herramientas de Desarrollo
- **[NixOS](https://nixos.org/)** - Sistema operativo (desarrollo)
- **[Visual Studio Code](https://code.visualstudio.com/)** - Editor de código
- **Git** - Control de versiones

## 📦 Instalación

### Prerrequisitos

```bash
# Verificar instalación de .NET
dotnet --version  # Debe ser 9.0 o superior

```

### Clonar el Repositorio

```bash
git clone https://github.com/ferjovel06/Quibee.git
cd Quibee/Quibee
```

### Configurar Base de Datos 

Cadena de conexión por defecto en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "QuibeeDatabase": "Data Source=quibee.db"
  }
}
```

### Compilar y Ejecutar

#### Usando .NET CLI
```bash
dotnet restore
dotnet build
dotnet run
```

El DataSeeder se ejecutará automáticamente en el primer inicio y poblará la base de datos con datos iniciales (3 grados, 15 temas, ejemplos de lecciones).

## 🔄 Auto-actualización (Windows)

La app está preparada para actualizarse desde GitHub Releases usando Velopack.

1. Configura `appsettings.json`:

```json
"AutoUpdate": {
  "Enabled": true,
  "CheckOnStartup": true,
  "GithubRepoUrl": "https://github.com/TU_USUARIO/TU_REPO"
}
```

2. Instala la herramienta de empaquetado:

```bash
dotnet tool install -g vpk
```

3. Publica binarios y crea paquete Velopack:

```bash
dotnet publish -c Release -r win-x64 --self-contained true
vpk pack --packId Quibee --packVersion 1.0.0 --packDir bin/Release/net9.0/win-x64/publish --mainExe Quibee.exe --outputDir releases
```

4. Sube el contenido de `releases` a un Release de GitHub.

En el siguiente inicio, la app verificará actualizaciones y pedirá confirmación para descargar e instalar.
