using System;
using System.Linq;
using Microsoft.Azure.Cosmos;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using System.Text.Json;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;

namespace VirtualGarage.VehicleSpecs.Persistence.Repositories;

public class VehicleSpecsRepository : IVehicleSpecsRepository
{
    private readonly Container _container;

    public VehicleSpecsRepository(
        CosmosClient client,
        string databaseId,
        string containerId)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        var database = client.CreateDatabaseIfNotExistsAsync(databaseId)
                             .GetAwaiter().GetResult()
                             .Database;

        _container = database.CreateContainerIfNotExistsAsync(
            containerId,
            "/id"
        ).GetAwaiter().GetResult().Container;
    }
    public async Task<CarSpecs?> GetAsync(string id, string make)
    {
    if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(make))
        return null;

    try
    {
        var response = await _container.ReadItemAsync<CarSpecs>(
            id,
            new PartitionKey(id)
        );

        return response.Resource;
    }
    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        return null;
    }
    }
    public async Task SaveAsync(CarSpecs specs)
    {
        System.Console.WriteLine(JsonSerializer.Serialize(specs));
        if (specs == null) throw new ArgumentNullException(nameof(specs));

        await _container.UpsertItemAsync(
            specs,
            new PartitionKey(specs.Id)
        );
    }
}