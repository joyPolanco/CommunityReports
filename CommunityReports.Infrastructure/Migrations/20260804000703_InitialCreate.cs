using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommunityReports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tiempo_respuesta = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoria", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "institucion",
                columns: table => new
                {
                    id_institucion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    siglas = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    tipo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    correo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    sitio_web = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institucion", x => x.id_institucion);
                });

            migrationBuilder.CreateTable(
                name: "provincia",
                columns: table => new
                {
                    id_provincia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincia", x => x.id_provincia);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nombre_normalizado = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_registro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ultimo_acceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    nombre_usuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nombre_usuario_normalizado = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    correo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    correo_normalizado = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    correo_confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    telefono_confirmado = table.Column<bool>(type: "boolean", nullable: false),
                    doble_factor_habilitado = table.Column<bool>(type: "boolean", nullable: false),
                    bloqueo_hasta = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    bloqueo_habilitado = table.Column<bool>(type: "boolean", nullable: false),
                    intentos_fallidos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "municipio",
                columns: table => new
                {
                    id_municipio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_provincia = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipio", x => x.id_municipio);
                    table.ForeignKey(
                        name: "FK_municipio_provincia_id_provincia",
                        column: x => x.id_provincia,
                        principalTable: "provincia",
                        principalColumn: "id_provincia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rol_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_rol = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_claim", x => x.id);
                    table.ForeignKey(
                        name: "FK_rol_claim_rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ciudadano",
                columns: table => new
                {
                    id_ciudadano = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    cedula = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    nombres = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    apellidos = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    foto = table.Column<string>(type: "text", nullable: true),
                    nivel_confiabilidad = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)3)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ciudadano", x => x.id_ciudadano);
                    table.ForeignKey(
                        name: "FK_ciudadano_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "empleado",
                columns: table => new
                {
                    id_empleado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_institucion = table.Column<int>(type: "integer", nullable: false),
                    cargo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    codigo_empleado = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleado", x => x.id_empleado);
                    table.ForeignKey(
                        name: "FK_empleado_institucion_id_institucion",
                        column: x => x.id_institucion,
                        principalTable: "institucion",
                        principalColumn: "id_institucion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_empleado_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_claim", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_claim_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_login",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_usuario_login_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_rol",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_rol", x => new { x.id_usuario, x.id_rol });
                    table.ForeignKey(
                        name: "FK_usuario_rol_rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_rol_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_token",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_token", x => new { x.id_usuario, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_usuario_token_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sector",
                columns: table => new
                {
                    id_sector = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_municipio = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sector", x => x.id_sector);
                    table.ForeignKey(
                        name: "FK_sector_municipio_id_municipio",
                        column: x => x.id_municipio,
                        principalTable: "municipio",
                        principalColumn: "id_municipio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "direccion",
                columns: table => new
                {
                    id_direccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_sector = table.Column<int>(type: "integer", nullable: false),
                    calle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    referencia = table.Column<string>(type: "text", nullable: true),
                    codigo_postal = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    latitud = table.Column<decimal>(type: "numeric(10,8)", nullable: true),
                    longitud = table.Column<decimal>(type: "numeric(11,8)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_direccion", x => x.id_direccion);
                    table.ForeignKey(
                        name: "FK_direccion_sector_id_sector",
                        column: x => x.id_sector,
                        principalTable: "sector",
                        principalColumn: "id_sector",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "infraestructura",
                columns: table => new
                {
                    id_infraestructura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    id_direccion = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    codigo = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_infraestructura", x => x.id_infraestructura);
                    table.ForeignKey(
                        name: "FK_infraestructura_direccion_id_direccion",
                        column: x => x.id_direccion,
                        principalTable: "direccion",
                        principalColumn: "id_direccion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categoria_nombre",
                table: "categoria",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ciudadano_cedula",
                table: "ciudadano",
                column: "cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ciudadano_id_usuario",
                table: "ciudadano",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_direccion_id_sector",
                table: "direccion",
                column: "id_sector");

            migrationBuilder.CreateIndex(
                name: "IX_empleado_codigo_empleado",
                table: "empleado",
                column: "codigo_empleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_empleado_id_institucion",
                table: "empleado",
                column: "id_institucion");

            migrationBuilder.CreateIndex(
                name: "IX_empleado_id_usuario",
                table: "empleado",
                column: "id_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_infraestructura_codigo",
                table: "infraestructura",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_infraestructura_id_direccion",
                table: "infraestructura",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_institucion_nombre",
                table: "institucion",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipio_id_provincia",
                table: "municipio",
                column: "id_provincia");

            migrationBuilder.CreateIndex(
                name: "IX_provincia_nombre",
                table: "provincia",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "rol",
                column: "nombre_normalizado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rol_claim_id_rol",
                table: "rol_claim",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_sector_id_municipio",
                table: "sector",
                column: "id_municipio");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "usuario",
                column: "correo_normalizado");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "usuario",
                column: "nombre_usuario_normalizado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_claim_id_usuario",
                table: "usuario_claim",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_login_id_usuario",
                table: "usuario_login",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_rol_id_rol",
                table: "usuario_rol",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "ciudadano");

            migrationBuilder.DropTable(
                name: "empleado");

            migrationBuilder.DropTable(
                name: "infraestructura");

            migrationBuilder.DropTable(
                name: "rol_claim");

            migrationBuilder.DropTable(
                name: "usuario_claim");

            migrationBuilder.DropTable(
                name: "usuario_login");

            migrationBuilder.DropTable(
                name: "usuario_rol");

            migrationBuilder.DropTable(
                name: "usuario_token");

            migrationBuilder.DropTable(
                name: "institucion");

            migrationBuilder.DropTable(
                name: "direccion");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "sector");

            migrationBuilder.DropTable(
                name: "municipio");

            migrationBuilder.DropTable(
                name: "provincia");
        }
    }
}
