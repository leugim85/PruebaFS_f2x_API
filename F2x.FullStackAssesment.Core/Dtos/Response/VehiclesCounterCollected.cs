using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.Dtos.Response
{
    public class VehiclesCounterCollectedPaginated
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<VehiclesCounterDataDto> Items { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public int TotalRows { get; set; }

        public VehiclesCounterCollectedPaginated(int pageNumber, int pageSize, int totalRows, List<VehiclesCounterDataDto> intems)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalRows / (double)pageSize);
            Items = intems;
            HasPreviousPage = (pageNumber > 1);
            HasNextPage = (pageNumber < TotalPages);
            TotalRows = totalRows;
        }
    }
}
