using UdlaActivos.Web.Abstractions;

namespace UdlaActivos.Web.Data;

public sealed class MockAssetsData : IAssetsData
{
    private readonly List<AssetDto> _data = new()
    {
        new("CPU-1001","Computadora","Dell Optiplex 7090","DLLXPS7090123","Ana María Ariza","UDLA Granados","Asignado"),
        new("IMP-3001","Impresora","HP LaserJet Pro M404dn","HPLJ404DN101","—","UDLA Granados","Disponible"),
        new("CPU-1003","Computadora","Apple iMac 27\"","APLIMAC27202","Juan Pérez","UDLA Colón","Mantenimiento"),
    };

    public Task<PagedResult<AssetDto>> GetAsync(AssetsQuery q, CancellationToken ct = default)
    {
        IEnumerable<AssetDto> s = _data;

        if (q.State is "Asignados" or "Disponibles" or "En Mantenimiento")
        {
            var wanted = q.State == "En Mantenimiento" ? "Mantenimiento" : q.State.TrimEnd('s');
            s = s.Where(a => a.Status.Equals(wanted, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(q.Text))
        {
            var t = q.Text.Trim();
            s = s.Where(a =>
                a.Code.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                a.Type.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                a.BrandModel.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                a.Serial.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                a.Responsible.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                a.Site.Contains(t, StringComparison.OrdinalIgnoreCase));
        }

        var ordered = s.OrderBy(a => a.Code);
        var total = ordered.Count();
        var page = ordered.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize).ToList();

        return Task.FromResult(new PagedResult<AssetDto>(page, total, q.Page, q.PageSize));
    }

    public Task CreateAsync(AssetDto dto, CancellationToken ct = default)
    {
        _data.Add(dto with { Code = string.IsNullOrWhiteSpace(dto.Code) ? $"NEW-{_data.Count + 1:0000}" : dto.Code });
        return Task.CompletedTask;
    }

    public Task<AssetDetailDto?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        // Busca en la lista base para validar existencia
        var row = _data.FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        if (row is null) return Task.FromResult<AssetDetailDto?>(null);

        var detail = new AssetDetailDto(
            Code: row.Code,
            Title: row.BrandModel,
            Type: row.Type,
            Brand: row.BrandModel.Split(' ').FirstOrDefault() ?? "",
            Model: string.Join(' ', row.BrandModel.Split(' ').Skip(1)),
            Serial: row.Serial,
            Status: row.Status,
            PurchaseDate: new DateTime(2023, 05, 15),
            Description: "Equipo de escritorio con procesador Intel Core i7, 16GB RAM, 512GB SSD, Windows 11 Pro.",
            Notes: "Equipo en excelentes condiciones. Se realizó actualización de sistema operativo en marzo 2025.",
            AssignedTo: "Ana María Ariza",
            AssignedRole: "Soporte Técnico",
            Site: "UDLA Granados",
            AssignedAt: new DateTime(2023, 06, 01),
            LastMaintenance: new DateTime(2025, 03, 10),
            DeliveryDocUrl: "#", // mock
            History: new List<HistoryItemDto>{
            new("Mantenimiento", new DateTime(2025,03,10), "Mantenimiento preventivo y actualización de sistema operativo", "Roberto Sánchez", "TKT-4325"),
            new("Mantenimiento", new DateTime(2024,09,15), "Limpieza interna y actualización de drivers", "María López", "TKT-3856"),
            new("Asignación",    new DateTime(2023,06,01), "Asignación a Ana María Ariza", "Carlos Mendoza", "TKT-2541"),
            new("Ingreso",       new DateTime(2023,05,20), "Registro inicial en el sistema", "Carlos Mendoza", "TKT-2530")
            }
        );

        return Task.FromResult<AssetDetailDto?>(detail);
    }

}
