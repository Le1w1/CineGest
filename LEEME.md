# CineGest

Sistema integral de gestión para un cine de atención presencial.

---

## Información del proyecto

| Dato | Valor |
|---|---|
| **Nombre del sistema** | CineGest |
| **Materia** | Ingeniería de Software |
| **Comisión** | 3-A |
| **Turno** | Mañana |
| **Sede** | Centro |
| **Año de cursada** | 2026 |
| **Universidad** | UAI – Universidad Abierta Interamericana |

### Integrantes

- Rodríguez Leonel Jesús
- Riccio Sebastián Gael

### Docentes

- Pereyra Jorge Agustín
- Silvestro Lucas Agustín

---

## Tecnologías utilizadas

- **Lenguaje y framework**: C# con .NET 9.0
- **Interfaz de usuario**: Windows Forms
- **Base de datos**: Microsoft SQL Server (cliente `Microsoft.Data.SqlClient`)
- **Encriptación de contraseñas**: SHA-256 (irreversible)
- **Internacionalización**: archivos JSON cargados en runtime (`es.json`, `en.json`)
- **Exportación**: `iTextSharp` para generación de reportes PDF (auditoría de bitácora)
- **IDE**: Microsoft Visual Studio 2022

---

## Arquitectura

El sistema sigue una **arquitectura en cuatro capas** sin uso de frameworks de persistencia de terceros:

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Presentación | `UI` | Formularios Windows Forms |
| Lógica de Negocio | `BLL` | Validaciones y reglas del dominio |
| Acceso a Datos | `DAL` | Operaciones SQL y transacciones |
| Servicios | `Servicios` | Entidades del dominio + servicios transversales (SM, Traductor, Cripto, IObservadorIdioma) |

**Nota**: En este proyecto **NO se utiliza una capa BE separada**. Las entidades del dominio residen en la capa `Servicios` junto con los servicios compartidos.

### Patrones de diseño aplicados

- **Singleton**: `SM` (sesión activa) y `Traductor` (servicio de traducción).
- **Observer**: cambio dinámico de idioma sobre formularios abiertos (`IObservadorIdioma`).
- **Composite**: jerarquía recursiva `Componente / PermisoSimple / Familia / Rol` para el modelo de control de accesos.

---

## Módulos implementados en esta entrega

| Módulo | Descripción |
|---|---|
| **Gestión de Sesión** | Inicio de sesión con email y contraseña, cierre de sesión, re-login, cambio de clave, bloqueo automático tras 3 intentos fallidos |
| **Gestión de Usuarios** | Crear, modificar, activar/desactivar, desbloquear, asignación de rol |
| **Encriptación** | Hash SHA-256 irreversible sobre las contraseñas (clase `Cripto`) |
| **Gestión de Roles y Familias** | Composite recursivo de permisos. Crear, modificar y eliminar Roles y Familias con validación de unicidad, ciclos y duplicados |
| **Cambio de Idioma** | Patrón Observer. Soporte para Español e Inglés. Persistencia del idioma del usuario al cerrar sesión |
| **Bitácora y Auditoría** | Registro automático de eventos sensibles. Consulta filtrada por usuario, fecha, módulo, criticidad y resultado. Exportación a PDF |

### Módulos planificados para próximas entregas

- Boletería (venta de entradas y productos de concesionaria)
- Cartelera (gestión de películas, funciones y salas)
- Gerencia (consultas comerciales y de ocupación)
- Dígitos Verificadores (control de integridad de datos)

---

## Instrucciones de instalación

### Requisitos previos

- Windows 10 / 11
- Microsoft SQL Server (versión 2019 o superior) — Express edition es suficiente
- .NET 9.0 SDK
- Microsoft Visual Studio 2022 (o compatible con .NET 9.0)
- Resolución mínima 1366×768

### Pasos

1. **Clonar o descomprimir** el proyecto en una carpeta local.

2. **Crear la base de datos** en SQL Server Management Studio:
   - Crear una base con el nombre `CineGestDB`.

