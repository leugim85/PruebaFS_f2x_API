
using F2xF2xFullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Domain.IRepository;
using System;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Domain.Repository
{
    public class VehicleCounterRepository : ERepository<Guid, VehicleCounterInformation>, IVehicleCounterRepository
    {
        public VehicleCounterRepository(IQueryableUnitOfWork queryableUnitOfWork)
            : base(queryableUnitOfWork)
        {

        }
    }
}
