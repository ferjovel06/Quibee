# WelcomeView - Guía de Diseño Visual

## 🎨 Filosofía de Diseño

La pantalla de bienvenida de Quibee adopta una **estética espacial lúdica** que combina elementos educativos (números) con un ambiente cósmico divertido. El diseño busca:

1. **Atraer la atención** de estudiantes jóvenes
2. **Transmitir diversión** en el aprendizaje de matemáticas
3. **Establecer identidad de marca** con el robot Quibee
4. **Crear atmósfera amigable** y no intimidante

## 🌈 Sistema de Color

### Paleta Principal

```css
/* Fondo */
--background-primary: #311B42;    /* Morado oscuro - transmite misterio espacial */

/* Acentos */
--accent-red: #D51A1A;            /* Rojo vibrante - llamadas a la acción */
--accent-red-hover: #D62839;      /* Rojo hover - interacción */
--accent-pink: #F25E67;           /* Rosa/coral - texto amigable */

/* Texto */
--text-primary: #F2F2F2;          /* Blanco humo - alta legibilidad */
--text-button: #FFFFFF;           /* Blanco puro - contraste en botones */
```

### Contraste y Accesibilidad

- **Ratio de contraste fondo/texto**: >7:1 (AAA según WCAG)
- **Botón rojo sobre morado**: Excelente contraste visual
- **Texto blanco sobre morado**: Alta legibilidad

## 📐 Sistema de Espaciado

### Unidad Base
- **Base**: 10px
- Los espacios siguen múltiplos de 10 para consistencia

### Margins Principales

```
Robot:           50px desde la izquierda
Meteorito:       80px desde la izquierda, 5px desde arriba
Texto principal: 40px margen inferior
Botón:           Alineación natural del grid
```

## 🔤 Tipografía

### Fuentes Utilizadas

#### LilitaOne
- **Tipo**: Display/Decorativa
- **Uso**: Títulos, texto principal
- **Características**: Redondeada, amigable, legible
- **Tamaños**:
  - 100px - Texto principal ("Matemáticas con QUIBEE")
  - 48px - Saludo ("Bienvenido")

#### MoreSugar
- **Tipo**: Display/Manuscrita
- **Uso**: Botones
- **Características**: Divertida, manuscrita, juvenil
- **Tamaños**:
  - 30px - Botones

### Jerarquía Tipográfica

```
Nivel 1: "Matemáticas con QUIBEE" - 100px (Mensaje principal)
Nivel 2: "Bienvenido"              - 48px  (Saludo secundario)
Nivel 3: "Inicio"                  - 30px  (Acción)
```

### Propiedades Especiales

- **Rotación**: -3° en elementos de texto para dinamismo
- **TextAlignment**: Left (mantiene consistencia)
- **Spacing**: 0 en StackPanel del texto principal (compacto)

## 🖼️ Elementos Decorativos

### Tema Espacial

#### Estrellas (★)
- **Cantidad**: 12
- **Tamaño**: 24x24 px (pequeñas, no invasivas)
- **Opacidad**: 0.6 - 0.9 (crear profundidad)
- **Distribución**: Uniforme, evitando superposición
- **Propósito**: Ambiente, relleno visual sutil

#### Cuerpos Celestes
| Elemento | Tamaño | Ubicación | Propósito |
|----------|--------|-----------|-----------|
| Luna | 240x240 | Inf. Izq. | Balance, suavidad |
| Agujero Negro | 300x300 | Inf. Der. | Misterio, profundidad |
| Meteorito | 320x320 | Sup. Izq. | Movimiento, dinamismo |

#### Números Decorativos
| Número | Tamaño | Ubicación | Simbolismo |
|--------|--------|-----------|------------|
| 1 | 140x140 | Sup. Der. | Principio, inicio |
| 3 | 120x120 | Sobre texto | Número mágico, triada |
| 5 | 120x120 | Sup. Der. | Completitud |
| 6 | 120x120 | Inf. Der. | Equilibrio |

**Nota**: Los números refuerzan el tema matemático de forma sutil y decorativa.

### Robot Quibee (Mascota)
- **Tamaño**: 300x375 px
- **Ubicación**: Izquierda centro
- **Propósito**: 
  - Identidad de marca
  - Guía/compañero del usuario
  - Punto focal secundario
- **Posicionamiento**: En Border para mejor control

## 📏 Layout y Composición

### Principios de Composición

1. **Regla de Tercios**: El robot ocupa el tercio izquierdo
2. **Punto Focal**: El texto principal está centrado verticalmente
3. **Balance Asimétrico**: Elementos decorativos balancean el peso del robot
4. **Espacio Negativo**: Áreas vacías permiten respirar al diseño

