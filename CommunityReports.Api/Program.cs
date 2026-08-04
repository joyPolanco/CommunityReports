using System.Text;
using CommunityReports.Api.Middleware;
using CommunityReports.Application.Interfaces;
using CommunityReports.Application.Services;
using CommunityReports.Application.Validators;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Identity;
using CommunityReports.Infrastructure.Persistence;
using CommunityReports.Infrastructure.Repositories;
using CommunityReports.Infrastructure.Security;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ---- Persistencia (PostgreSQL) -------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- Opciones ---------------------------------------------------------------
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

// ---- Identity (autenticación, cuentas y roles: Admin/Ciudadano/Empleado) -------
// AddIdentityCore (no AddIdentity) porque esto es una Api pura: no queremos el
// esquema de cookies que agrega AddIdentity, solo UserManager/RoleManager y el
// hashing de contraseñas; la autenticación real la resuelve JWT Bearer más abajo.
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        // Reglas de contraseña. Mantenidas alineadas con Usuario.ValidarPassword
        // del modelo anterior (mínimo 8 caracteres) pero usando la validación
        // nativa de Identity en vez de una regla propia.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;

        // Bloqueo por intentos fallidos (además del bloqueo manual usado por
        // Activar/Desactivar cuenta).
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ---- Inyección de dependencias: módulo de usuarios (identidad + dominio) ------
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICiudadanoRepository, CiudadanoRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// ---- Inyección de dependencias: territorio, infraestructura y catálogos --------
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();
builder.Services.AddScoped<IInfraestructuraRepository, InfraestructuraRepository>();
builder.Services.AddScoped<IInfraestructuraService, InfraestructuraService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IInstitucionRepository, InstitucionRepository>();
builder.Services.AddScoped<IInstitucionService, InstitucionService>();

// ---- Validación ---------------------------------------------------------------
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCiudadanoValidator>();

// ---- Autenticación JWT ----------------------------------------------------------
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("La sección Jwt no está configurada en appsettings.json.");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

// ---- Controladores + OpenApi ----------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Aplica migraciones pendientes y siembra roles/Admin al iniciar (conveniente para
// desarrollo/Docker). En producción muchos equipos prefieren correr
// `dotnet ef database update` como paso de despliegue separado; se deja así por
// simplicidad para este MVP.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");
    await IdentitySeeder.SeedAsync(scope.ServiceProvider, app.Configuration, logger);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
