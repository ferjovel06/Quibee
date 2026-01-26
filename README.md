# 🚀 Quibee - Plataforma Educativa Interactiva

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square)
![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC?style=flat-square)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat-square&logo=mysql&logoColor=white)
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
- **[Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)** - Provider MySQL para EF Core

### Base de Datos
- **[MySQL 8.0](https://www.mysql.com/)** - Sistema de gestión de bases de datos
- **MySQL Workbench** - Herramienta de administración (opcional)

### Herramientas de Desarrollo
- **[NixOS](https://nixos.org/)** - Sistema operativo (desarrollo)
- **[Visual Studio Code](https://code.visualstudio.com/)** - Editor de código
- **Git** - Control de versiones

## 📦 Instalación

### Prerrequisitos

```bash
# Verificar instalación de .NET
dotnet --version  # Debe ser 9.0 o superior

# Verificar instalación de MySQL
mysql --version   # Debe ser 8.0 o superior
```

### Clonar el Repositorio

```bash
git clone https://github.com/ferjovel06/Quibee.git
cd Quibee/Quibee
```

### Configurar Base de Datos

1. **Crear la base de datos:**
```sql
CREATE DATABASE quibee_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

2. **Ejecutar el schema:**
```bash
mysql -u root -p quibee_db < Database/schema_improved.sql
```

3. **Ejecutar migraciones:**
```bash
mysql -u root -p quibee_db < Database/migration_add_topic_positions.sql
mysql -u root -p quibee_db < Database/sync_topics_with_fallback.sql
```

4. **Configurar cadena de conexión en `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "QuibeeDatabase": "Server=localhost;Database=quibee_db;User=root;Password=TU_PASSWORD;"
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
