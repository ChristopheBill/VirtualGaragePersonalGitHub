using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Persistence.Entities;

namespace VirtualGarage.Domain.Services.Mapping
{
    internal class UserMapperExtension
    {
        public User ToEntity(UserResponseContract user)
        {
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Vehicles = user.Vehicles?.ConvertAll(v => new Vehicle
                {
                    Id = v.Id,
                    Brand = v.Brand,
                    Model = v.Model,
                    ManufactureDate = v.Year,
                    UserId = user.Id
                })
            };
        }
        public UserResponseContract ToContract(User user)
        {
            return new UserResponseContract
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Vehicles = user.Vehicles?.ConvertAll(v => new VehicleResponseContract
                {
                    Id = v.Id,
                    Brand = v.Brand,
                    Model = v.Model,
                    Year = v.ManufactureDate,
                    UserId = user.Id
                })
            };
        }
    }
}
