using Chavez_Logistica.Interfaces;
using Chavez_Logistica.Repositorys;
using Chavez_Logistica.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string AngularCorsPolicy = "AngularCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AngularCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IInventarioService, InventarioService>();

builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<ICompraService, CompraService>();

builder.Services.AddScoped<IOrdenFinalRepository, OrdenFinalRepository>();
builder.Services.AddScoped<IOrdenFinalService, OrdenFinalService>();

builder.Services.AddScoped<IRecepcionCompraRepository, RecepcionCompraRepository>();
builder.Services.AddScoped<IRecepcionCompraService, RecepcionCompraService>();

builder.Services.AddScoped<IRecepcionObraRepository, RecepcionObraRepository>();
builder.Services.AddScoped<IRecepcionObraService, RecepcionObraService>();

builder.Services.AddScoped<IAtencionRepository, AtencionRepository>();
builder.Services.AddScoped<IAtencionService, AtencionService>();

builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();

builder.Services.AddScoped<IObraRepository, ObraRepository>();
builder.Services.AddScoped<IObraService, ObraService>();

builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

builder.Services.AddScoped<IPartidaRepository, PartidaRepository>();
builder.Services.AddScoped<IPartidaService, PartidaService>();

builder.Services.AddScoped<IRequerimientoRepository, RequerimientoRepository>();
builder.Services.AddScoped<IRequerimientoService, RequerimientoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AngularCorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();
