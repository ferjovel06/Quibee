# WelcomeView - Vista de Bienvenida

## 📋 Descripción General

`WelcomeView` es la pantalla de inicio de la aplicación Quibee. Presenta una interfaz amigable y colorida con elementos decorativos espaciales que invitan al usuario a comenzar su experiencia de aprendizaje de matemáticas.

## 🎯 Propósito

- Dar la bienvenida al usuario a la aplicación
- Presentar la identidad visual de Quibee (robot mascota)
- Proporcionar un punto de entrada claro a través del botón "Inicio"
- Crear una atmósfera lúdica y motivadora para el aprendizaje

## 🏗️ Estructura del Componente

### Jerarquía de Elementos

```
UserControl
└── Grid (contenedor principal)
    ├── Border (fondo morado)
    ├── Grid (elementos decorativos)
    │   ├── Estrellas (12 imágenes)
    │   ├── Números decorativos (1, 3, 5, 6)
    │   ├── Luna
    │   └── Agujero negro
    └── Grid (contenido principal)
        ├── Meteorito
        ├── Robot Quibee
        ├── Número 3
        ├── TextBlock "Bienvenido"
        ├── StackPanel (texto principal)
        │   ├── "Matemáticas"
        │   └── "con QUIBEE"
        └── Button "Inicio"
```

## 🎨 Estilos Definidos

### `welcomeButton`

Estilo personalizado para el botón de inicio.

**Propiedades:**
- **Background**: `#D51A1A` (rojo)
- **Foreground**: `White`
- **FontFamily**: `MoreSugar`
- **FontSize**: `30`
- **Padding**: `20,10`
- **CornerRadius**: `30` (botón redondeado)
- **Cursor**: `Hand`

**Estados:**
- **:pointerover**: Background cambia a `#D62839` (rojo más claro)

## 📐 Layout Principal

### Grid de Contenido

El layout utiliza un sistema de Grid con 5 filas y 3 columnas:

**Filas:**
- Fila 0: Altura flexible (`*`) - Espacio superior
- Fila 1: Auto - Texto "Bienvenido"
- Fila 2: Auto - Texto principal
- Fila 3: Auto - Botón
- Fila 4: Altura flexible (`*`) - Espacio inferior

**Columnas:**
- Columna 0: Ancho flexible (`*`) - Zona del robot
- Columna 1: Auto - Contenido central
- Columna 2: Ancho flexible (`*`) - Espacio derecho

## 🖼️ Elementos Decorativos

### Estrellas (12 total)

Distribuidas uniformemente por toda la pantalla para crear ambiente espacial:

**Zonas:**
- **Superior izquierda**: 2 estrellas (evitando meteorito)
- **Superior centro**: 2 estrellas
- **Superior derecha**: 1 estrella (evitando números 1 y 5)
- **Media izquierda**: 1 estrella (evitando robot)
- **Media derecha**: 1 estrella
- **Inferior izquierda**: 2 estrellas (evitando luna)
- **Inferior centro**: 2 estrellas
- **Inferior derecha**: 1 estrella (evitando agujero negro y número 6)

**Características:**
- Tamaño: 24x24 px
- Opacidad variable: 0.6 a 0.9

### Números Decorativos

- **Número 1**: Superior derecha (140x140 px), Margin: `0,140,0,0`
- **Número 3**: Sobre "Bienvenido" (120x120 px), Margin: `100,0,0,0`
- **Número 5**: Superior derecha (120x120 px), Margin: `0,0,200,0`
- **Número 6**: Inferior derecha (120x120 px), Margin: `0,0,600,0`

### Elementos Principales

- **Luna**: Esquina inferior izquierda (240x240 px), Opacity: 0.9
- **Agujero Negro**: Esquina inferior derecha (300x300 px), Opacity: 0.95
- **Meteorito**: Superior izquierda (320x320 px), sobre el robot
- **Robot Quibee**: Izquierda centro (300x375 px)

