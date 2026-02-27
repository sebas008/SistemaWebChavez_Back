using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces
{
    public interface IInventarioRepository
    {
        // ALMACEN
        Task<IEnumerable<Almacen>> Almacen_ListAsync(bool? soloActivos, CancellationToken ct);
        Task<Almacen?> Almacen_GetByIdAsync(int idAlmacen, CancellationToken ct);
        Task<int> Almacen_CrearAsync(Almacen entity, CancellationToken ct);
        Task Almacen_ActualizarAsync(int idAlmacen, string nombre, bool activo, CancellationToken ct);

        // ITEM
        Task<IEnumerable<Item>> Item_ListAsync(bool? soloActivos, CancellationToken ct);
        Task<Item?> Item_GetByIdAsync(int idItem, CancellationToken ct);
        Task<int> Item_CrearAsync(Item entity, CancellationToken ct);
        Task Item_ActualizarAsync(int idItem, Item entity, CancellationToken ct);

        // STOCK
        Task<IEnumerable<Stock>> Stock_ListAsync(int? idAlmacen, CancellationToken ct);

        // KARDEX
        Task<IEnumerable<Kardex>> Kardex_ListAsync(int? idAlmacen, int? idItem, DateTime? desde, DateTime? hasta, CancellationToken ct);
    }
}