## 🎭 Efectos Visuales

### Opacidad y Profundidad
```
Capa Frontal:    Texto, Botón          (Opacity: 1.0)
Capa Media:      Robot, Números        (Opacity: 1.0)
Capa Fondo:      Luna, Agujero Negro   (Opacity: 0.9-0.95)
Capa Ambiente:   Estrellas             (Opacity: 0.6-0.9)
```

### Estados Interactivos

#### Botón "Inicio"
- **Normal**: 
  - Background: #D51A1A
  - Cursor: Default
  
- **Hover** (:pointerover):
  - Background: #D62839 (más claro)
  - Cursor: Hand
  - Transición: Suave (implícita en Avalonia)

### Border Radius
- **Botón**: 30px (muy redondeado, amigable)
- **Propósito**: Suavizar formas, aspecto lúdico

## 📱 Diseño Responsive

### Resolución Objetivo
- **Primaria**: 1920x1080 (Full HD)
- **Estrategia**: Grid flexible con columnas `*` (proporcionales)

### Adaptabilidad
```
Elementos Flexibles:
- Columnas del grid (se ajustan proporcionalmente)
- Filas superior e inferior (espacio flexible)

Elementos Fijos:
- Tamaños de imágenes (mantienen proporción)
- Tamaños de fuente (legibilidad consistente)
```

### Consideraciones

⚠️ **Importante**: 
- En pantallas más pequeñas, considerar reducir tamaño de fuentes
- Las posiciones absolutas de estrellas pueden requerir ajuste
- Verificar que el robot no obstruya contenido en pantallas estrechas

## 🎯 Principios de Diseño Aplicados

### 1. Consistencia
- Todos los elementos de texto rotados -3°
- Paleta de colores limitada y coherente
- Tipografía consistente por tipo de elemento

### 2. Jerarquía
- Texto principal es el más grande (100px)
- Saludo secundario (48px)
- Botón de acción (30px)

### 3. Contraste
- Fondo oscuro vs texto claro
- Botón rojo vibrante sobre morado
- Rosa coral para elementos de acento

### 4. Alineación
- Contenido principal alineado a la izquierda
- Robot alineado a la izquierda
- Uso de Grid para alineación precisa

### 5. Proximidad
- Texto "Matemáticas" y "con QUIBEE" agrupados (spacing: 0)
- Decoraciones espaciales distribuidas pero relacionadas temáticamente

### 6. Repetición
- Múltiples estrellas crean patrón
- Números repetidos refuerzan tema matemático
- Esquema de color repetido

## 🔍 Detalles Técnicos

### Interpolación de Imágenes
```xml
RenderOptions.BitmapInterpolationMode="HighQuality"
```
- Aplicado a todas las imágenes
- Asegura renderizado suave y de alta calidad
- Evita pixelación en diferentes tamaños de pantalla

### Transformaciones
```xml
<TransformGroup>
    <RotateTransform Angle="-3"/>
</TransformGroup>
```
- Centro de rotación: 0.5, 0.5 (centro del elemento)
- Mantiene centrado durante la rotación

## 📊 Métricas de Diseño

### Densidad Visual
- **Alta**: Esquinas (decoraciones múltiples)
- **Media**: Laterales (robot, números)
- **Baja**: Centro (contenido principal, máxima legibilidad)

### Balance de Peso Visual
```
Izquierda: ████████░░  80% (Robot + meteorito + luna)
Centro:    ██████████ 100% (Contenido principal)
Derecha:   ███████░░░  70% (Números + agujero negro)
```

## 🎨 Mood Board

**Conceptos Clave:**
- 🚀 Espacio
- 🎮 Diversión
- 📚 Educación
- 🤖 Tecnología amigable
- ✨ Magia del aprendizaje

**Referencias de Estilo:**
- Aplicaciones educativas infantiles
- Juegos casuales espaciales
- Material Design (simplicidad)
- Ilustraciones planas modernas

## 📋 Checklist de Diseño

Al modificar este diseño, verificar:

- [ ] ¿Se mantiene el contraste de colores (accesibilidad)?
- [ ] ¿Los elementos decorativos NO obstruyen el contenido?
- [ ] ¿La jerarquía visual es clara?
- [ ] ¿El botón es fácilmente identificable?
- [ ] ¿Las estrellas están distribuidas uniformemente?
- [ ] ¿Se mantiene la identidad visual de Quibee?
- [ ] ¿El texto es legible en todos los tamaños?
- [ ] ¿Los efectos hover son evidentes?

---

**Última actualización**: Noviembre 16, 2025  
**Diseñador**: Equipo Quibee  
**Revisión**: v1.0
