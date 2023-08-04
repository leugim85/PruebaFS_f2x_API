﻿using F2x.FullStackAssesment.Core.Dtos.Request;
using F2x.FullStackAssesment.Core.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F2x.FullStackAssesment.Core.IServices
{
    public interface IVehicleCountService
    {
        Task ProcessdataFromCounterApi();
        Task<VehiclesCounterCollectedPaginated> GetDataVehicleCounterPaginated(VehiclesInformationPaginatedDto vehiclesInformationPaginated);
        Task<DateOnly> GetLastDate();
    }
}