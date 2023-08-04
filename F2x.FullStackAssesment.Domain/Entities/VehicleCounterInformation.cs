using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace F2xF2xFullStackAssesment.Domain.Entities
{
    [Table(name: "TblVehicleCount")]
    public class VehicleCounterInformation : EntityBase<Guid>
    {
        [Column(name: "strStation")]
        public string Station { get; set; }

        [Column(name: "strDirection")]
        public string Direction { get; set; }

        [Column(name: "tHour")]
        public string Hour { get; set; }

        [Column(name: "strCategory")]
        public string Category { get; set; }

        [Column(name: "dtDate")]
        public DateTime Date { get; set; }

        [Column(name: "intQuantity")]
        public int Quantity { get; set; }        
    }
}
