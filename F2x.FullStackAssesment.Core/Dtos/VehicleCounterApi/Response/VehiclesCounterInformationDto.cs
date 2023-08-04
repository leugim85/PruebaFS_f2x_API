using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace F2xFullStackAssesment.Core.Dtos.Response
{

    public class VehiclesCounterInformationDto
    {

        public string Estacion { get; set; }

        public string Sentido { get; set; }

        public string Categoria { get; set; }

        public int Cantidad { get; set; }

        public int Hora { get; set; }
    }
}
