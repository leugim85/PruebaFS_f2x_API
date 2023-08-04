using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.Request
{
    public class VehiclesInformationPaginatedDto
    {
        public string Station { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndtDate { get; set; }

        public int Take { get; set; }

        public int PageNumber { get; set; }
    }
}
