using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _repo;
    public CompraService(ICompraRepository repo) => _repo = repo;

    public async Task<List<CompraDto>> ListAsync(int? idProveedor, int? idObra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idProveedor, idObra, estado, ct)).Select(c => new CompraDto
        {
            IdCompra = c.IdCompra,
            Codigo = c.Codigo,
            Fecha = c.Fecha,
            IdProveedor = c.IdProveedor,
            IdObra = c.IdObra,
            Estado = c.Estado,
            Observacion = c.Observacion
        }).ToList();

    public async Task<CompraDto?> GetByIdAsync(int idCompra, CancellationToken ct)
    {
        var header = await _repo.GetByIdAsync(idCompra, ct);
        if (header == null) return null;

        var detalle = await _repo.Detalle_ListByCompraAsync(idCompra, ct);

        return new CompraDto
        {
            IdCompra = header.IdCompra,
            Codigo = header.Codigo,
            Fecha = header.Fecha,
            IdProveedor = header.IdProveedor,
            IdObra = header.IdObra,
            Estado = header.Estado,
            Observacion = header.Observacion,
            Detalle = detalle.Select(d => new CompraDetalleDto
            {
                IdCompraDetalle = d.IdCompraDetalle,
                IdItem = d.IdItem,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Observacion = d.Observacion
            }).ToList()
        };
    }

    public Task<List<CompraBandejaDto>> ListarBandejaAsync(CancellationToken ct)
        => _repo.ListarBandejaAsync(ct);

    public async Task<CompraCreateResponseDto> CrearAsync(CompraCreateRequestDto req, CancellationToken ct)
    {
        if (req.IdProveedor <= 0) throw new ArgumentException("IdProveedor inválido.");
        if (req.IdObra <= 0) throw new ArgumentException("IdObra inválido.");
        if (req.Detalle == null || req.Detalle.Count == 0) throw new ArgumentException("Detalle es obligatorio.");

        var detalle = req.Detalle.Select(d => new CompraDetalle
        {
            IdItem = d.IdItem,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario,
            Observacion = string.IsNullOrWhiteSpace(d.Observacion) ? null : d.Observacion.Trim()
        });

        var result = await _repo.CrearAsync(
            req.IdProveedor,
            req.IdObra,
            string.IsNullOrWhiteSpace(req.Observacion) ? null : req.Observacion.Trim(),
            req.IdUsuario,
            detalle,
            ct);

        return new CompraCreateResponseDto
        {
            IdCompra = result.IdCompra,
            Codigo = result.Codigo
        };
    }

    public async Task CambiarEstadoAsync(int idCompra, CompraCambiarEstadoRequestDto req, CancellationToken ct)
        => await _repo.CambiarEstadoAsync(
            idCompra,
            req.Estado.Trim().ToUpperInvariant(),
            req.IdUsuario,
            string.IsNullOrWhiteSpace(req.Observacion) ? null : req.Observacion.Trim(),
            ct);
}