3. **Ejecutar los scripts SQL en el siguiente orden** (ubicados en la carpeta raíz del proyecto):

   | Orden | Script | Propósito |
   |---|---|---|
   | 1 | `CineGestDB.sql` | Crea las tablas base (`Usuario`, `BitacoraEvento`) e inserta el usuario administrador inicial. |
   | 2 | `Script_Idioma.sql` | Crea la tabla `Idioma` y agrega `IdIdioma` a `Usuario`. |
   | 3 | `Script_Roles_Familias_Permisos.sql` | Crea las tablas del modelo RBAC (8 tablas) y carga los permisos, familias y roles iniciales. |
   | 4 | `Script_Migrar_UsuarioRol_a_Usuario.sql` | Migra el modelo a 1 Rol por Usuario (elimina la tabla `Usuario_Rol` y agrega la columna `IdRol` a `Usuario`). |
   | 5 | `Script_Quitar_Activo_FamiliaRol.sql` | Elimina la columna `Activo` de las tablas `Familia` y `Rol` (al pasar a eliminación física). |

   Cada script es **idempotente**: puede ejecutarse varias veces sin generar errores.

4. **Configurar la cadena de conexión** abriendo el archivo `DAL/DAO_AccesoDatos.cs` y ajustando el campo `_cadenaConexion` según tu instalación de SQL Server:

   ```csharp
   _cadenaConexion = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=CineGestDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
   ```

   - Para SQL Server Express local con instancia nombrada: `Data Source=localhost\SQLEXPRESS`
   - Para SQL Server Express con instancia default: `Data Source=.` o `Data Source=localhost`

5. **Abrir la solución** `CineGest.slnx` en Visual Studio 2022.

6. **Compilar la solución** (Build → Rebuild Solution).

7. **Ejecutar** estableciendo el proyecto `UI` como proyecto de inicio (Set as Startup Project) y presionando F5.

---

## Credenciales iniciales

El usuario administrador se crea automáticamente al ejecutar `CineGestDB.sql`:

| Campo | Valor |
|---|---|
| **Email** | `admin1@cinegest.com` |
| **Contraseña inicial** | (definida por el equipo de desarrollo — solicitar a los integrantes) |

**Nota**: Al primer ingreso, el sistema sugerirá cambiar la contraseña por motivos de seguridad.

---

## Estructura de carpetas

```
CineGest/
├── UI/                     # Proyecto Windows Forms (presentación)
│   ├── Recursos/Idiomas/   # Archivos JSON de traducción (es.json, en.json)
│   └── *.cs, *.Designer.cs # Formularios
├── BLL/                    # Proyecto lógica de negocio
├── DAL/                    # Proyecto acceso a datos
├── Servicios/              # Proyecto entidades y servicios transversales
├── *.sql                   # Scripts de creación y migración de base de datos
├── CineGest.slnx           # Solución de Visual Studio
└── LEEME.md                # Este archivo
```

---

## Consideraciones para la evaluación

- El sistema está pensado para ejecutarse en **entorno local** (PC standalone con SQL Server local).
- Las contraseñas se almacenan únicamente como **hash SHA-256**, nunca en texto plano.
- La aplicación cuenta con soporte multi-idioma (Español / Inglés). El idioma del usuario se persiste al cerrar sesión.
- El módulo de Bitácora registra automáticamente las operaciones sensibles del sistema. Su consulta es accesible desde el menú **Administrador → Bitácora Eventos** y permite filtrar y exportar los resultados a PDF.
- El modelo RBAC implementa el **patrón Composite recursivo**: un Rol puede contener Familias, una Familia puede contener otras Familias y Permisos Simples. La eliminación física de un Rol o Familia solo se permite si la entidad NO está en uso por otra Familia/Rol o Usuario.
- Cada usuario tiene **exactamente un Rol asignado** (relación 1:N entre Rol y Usuario), gestionado a través de la columna `IdRol` en la tabla `Usuario`.

---

## Soporte y contacto

Ante cualquier consulta sobre la corrección o evaluación del proyecto, contactar a:

- Rodríguez Leonel Jesús
- Riccio Sebastián Gael

---

*Documento generado para la entrega académica del proyecto CineGest — Ingeniería de Software, UAI 2026.*
