using System;
using System.Net.Http.Json;
using Duende.IdentityModel.Client;
using VirtualGarage.Domain.Models;
using VirtualGarage.QuestPDF.Infrastructure.DTOs;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure.Mapping;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace VirtualGarage.QuestPDF.Infrastructure.Clients;

public sealed class VehicleSpecsClient : IVehicleSpecsProvider
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;

    // Constructor receives an HttpClient that's configured with BaseAddress
    public VehicleSpecsClient(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<VehicleSpecs> GetSpecsAsync(string brand, string model, int year)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Brand is required", nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model is required", nameof(model));

        var url = $"api/specifications?brand={Uri.EscapeDataString(brand)}&model={Uri.EscapeDataString(model)}&year={year}";

        // Use a separate HttpClient for Identity Server requests
        using var identityClient = _httpClientFactory.CreateClient();
        
        var disco = await identityClient.GetDiscoveryDocumentAsync("https://virtualgarage-identityserver.azurewebsites.net");
        if (disco.IsError)
        {
            throw new ApplicationException($"Failed to get discovery document: {disco.Error}");
        }

        var tokenResponse = await identityClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "m2m.virtualgarage.api",
            ClientSecret = "virtualgaragesecret",
            Scope = "vehiclespecs.api"
        });

        if (tokenResponse.IsError)
        {
            throw new ApplicationException($"Failed to get access token: {tokenResponse.Error}");
        }

        // Create a new request message with the bearer token
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.SetBearerToken(tokenResponse.AccessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            // Try to extract ProblemDetails-style error message from API
            ProblemDetailsDto? problem = null;
            try
            {
                problem = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>();
            }
            catch
            {
                // ignore parse errors and fall back to status code
            }

            var message = problem?.Detail ?? $"VehicleSpecs API failed: {response.StatusCode}";
            throw new ApplicationException(message);
        }

        var dto = await response.Content.ReadFromJsonAsync<VehicleSpecsResponseDto>();

        if (dto is null)
            throw new ApplicationException("Empty VehicleSpecs response");

        return VehicleSpecsMapper.MapToDomain(dto);
    }
}

internal sealed class ProblemDetailsDto
{
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("detail")] public string? Detail { get; set; }
    [JsonPropertyName("status")] public int? Status { get; set; }
}