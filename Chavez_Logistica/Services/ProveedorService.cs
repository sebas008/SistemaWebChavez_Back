using Chavez_Logistica.Dtos.Maestros.Proveedor;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _repo;
    public ProveedorService(IProveedorRepository repo) => _repo = repo;

    public async Task<List<ProveedorDto>> ListAsync(bool? soloActivos, CancellationToken ct)
        => (await _repo.ListAsync(soloActivos, ct)).Select(Map).ToList();

    public async Task<ProveedorDto?> GetByIdAsync(int idProveedor, CancellationToken ct)
    {
        var row = await _repo.GetByIdAsync(idProveedor, ct);
        return row == null ? null : Map(row);
    }

    public async Task<ProveedorCreateResponseDto> CrearAsync(ProveedorCreateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.RazonSocial)) throw new ArgumentException("RazonSocial es obligatoria.");
        var entity = new Proveedor {
            Ruc = string.IsNullOrWhiteSpace(req.Ruc)? null : req.Ruc.Trim(),
            RazonSocial = req.RazonSocial.Trim(),
            Email = string.IsNullOrWhiteSpace(req.Email)? null : req.Email.Trim(),
            Telefono = string.IsNullOrWhiteSpace(req.Telefono)? null : req.Telefono.Trim(),
            Activo = true
        };
        var id = await _repo.CrearAsync(entity, ct);
        return new ProveedorCreateResponseDto { IdProveedor = id };
    }

    public async Task ActualizarAsync(int idProveedor, ProveedorUpdateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.RazonSocial)) throw new ArgumentException("RazonSocial es obligatoria.");
        var entity = new Proveedor {
            Ruc = string.IsNullOrWhiteSpace(req.Ruc)? null : req.Ruc.Trim(),
            RazonSocial = req.RazonSocial.Trim(),
            Email = string.IsNullOrWhiteSpace(req.Email)? null : req.Email.Trim(),
            Telefono = string.IsNullOrWhiteSpace(req.Telefono)? null : req.Telefono.Trim(),
            Activo = req.Activo
        };
        await _repo.ActualizarAsync(idProveedor, entity, ct);
    }

    private static ProveedorDto Map(Proveedor p) => new() { IdProveedor=p.IdProveedor, Ruc=p.Ruc, RazonSocial=p.RazonSocial, Email=p.Email, Telefono=p.Telefono, Activo=p.Activo };
}
