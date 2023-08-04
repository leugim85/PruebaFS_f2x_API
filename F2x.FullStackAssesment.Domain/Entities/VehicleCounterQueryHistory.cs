using F2xF2xFullStackAssesment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Domain.Entities
{

    [Table(name: "TblVehicleCounterQueryHistory")]
    public class VehicleCounterQueryHistory: EntityBase<Guid>
    {
        [Column(name: "intRegisters")]
        public int Quantity { get; set; }

        [Column(name: "dtDate")]
        public DateTime Date { get; set; }
    }
}
