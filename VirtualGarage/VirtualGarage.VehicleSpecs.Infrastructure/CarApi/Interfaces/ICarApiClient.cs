using System;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi.Contracts;

namespace VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

public interface ICarApiClient
{
    Task<CarApiCarResponse> GetCarAsync(
        string make,
        string model,
        int year);
}