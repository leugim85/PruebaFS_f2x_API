using AutoMapper;
using F2x.FullStackAssesment.Core.Dtos.Response;
using F2x.FullStackAssesment.Core.Dtos.VehicleCounterApi.Response;
using F2x.FullStackAssesment.Domain.Entities;
using F2xF2xFullStackAssesment.Domain.Entities;
using F2xFullStackAssesment.Core.Dtos.Response;
using System;

namespace F2xFullStackAssesment.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VehicleCounterInformation, VehiclesCounterInformationDto>()
                .ForMember(dest => dest.Estacion, opt => opt.MapFrom(src => src.Station))
                .ForMember(dest => dest.Sentido, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.Hour))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();

            CreateMap<VehicleCounterWithAmount, VehicleCounterWithAmountDto>()
           .ForMember(dest => dest.Estacion, opt => opt.MapFrom(src => src.Station))
           .ForMember(dest => dest.Sentido, opt => opt.MapFrom(src => src.Direction))
           .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.Hour))
           .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Category))
           .ForMember(dest => dest.valorTabulado, opt => opt.MapFrom(src => src.Amount))
           .ReverseMap();

            CreateMap<VehicleCounterInformation, VehiclesCounterDataDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => new DateOnly(src.Date.Year, src.Date.Month, src.Date.Day)))
                .ReverseMap(); 

            CreateMap<VehicleCounterQueryHistory, VehicleCounterQueryHistoryDto>().ReverseMap();
        }
    }
}
