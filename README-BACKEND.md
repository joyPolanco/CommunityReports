# Módulo implementado: usuarios (Identity) + territorio + infraestructura + catálogos

Este documento describe todo lo implementado hasta ahora, siguiendo Arquitectura
Onion con PostgreSQL + EF Core, cubriendo el diagrama ER completo que compartiste
(excepto `incidencia`, `reporte_ciudadano`, `comentario`, `asignacion`, `evidencia`
y `validacion`, que no aparecían en esa captura y quedan fuera de este alcance).

## ⚠️ Actualización: autenticación migrada a ASP.NET Core Identity

La autenticación ya no es una jerarquía `Usuario`/`Ciudadano`/`Empleado` con hashing
propio (BCrypt): ahora usa **ASP.NET Core Identity** sobre PostgreSQL, con 3 roles
reales (`Admin`, `Ciudadano`, `Empleado`). Resumen del rediseño:

- **Identity concentra TODO lo de cuenta**: `ApplicationUser : IdentityUser<int>`
  (en `CommunityReports.Infrastructure/Identity/`) reemplaza a la antigua clase
  abstracta `Usuario`. Contiene nombre de usuario, correo, hash de contraseña
  (hasheado por Identity, no por un `IPasswordHasher` propio), bloqueo de cuenta
  (`LockoutEnabled`/`LockoutEnd`, usado para Activar/Desactivar) y dos campos
  propios: `FechaRegistro`, `UltimoAcceso`.
- **`Ciudadano` y `Empleado` vuelven a ser dominio puro**: ya no heredan de
  `Usuario` (se eliminó esa clase). Cada uno tiene su propio `Id` (PK propia) y un
  `UsuarioId` (FK simple hacia Identity, no una navegación) con sus propios campos
  de negocio (cédula/nivel de confiabilidad; institución/cargo/código). El dominio
  conoce el id del usuario, no conoce Identity.
- **`Admin` es solo un rol de Identity**, sin perfil de dominio: no hay tabla
  `admin` ni endpoint de auto-registro para ese rol (se siembra al iniciar, ver
  `Seed:AdminEmail`/`Seed:AdminPassword` en `appsettings.json`, o se asciende un
  usuario manualmente).
- **Abstracción profesional**: Application y Domain **nunca referencian** tipos de
  Identity (`UserManager`, `IdentityUser`, etc.). Todo pasa por
  `IIdentityService` (interfaz en `Application/Interfaces`, implementada en
  `Infrastructure/Identity/IdentityService.cs`), que expone únicamente DTOs propios
  (`UsuarioIdentityDto`, `IdentityOperationResult`, `TokenClaimsData`).
- **Tablas de Identity renombradas** a snake_case en español para que convivan con
  el resto del esquema: `AspNetUsers → usuario`, `AspNetRoles → rol`,
  `AspNetUserRoles → usuario_rol`, `AspNetUserClaims → usuario_claim`,
  `AspNetRoleClaims → rol_claim`, `AspNetUserLogins → usuario_login`,
  `AspNetUserTokens → usuario_token` (ver `IdentityTableNamesConfiguration.cs`).
- **`RoleNames`** (`CommunityReports.Domain/Constants/RoleNames.cs`) centraliza los
  3 nombres de rol (`Admin`, `Ciudadano`, `Empleado`) como `const string`, usables
  tanto en `[Authorize(Roles = ...)]` como al sembrar roles.

### Cómo levantar el proyecto desde cero

```bash
# 1) Base de datos
docker compose up -d

# 2) Restaurar paquetes (requiere acceso a NuGet)
dotnet restore

# 3) Generar la migración inicial (Identity + dominio; no existía ninguna aún)
dotnet ef migrations add InitialCreate -p CommunityReports.Infrastructure -s CommunityReports.Api

# 4) Aplicar migraciones (Program.cs también las aplica solo al arrancar)
dotnet ef database update -p CommunityReports.Infrastructure -s CommunityReports.Api

# 5) Correr la Api
dotnet run --project CommunityReports.Api
```

Al iniciar, `IdentitySeeder` crea los 3 roles y, si `Seed:AdminEmail` /
`Seed:AdminPassword` están configurados (ya vienen con valores de ejemplo en
`appsettings.json` — **cámbialos antes de producción**), crea un Admin inicial.

### Flujo de autenticación

- `POST /api/auth/register/ciudadano` y `POST /api/auth/register/empleado`: crean
  el usuario de Identity + el perfil de dominio en un solo paso; si el perfil de
  dominio falla al guardarse (ej. cédula duplicada detectada por una carrera),
  se revierte el usuario de Identity para no dejar cuentas huérfanas.
- `POST /api/auth/login`: valida credenciales vía Identity, emite un JWT con el
  id, nombre, correo y **todos los roles** del usuario como claims, y devuelve el
  perfil combinado (dominio + cuenta) usando el discriminador polimórfico `rol`
  (`"ciudadano"`, `"empleado"` o `"admin"`).
