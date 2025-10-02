namespace UdlaActivos.Web.Models;

public record AssetsPageVm(
    List<AssetViewModel> Items,
    string Query,
    string State,
    int Page,
    int PageSize,
    int Total
);
