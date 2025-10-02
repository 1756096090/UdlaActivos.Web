using Microsoft.AspNetCore.Mvc;
using UdlaActivos.Web.Services;
using UdlaActivos.Web.Abstractions;
using UdlaActivos.Web.Models;

namespace UdlaActivos.Web.Controllers;

public class AssetsController(AssetsService svc) : Controller
{
    // Listado con filtros (q/state) y paginación
    public async Task<IActionResult> Index(string? q, string? state = "Todos", int page = 1, int pageSize = 10)
    {
        ViewData["Title"] = "Activos";

        var result = await svc.GetAsync(q, state ?? "Todos", page, pageSize);

        var vm = new AssetsPageVm(
            Items: result.Items.Select(a => new AssetViewModel(
                Code: a.Code,
                Type: a.Type,
                BrandModel: a.BrandModel,
                Serial: a.Serial,
                Responsible: a.Responsible,
                Site: a.Site,
                Status: a.Status
            )).ToList(),
            Query: q ?? "",
            State: state ?? "Todos",
            Page: result.Page,
            PageSize: result.PageSize,
            Total: result.Total
        );

        return View(vm); // usa Views/Assets/Index.cshtml que ya tienes
    }

    // GET: formulario "Nuevo Activo"
    [HttpGet]
    public IActionResult New()
    {
        ViewData["Title"] = "Nuevo Activo";
        return View(new AssetCreateVm()); // Views/Assets/New.cshtml
    }

    // POST: crear (mock o API según config)
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> New(AssetCreateVm vm)
    {
        ViewData["Title"] = "Nuevo Activo";
        if (!ModelState.IsValid) return View(vm);

        // Mapeo VM -> DTO (contrato estable)
        var dto = new AssetDto(
            Code: vm.UdlaCode,
            Type: vm.Type,
            BrandModel: $"{vm.Brand} {vm.Model}".Trim(),
            Serial: vm.Serial,
            Responsible: "—",
            Site: vm.Site,
            Status: vm.Status
        );

        await svc.CreateAsync(dto);
        TempData["Ok"] = "Activo creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(string code)
    {
        ViewData["Title"] = "Detalle de Activo";
        var dto = await svc.GetDetailAsync(code);
        if (dto is null) return NotFound();

        // Map a VM de vista
        var vm = new AssetDetailVm(
            Code: dto.Code,
            Title: dto.Title,
            Type: dto.Type,
            Brand: dto.Brand,
            Model: dto.Model,
            Serial: dto.Serial,
            Status: dto.Status,
            PurchaseDate: dto.PurchaseDate,
            Description: dto.Description,
            Notes: dto.Notes,
            AssignedTo: dto.AssignedTo,
            AssignedRole: dto.AssignedRole,
            Site: dto.Site,
            AssignedAt: dto.AssignedAt,
            LastMaintenance: dto.LastMaintenance,
            DeliveryDocUrl: dto.DeliveryDocUrl ?? "#",
            History: dto.History.Select(h => new HistoryItemVm(h.Kind, h.Date, h.Title, h.By, h.Ref)).ToList()
        );

        return View(vm); // Views/Assets/Details.cshtml
    }
}
