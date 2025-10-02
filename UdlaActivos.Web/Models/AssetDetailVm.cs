namespace UdlaActivos.Web.Models;

public record AssetDetailVm(
    string Code, string Title, string Type, string Brand, string Model, string Serial, string Status,
    DateTime? PurchaseDate, string? Description, string? Notes,
    string AssignedTo, string AssignedRole, string Site, DateTime? AssignedAt,
    DateTime? LastMaintenance, string DeliveryDocUrl,
    List<HistoryItemVm> History
);

public record HistoryItemVm(string Kind, DateTime Date, string Title, string By, string Ref);
