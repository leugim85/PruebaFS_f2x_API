using F2x.FullStackAssesment.Core.Dtos.Request;
using F2x.FullStackAssesment.Core.Dtos.Response;
using F2x.FullStackAssesment.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class VehicleCounterController : ControllerBase
    {
        private readonly IVehicleCountService vehicleCountService;

        public VehicleCounterController(IVehicleCountService vehicleCountService)
        {
            this.vehicleCountService = vehicleCountService;
        }

        [HttpPost("GetVehicleCounterInformation")]
        [Produces(typeof(VehiclesCounterCollectedPaginated))]
        public async Task<ActionResult<VehiclesCounterCollectedPaginated>> GetInvoiceInformation([FromBody] VehiclesInformationPaginatedDto request)
        {
            return Ok(await vehicleCountService.GetDataVehicleCounterPaginated(request));
        }          
    }
}
