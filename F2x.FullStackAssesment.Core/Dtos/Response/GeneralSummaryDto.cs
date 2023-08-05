using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.Response
{
    public class GeneralSummaryDto
    {
        public List<StationSummaryDto> VehicleCounterSummaryList { get; set; } = new List<StationSummaryDto>();

        public int TotalCarsGeneral { get; set; }

        public double TotalAmountGeneral { get; set; }

    }
    public class StationSummaryDto
    {
        public string Station { get; set; }
        public int TotalAmount { get; set; }
        public int VehicleCount { get; set;}

        public List<SummaryByDateDto> SummaryByDates { get; set; } = new List<SummaryByDateDto>();
    }

    public class SummaryByDateDto
    {
        public int TotalAmountByDate { get; set; }

        public int VehicleCountByDate { get; set; }

        public DateOnly Date { get; set; }
    }
}
