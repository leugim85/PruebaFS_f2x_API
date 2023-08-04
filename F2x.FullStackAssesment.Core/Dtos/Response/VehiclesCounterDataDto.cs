using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.Response
{
    public class VehiclesCounterDataDto
    {
        public string Station { get; set; }

        public string Direction { get; set; }

        public string Hour { get; set; }

        public string Category { get; set; }

        public DateOnly Date { get; set; }

        public string Quantity { get; set; }
    }
}

