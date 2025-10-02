namespace UdlaActivos.Web.Models;

public record AssetViewModel(
    string Code,
    string Type,
    string BrandModel,
    string Serial,
    string Responsible,
    string Site,
    string Status
);
