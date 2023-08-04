using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.VehicleCounterApi.Response
{
    public class VehicleCounterWithAmountDto
    {
        public string Estacion { get; set; }

        public string Sentido { get; set; }

        public string Categoria { get; set; }

        public double valorTabulado { get; set; }

        public int Hora { get; set; }
    }
}
