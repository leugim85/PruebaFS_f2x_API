using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.VehicleCounterApi.Response
{
    public class VehicleCounterQueryHistoryDto
    {
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
