# 🗺️ Mapa de Lecciones - Documentación

## Vista Creada: `LessonsMapView`

Esta es la pantalla principal que ve el estudiante después de hacer login o completar el registro.

## 📊 Elementos de la UI

### 1. Barra de Navegación Superior
- **Inicio**: Vuelve a la pantalla de bienvenida
- **Lecciones**: Vista actual (botón blanco activo)
- **Manual**: Abre el manual de usuario (próximamente)
- **Usuario**: Botón rojo para ver perfil (próximamente)

### 2. Título
- Muestra: "Lecciones: [Primer/Segundo/Tercer] grado"
- Se actualiza dinámicamente según el grado del estudiante

### 3. Mapa de Temas (5 temas)

#### Tema 1: Cuerpos geométricos
- **Ícono**: Planeta morado (LilacPlanet.png)
- **Posición**: Arriba a la derecha
- **Tamaño**: 100x100px

#### Tema 2: Fracciones
- **Ícono**: Estrella amarilla (Star.png)
- **Posición**: Centro arriba
- **Tamaño**: 100x100px

#### Tema 3: Números decimales
- **Ícono**: Robot (Robot.png)
- **Posición**: Derecha centro
- **Tamaño**: 100x100px

#### Tema 4: Operaciones combinadas
- **Ícono**: Alien verde (Alien.png)
- **Posición**: Izquierda centro
- **Tamaño**: 100x100px

#### Tema 5: Medidas de capacidad
- **Ícono**: Planeta verde (GreenPlanet.png)
- **Posición**: Abajo izquierda
- **Tamaño**: 120x120px

### 4. Conexiones Visuales
- Líneas punteadas blancas conectan los temas
- Crea un "camino" visual entre los temas
- Estilo: StrokeDashArray="5,5"

## 🎨 Diseño

### Colores
- **Fondo**: #311B42 (morado oscuro)
- **Texto**: Blanco
- **Botón activo**: Blanco con texto morado
- **Botón usuario**: #D51A52 (rojo)
- **Líneas**: Blanco con opacidad

### Tipografía
- **Fuente**: LilitaOne
- **Título**: 42px
- **Navegación**: 24px
- **Títulos de temas**: 18px
- **Descripciones**: 14px

### Layout
- **Tamaño virtual**: 1200x700px
- **Responsive**: ScrollViewer + Viewbox
- **Posicionamiento**: Canvas para ubicación absoluta de temas

## 🔗 Navegación

### Entrada a esta vista:
1. **Desde Registro**: Después de completar el registro (RegistrationConfirmationView)
2. **Desde Login**: Después de hacer login exitoso (LoginView)

### Método de navegación:
```csharp
_mainWindowViewModel.NavigateToLessonsMap(studentId, gradeLevel);
```

### Salida de esta vista:
- **Inicio**: Vuelve a WelcomeView
- **Usuario**: Perfil del estudiante (próximamente)
- **Click en tema**: Abre las lecciones de ese tema (próximamente)

## 📝 Archivos Involucrados

```
/ViewModels/
  └── LessonsMapViewModel.cs      (Lógica y comandos)

/Views/
  ├── LessonsMapView.axaml        (UI en XAML)
  └── LessonsMapView.axaml.cs     (Code-behind)

/ViewModels/
  ├── MainWindowViewModel.cs      (Método NavigateToLessonsMap)
  ├── LoginViewModel.cs           (Navega aquí después del login)
  └── RegistrationConfirmationViewModel.cs  (Navega aquí después del registro)

/Views/
  └── MainWindow.axaml            (DataTemplate agregado)
```

## 🎮 Funcionalidad Actual

### ✅ Implementado:
- Vista del mapa con 5 temas
- Navegación desde registro y login
- Títulos dinámicos según el grado
- Barra de navegación superior
- Diseño responsive

### 🚧 Por implementar:
- Click en temas → abrir lecciones específicas
- Perfil de usuario
- Manual de ayuda
- Bloqueo de temas (desbloquear según progreso)
- Indicadores visuales de progreso (estrellas, porcentaje)
- Animaciones de entrada

## 🧪 Cómo Probar

### Test 1: Desde el código (temporal)
```csharp
// En MainWindowViewModel constructor:
_currentView = new LessonsMapViewModel(this, studentId: 1, gradeLevel: 3);
```

### Test 2: Flujo completo
1. Ejecutar la app
2. Click en "Inicio"
3. Hacer login con un usuario existente
4. Debería aparecer el mapa de lecciones

### Test 3: Desde registro
1. Ejecutar la app
2. Completar el registro
3. Click en "Continuar" en la confirmación
4. Debería aparecer el mapa de lecciones

## 💾 Datos del Estudiante

La vista recibe:
- `studentId`: ID único del estudiante en la base de datos
- `gradeLevel`: Número del grado (1, 2 o 3)

Estos datos se usan para:
- Mostrar el título correcto
- (Futuro) Cargar el progreso del estudiante
- (Futuro) Personalizar los temas según el grado

## 🎯 Próximos Pasos

1. **Crear vista de lecciones por tema**
   - Lista de lecciones del tema seleccionado
   - Indicadores de completado

2. **Implementar perfil de usuario**
   - Estadísticas
   - Logros
   - Configuración

3. **Sistema de progreso**
   - Guardar qué temas/lecciones completó
   - Desbloquear temas progresivamente
   - Mostrar estrellas ganadas

4. **Manual de ayuda**
   - Tutoriales
   - Instrucciones para padres

---

**Estado**: ✅ Vista completada y funcional
**Fecha**: 13 de enero de 2026
