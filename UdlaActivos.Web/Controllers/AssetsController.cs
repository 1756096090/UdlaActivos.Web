using Microsoft.AspNetCore.Mvc;

namespace UdlaActivos.Web.Controllers;

public class AssetsController : Controller
{
    // MOCK: fuente en memoria (puedes moverla a un servicio luego)
    private static readonly List<AssetViewModel> Data = new()
    {
        new("CPU-1001","Computadora","Dell Optiplex 7090","DLLXPS7090123","Ana María Ariza","UDLA Granados","Asignado"),
        new("CPU-1002","Computadora","HP EliteDesk 800 G6","HPEDS800GG6456","Carlos Mendoza","UDLA Colón","Asignado"),
        new("MON-2001","Monitor","Dell P2419H","DLLP2419H789","María González","UDLA Park","Asignado"),
        new("IMP-3001","Impresora","HP LaserJet Pro M404dn","HPLJ404DN101","—","UDLA Granados","Disponible"),
        new("CPU-1003","Computadora","Apple iMac 27\"","APLIMAC27202","Juan Pérez","UDLA Colón","Mantenimiento"),
        new("MON-2002","Monitor","LG 27UK850-W","LG27UK850303","—","UDLA Granados","Disponible"),
        new("TEC-4001","Teclado","Logitech K120","LGTK120404","Ana María Ariza","UDLA Granados","Asignado"),
    };

    public IActionResult Index(string? q, string? state = "Todos", int page = 1, int pageSize = 10)
    {
        // base query
        IEnumerable<AssetViewModel> query = Data;

        // filtro por estado
        state = string.IsNullOrWhiteSpace(state) ? "Todos" : state;
        if (state is "Asignados" or "Disponibles" or "En Mantenimiento")
        {
            var wanted = state == "En Mantenimiento" ? "Mantenimiento" : state.TrimEnd('s');
            query = query.Where(a => a.Status.Equals(wanted, StringComparison.OrdinalIgnoreCase));
        }

        // filtro por texto
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(a =>
                a.Code.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Type.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.BrandModel.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Serial.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Responsible.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Site.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        // orden y paginación
        query = query.OrderBy(a => a.Code);
        var total = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var vm = new AssetsPageVm(
            Items: items,
            Query: q ?? "",
            State: state!,
            Page: page,
            PageSize: pageSize,
            Total: total
        );

        ViewData["Title"] = "Activos";
        return View(vm);
    }
}

public record AssetViewModel(
    string Code, string Type, string BrandModel, string Serial,
    string Responsible, string Site, string Status
);

// ViewModel para la vista (lista + metadatos)
public record AssetsPageVm(
    List<AssetViewModel> Items,
    string Query,
    string State,
    int Page,
    int PageSize,
    int Total
);