- No existe (a propósito) un endpoint público para registrar Admins.

## Entidades implementadas

- **Usuarios**: `Ciudadano`, `Empleado` (perfiles de dominio) + `ApplicationUser`
  (cuenta, Identity) — ver sección de arriba.
- **Territorio**: `Provincia` → `Municipio` → `Sector` → `Direccion` (jerarquía
  1 a N encadenada).
- **Infraestructura**: `TipoInfraestructura` → `Infraestructura` (clasificada por
  tipo y ubicada en una `Direccion`).
- **Catálogos**: `Categoria` (con color y tiempo de respuesta), `Estado`.
- **Institucion**: catálogo con datos de contacto, referenciado por `Empleado`.

## Decisiones de modelado (nivel más técnico)

1. **Autenticación vs. dominio, separados**: ver sección de Identity arriba. Antes
   había herencia real Table-Per-Type (`Ciudadano : Usuario`); ahora `Ciudadano` y
   `Empleado` son entidades independientes con una FK simple (`UsuarioId`) hacia
   el usuario de Identity, que vive en su propia tabla ("usuario", generada y
   gestionada por Identity).

2. **`CatalogoBase`**: clase abstracta compartida por `Provincia`, `Municipio`,
   `Sector`, `TipoInfraestructura`, `Categoria`, `Estado` e `Institucion`. No es
   una jerarquía de tablas (cada una vive en su propia tabla independiente, igual
   que en el diagrama) — es reutilización de código: centraliza la validación de
   `Nombre` para que ninguna de estas entidades sea anémica ni repita el mismo
   `if (string.IsNullOrWhiteSpace(...))` siete veces.

3. **`Direccion` e `Infraestructura`** no heredan de `CatalogoBase` porque no son
   "un nombre" — tienen su propia identidad (coordenadas, código), así que quedan
   como entidades independientes bajo `BaseEntity`.

## Estructura por capa

- **Domain**: entidades ya mencionadas + `ICiudadanoRepository`, `IEmpleadoRepository`,
  `IUbicacionRepository` (unifica Provincia/Municipio/Sector/Direccion),
  `IInfraestructuraRepository`, `ICategoriaRepository`, `IEstadoRepository`,
  `IInstitucionRepository`; `Constants/RoleNames` con los 3 roles del sistema.
- **Application**: DTOs de request/response por módulo (incluye
  `DTOs/Identity/*` para el contrato con Identity), interfaces de servicio
  (incluye `IIdentityService`, el puerto hacia Identity), validadores
  FluentValidation, jerarquía de excepciones, y los servicios: `AuthService`,
  `UserService`, `UbicacionService`, `InfraestructuraService`, `CategoriaService`,
  `EstadoService`, `InstitucionService`.
- **Infrastructure**: `ApplicationDbContext` (extiende `IdentityDbContext`) con
  todas las configuraciones EF Core, repositorios para cada módulo,
  `Identity/ApplicationUser`, `Identity/IdentityService` (implementa
  `IIdentityService` sobre `UserManager`/`RoleManager`), `Identity/IdentitySeeder`,
  `JwtTokenGenerator`.
- **Api**: `AuthController`, `UserController`, `UbicacionController`,
  `InfraestructuraController`, `CategoriaController`, `EstadoController`,
  `InstitucionController`, middleware global de excepciones, `Program.cs` con
  toda la inyección de dependencias (incluye `AddIdentityCore` + seeding de roles).

## Refactor: catálogos simples → enums

Se convirtieron `Estado` y `TipoInfraestructura` de tablas a **enums de C#**
(`Domain.Enums.EstadoIncidencia`, `Domain.Enums.TipoInfraestructura`), eliminando
sus entidades, repositorios, servicios, controladores y configuraciones EF.

**Criterio usado** (para no eliminar donde no correspondía):
- Se vuelven enum los catálogos que son **listas fijas y cerradas de solo-nombre**,
  sin datos propios que un administrador necesite editar en producción.
- **Se mantienen como tabla** `Categoria` (tiene `color` y `tiempo_respuesta`,
  datos que un administrador sí necesita poder ajustar sin recompilar) e
  `Institucion` (crece con el tiempo — nuevas instituciones se dan de alta en
  producción, no en código). `Provincia`/`Municipio`/`Sector` también quedan como
  tablas: son decenas/cientos de filas con jerarquía real, no un enum viable.

`Infraestructura.Tipo` ahora es el enum directamente (columna `tipo` guardada como
texto en la BD para que sea legible). `GET /api/infraestructuras/tipos` sigue
existiendo, pero ahora solo devuelve `Enum.GetNames<TipoInfraestructura>()` — sin
tabla ni round-trip a la base de datos.