## 📝 Contenido de Texto

### "Bienvenido"
- **Fuente**: LilitaOne
- **Tamaño**: 48
- **Color**: `#F25E67` (rosa/coral)
- **Rotación**: -3 grados
- **Ubicación**: Fila 1, Columna 1

### "Matemáticas con QUIBEE"
- **Fuente**: LilitaOne
- **Tamaño**: 100
- **Color**: `#F2F2F2` (blanco humo)
- **Rotación**: -3 grados (todo el StackPanel)
- **Ubicación**: Fila 2, Columna 1
- **Estructura**: Dos TextBlocks en StackPanel

## 🔘 Interacciones

### Botón "Inicio"
- Cambia de color al pasar el mouse (hover effect)
- Cursor cambia a mano para indicar clickeable
- Rotación de -3 grados para mantener consistencia visual

## 🎨 Paleta de Colores

| Elemento | Color | Código Hex | Uso |
|----------|-------|------------|-----|
| Fondo | Morado oscuro | `#311B42` | Background principal |
| Botón normal | Rojo | `#D51A1A` | Botón "Inicio" |
| Botón hover | Rojo claro | `#D62839` | Botón al pasar mouse |
| "Bienvenido" | Rosa/Coral | `#F25E67` | Texto de saludo |
| Texto principal | Blanco humo | `#F2F2F2` | Texto "Matemáticas con QUIBEE" |
| Botón texto | Blanco | `White` | Texto del botón |

## 📦 Assets Utilizados

### Imágenes
- `/Assets/Images/Star.png` - Estrella decorativa
- `/Assets/Images/1.png` - Número uno decorativo
- `/Assets/Images/3.png` - Número tres decorativo
- `/Assets/Images/5.png` - Número cinco decorativo
- `/Assets/Images/6.png` - Número seis decorativo
- `/Assets/Images/Moon.png` - Luna
- `/Assets/Images/Blackhole.png` - Agujero negro
- `/Assets/Images/Meteor.png` - Meteorito
- `/Assets/Images/Robot.png` - Robot Quibee

### Fuentes
- `LilitaOne` - Títulos y texto principal
- `MoreSugar` - Botón

## 🔧 Configuración Técnica

### Propiedades del UserControl
- **Design Width**: 1920 px
- **Design Height**: 1080 px
- **DataType**: `WelcomeViewModel`

### Optimizaciones
- `RenderOptions.BitmapInterpolationMode="HighQuality"` en todas las imágenes para mejor calidad visual

## 📱 Consideraciones de Diseño

1. **Responsividad**: El layout usa Grid con columnas y filas flexibles (`*`) para adaptarse a diferentes tamaños de pantalla
2. **Jerarquía Visual**: El texto principal es significativamente más grande que el saludo para enfatizar la marca
3. **Rotación Sutil**: -3 grados en elementos de texto para dar un aspecto más dinámico y juguetón
4. **Espaciado**: Uso de margins cuidadosamente calculados para evitar superposición de elementos decorativos
5. **Opacidad Variable**: Las estrellas tienen diferentes opacidades para crear sensación de profundidad

## 🔗 Relaciones

- **ViewModel**: `WelcomeViewModel`
- **Parent**: Probablemente `MainWindow`
- **Navegación**: El botón "Inicio" debe navegar a la siguiente vista (configuración del usuario o menú principal)

## 📝 Notas de Mantenimiento

- Las posiciones de las estrellas están finamente ajustadas para evitar superposición - modificar con cuidado
- Los márgenes de elementos decorativos están optimizados para resolución 1920x1080
- Al agregar nuevos elementos decorativos, verificar que no interfieran con el contenido principal
- Mantener la paleta de colores consistente con el tema espacial de la aplicación

---

**Última actualización**: Noviembre 16, 2025  
**Autor**: Equipo Quibee
