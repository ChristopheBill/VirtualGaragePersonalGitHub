using System;
using System.Linq;
using Microsoft.Azure.Cosmos;
using VirtualGarage.VehicleSpecs.Persistence.Entities;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;

namespace VirtualGarage.VehicleSpecs.Persistence.Repositories;

public class VehicleSpecsRepository : IVehicleSpecsRepository
{
    private readonly Container _container;

    public VehicleSpecsRepository(CosmosClient client, string databaseId, string containerId)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (string.IsNullOrWhiteSpace(databaseId)) throw new ArgumentNullException(nameof(databaseId));
        if (string.IsNullOrWhiteSpace(containerId)) throw new ArgumentNullException(nameof(containerId));

        var dbResponse = client.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
        var containerResponse = dbResponse.Database.CreateContainerIfNotExistsAsync(containerId, "/PartitionKey").GetAwaiter().GetResult();
        _container = containerResponse.Container;
    }

    public async Task<CarSpecs?> GetAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;

        var pk = id.Split('-', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (pk == null) return null;

        try
        {
            var response = await _container.ReadItemAsync<CarSpecs>(id, new PartitionKey(pk));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task SaveAsync(CarSpecs specs)
    {
        if (specs == null) throw new ArgumentNullException(nameof(specs));

        await _container.UpsertItemAsync(specs, new PartitionKey(specs.PartitionKey));
    }
}