> Nota: ya existía un stub vacío `ReportStatus.cs` en `Domain/Enums` (parte del
> módulo de incidencias, fuera de este alcance). Cuando se implemente ese módulo,
> conviene unificarlo con `EstadoIncidencia` en vez de duplicar el concepto.

## Cómo correrlo

Necesitas tener instalado el **SDK de .NET 10** (para compilar/correr el código) y
**Docker** (para la base de datos). No hay forma de evitar instalar el SDK de
.NET: es lo que compila el proyecto, así como necesitas Node para correr un
proyecto de JavaScript.

### Paso 1 — Levantar PostgreSQL con Docker

```bash
docker compose up -d
```

Esto levanta *solo* la base de datos (Postgres en el puerto 5432, usuario
`postgres`, password `postgres`, base `community_reports` — ver `docker-compose.yml`).

### Paso 2 — Generar y aplicar la migración inicial (una sola vez)

Necesitas la herramienta `dotnet-ef` (se instala una vez, globalmente):

```bash
dotnet tool install --global dotnet-ef
```

Luego, desde la carpeta raíz del proyecto (donde está `CommunityReports.slnx`):

```bash
dotnet restore
dotnet ef migrations add InitialCreate --project CommunityReports.Infrastructure --startup-project CommunityReports.Api
```

Esto genera una carpeta `Migrations/` dentro de `CommunityReports.Infrastructure`
con el código que crea las tablas. **No necesitas correr `database update`
manualmente** — `Program.cs` ya llama a `db.Database.Migrate()` al iniciar, así
que la próxima vez que corras la Api, las tablas se crean/actualizan solas.

### Paso 3 — Ajustar el secreto de JWT

Antes de correr en serio, cambiá `Jwt:Key` en `CommunityReports.Api/appsettings.json`
por un secreto real de al menos 32 caracteres (el que viene es un placeholder).

### Paso 4 — Correr la Api

```bash
dotnet run --project CommunityReports.Api
```

Con eso ya tenés la Api corriendo (por defecto en `https://localhost:xxxx`, el
puerto exacto lo imprime la consola) contra la base de datos en Docker. Probá
`POST /api/auth/register/ciudadano` para confirmar que todo conecta.

### Alternativa: dockerizar también la Api

Si preferís no instalar el SDK de .NET localmente, hay un `Dockerfile` +
`docker-compose.full.yml` que levantan Postgres **y** la Api dentro de Docker:

```bash
docker compose -f docker-compose.full.yml up --build
```

Igual necesitás generar la migración inicial una vez (Paso 2) **con el SDK
instalado en algún lado** — ya sea tu máquina o corriendo ese mismo comando
dentro de un contenedor `mcr.microsoft.com/dotnet/sdk:9.0` con el volumen del
proyecto montado — porque el código de la migración se genera y se commitea al
repo, no se genera en cada arranque.

> Nota: este entorno de generación de código no tiene acceso a nuget.org, por lo
> que los paquetes no fueron restaurados/compilados aquí, y el `Dockerfile`/
> `docker-compose.full.yml` no se probaron end-to-end. Si algún número de versión
> de paquete no existe al restaurar, ajustalo a la última versión estable
> disponible para .NET 10 (los nombres de paquete son correctos).

## Endpoints principales

| Módulo | Rutas |
|---|---|
| Auth | `POST /api/auth/register/ciudadano`, `POST /api/auth/register/empleado`, `POST /api/auth/login` |
| Usuarios | `GET /api/users/me`, `GET /api/users/{id}`, `GET /api/users/ciudadanos`, `GET /api/users/empleados`, `PUT /api/users/me/perfil-ciudadano`, `PUT /api/users/me/perfil-empleado`, `POST /api/users/me/cambiar-password`, `PATCH /api/users/{id}/activar`, `PATCH /api/users/{id}/desactivar` |
| Ubicación | `GET/POST /api/ubicaciones/provincias`, `GET /api/ubicaciones/provincias/{id}/municipios`, `POST /api/ubicaciones/municipios`, `GET .../municipios/{id}/sectores`, `POST /api/ubicaciones/sectores`, `GET .../sectores/{id}/direcciones`, `POST /api/ubicaciones/direcciones` |
| Infraestructura | `GET /api/infraestructuras/tipos` (enum, sin BD), `GET/POST /api/infraestructuras`, `GET /api/infraestructuras/{id}`, `PUT /api/infraestructuras/{id}`, `GET /api/infraestructuras/por-direccion/{direccionId}` |
| Categorías | `GET/POST /api/categorias`, `PUT/DELETE /api/categorias/{id}` |
| Instituciones | `GET/POST /api/instituciones`, `PUT /api/instituciones/{id}` |

Las operaciones de creación/edición/borrado de catálogos están protegidas con
`[Authorize(Roles = "Empleado")]`; las de lectura son públicas.

