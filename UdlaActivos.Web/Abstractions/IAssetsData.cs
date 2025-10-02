namespace UdlaActivos.Web.Abstractions;

public record AssetDto(string Code, string Type, string BrandModel, string Serial,
                       string Responsible, string Site, string Status);

public record AssetsQuery(string? Text, string State, int Page = 1, int PageSize = 10);
public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);

/* NUEVO: contrato para detalle */
public record AssetDetailDto(
    string Code, string Title,          // "CPU-1001", "Dell Optiplex 7090"
    string Type, string Brand, string Model, string Serial, string Status,
    DateTime? PurchaseDate, string? Description, string? Notes,
    string AssignedTo, string AssignedRole, string Site, DateTime? AssignedAt,
    DateTime? LastMaintenance, string? DeliveryDocUrl,
    IReadOnlyList<HistoryItemDto> History
);

public record HistoryItemDto(string Kind, DateTime Date, string Title, string By, string Ref);

/* interfaz */
public interface IAssetsData
{
    Task<PagedResult<AssetDto>> GetAsync(AssetsQuery query, CancellationToken ct = default);
    Task CreateAsync(AssetDto dto, CancellationToken ct = default);

    // NUEVO: detalle por código
    Task<AssetDetailDto?> GetByCodeAsync(string code, CancellationToken ct = default);
}
