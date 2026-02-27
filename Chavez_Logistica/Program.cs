using Chavez_Logistica.Interfaces;
using Chavez_Logistica.Repositorys;
using Chavez_Logistica.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================
// SERVICES
// =====================

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (Angular)
const string AngularCorsPolicy = "AngularCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AngularCorsPolicy, policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =====================
// DEPENDENCY INJECTION
// =====================

// DB
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

// Usuarios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Auth
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Inventario
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IInventarioService, InventarioService>();

// Maestros - Unidad de Medida
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();

// Maestros - Obra
builder.Services.AddScoped<IObraRepository, ObraRepository>();
builder.Services.AddScoped<IObraService, ObraService>();

// Maestros - Proveedor
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

// Inventario - Requerimiento
builder.Services.AddScoped<IRequerimientoRepository, RequerimientoRepository>();
builder.Services.AddScoped<IRequerimientoService, RequerimientoService>();


var app = builder.Build();

// =====================
// MIDDLEWARE
// =====================

// Swagger (solo dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS antes de Authorization/Controllers
app.UseCors(AngularCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();