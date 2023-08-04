
using F2x.FullStackAssesment.Domain.Entities;
using F2xF2xFullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Domain.IRepository;
using System;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Domain.Repository
{
    public class VehicleCounterQueryHistoryRepository : ERepository<Guid, VehicleCounterQueryHistory>, IVehicleCounterQueryHistoryRepository
    {
        public VehicleCounterQueryHistoryRepository(IQueryableUnitOfWork queryableUnitOfWork)
            : base(queryableUnitOfWork)
        {

        }
    }
}
