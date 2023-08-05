using F2xF2xFullStackAssesment.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace F2x.FullStackAssesment.Domain.Entities
{
    [Table(name: "TblVehicleCounterWithAmount")]
    public class VehicleCounterWithAmount : EntityBase<Guid>
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

        [Column(name: "dblAmount")]
        public double Amount { get; set; }
    }
}

