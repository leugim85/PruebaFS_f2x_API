using F2x.FullStackAssesment.Domain.Entities;
using F2xF2xFullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Domain.Repository;
using System;

namespace F2xFullStackAssesment.Domain.IRepository
{
    public interface IVehicleCounterQueryHistoryRepository : IERepository<Guid, VehicleCounterQueryHistory>
    {

    }
}
