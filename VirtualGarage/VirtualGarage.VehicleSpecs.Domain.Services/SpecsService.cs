using System.Net.Http.Headers;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Domain.Services.Mapping;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Domain.Services;

public class SpecsService : ISpecsService
{
    private readonly HttpClient _httpClient;

    public SpecsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CarSpecs> GetSpecsAsync(
        string make,
        string model,
        int year)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://carapi.app/api/trims?make={Uri.EscapeDataString(make)}&model={Uri.EscapeDataString(model)}&year={year}"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", "YOUR_JWT");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        // TODO: deserialize and choose trim
        return CarSpecsMappingExtension.MapToCarSpecs(json);
    }
}
