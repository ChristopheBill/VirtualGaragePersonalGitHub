using System;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Persistence.Entities;

namespace VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;

public interface ISpecsService
{
    Task<CarSpecsResponse> LookupAsync(CarSpecsLookupRequest request);
}
