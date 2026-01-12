# 🤖 Quibee

Plataforma educativa para que niños de primaria aprendan matemáticas con el robot Q-BIT.

## ¿Qué necesito instalar?

### 1. Instalar .NET 9.0
1. Ve a: https://dotnet.microsoft.com/download/dotnet/9.0
2. Descarga el **SDK** (no el Runtime)
3. Ejecuta el instalador
4. Abre una terminal y verifica:
   ```
   dotnet --version
   ```
   Debe mostrar algo como `9.0.x`

### 2. Instalar Git (si no lo tienes)
1. Ve a: https://git-scm.com/download/win
2. Descarga e instala
3. Deja todo por defecto (Next, Next, Next...)

### 3. Instalar Visual Studio Code (opcional pero recomendado)
1. Ve a: https://code.visualstudio.com/
2. Descarga e instala
3. Abre VS Code e instala la extensión **C# Dev Kit**

## ¿Cómo lo ejecuto?

### Opción 1: Desde la terminal (más fácil)

1. **Abrir PowerShell** (búscalo en el menú inicio)

2. **Clonar el proyecto:**
   ```powershell
   git clone https://github.com/ferjovel06/Quibee.git
   cd Quibee
   ```

3. **Instalar dependencias** (Avalonia se descarga automáticamente):
   ```powershell
   dotnet restore
   ```
   
4. **Ejecutar:**
   ```powershell
   cd Quibee
   dotnet run
   ```

5. **¡Listo!** Debe abrir la ventana de la aplicación

### Opción 2: Desde Visual Studio Code

1. Abre VS Code
2. File → Open Folder → Selecciona la carpeta `Quibee`
3. Abre una terminal en VS Code (Terminal → New Terminal)
4. Ejecuta:
   ```powershell
   cd Quibee
   dotnet run
   ```

## ¿Qué tecnologías usa?

- **Avalonia UI** 11.3.6 - Framework para crear interfaces (como WPF pero cross-platform)
- **.NET 9.0** - Lo que hace que todo funcione
- **MySQL 8.0** - Base de datos (próximamente)

## ¿Problemas?

Si algo no funciona:
1. Verifica que instalaste .NET 9.0 SDK (no Runtime)
2. Ejecuta `dotnet restore` de nuevo
3. Cierra y vuelve a abrir la terminal
4. Pregunta en el grupo 😊
