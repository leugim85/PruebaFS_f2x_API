using F2xF2xFullStackAssesment.Domain.Entities;
using System;

namespace F2xFullStackAssesment.Domain.IRepository
{
    public interface IVehicleCounterRepository : IERepository<Guid, VehicleCounterInformation>
    {

    }
}
