using Chavez_Logistica.Dtos.Inventario.Almacen;
using Chavez_Logistica.Dtos.Inventario.Item;
using Chavez_Logistica.Dtos.Inventario.Kardex;
using Chavez_Logistica.Dtos.Inventario.Stock;

namespace Chavez_Logistica.Interfaces
{
    public interface IInventarioService
    {
        // ALMACEN
        Task<List<AlmacenDto>> Almacen_ListAsync(bool? soloActivos, CancellationToken ct);
        Task<AlmacenDto?> Almacen_GetByIdAsync(int idAlmacen, CancellationToken ct);
        Task<AlmacenCreateResponseDto> Almacen_CrearAsync(AlmacenCreateRequestDto req, CancellationToken ct);
        Task Almacen_ActualizarAsync(int idAlmacen, AlmacenUpdateRequestDto req, CancellationToken ct);

        // ITEM
        Task<List<ItemDto>> Item_ListAsync(bool? soloActivos, CancellationToken ct);
        Task<ItemDto?> Item_GetByIdAsync(int idItem, CancellationToken ct);
        Task<ItemCreateResponseDto> Item_CrearAsync(ItemCreateRequestDto req, CancellationToken ct);
        Task Item_ActualizarAsync(int idItem, ItemUpdateRequestDto req, CancellationToken ct);

        // STOCK
        Task<List<StockDto>> Stock_ListAsync(int? idAlmacen, CancellationToken ct);

        // KARDEX
        Task<List<KardexDto>> Kardex_ListAsync(int? idAlmacen, int? idItem, DateTime? desde, DateTime? hasta, CancellationToken ct);
    }
}
