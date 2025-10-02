using UdlaActivos.Web.Abstractions;

namespace UdlaActivos.Web.Services;

public sealed class AssetsService(IAssetsData data)
{
    public Task<PagedResult<AssetDto>> GetAsync(string? text, string state, int page, int pageSize, CancellationToken ct = default)
        => data.GetAsync(new AssetsQuery(text, state, page, pageSize), ct);

    public Task CreateAsync(AssetDto dto, CancellationToken ct = default)
        => data.CreateAsync(dto, ct);

    public Task<AssetDetailDto?> GetDetailAsync(string code, CancellationToken ct = default)
    => data.GetByCodeAsync(code, ct);

}
