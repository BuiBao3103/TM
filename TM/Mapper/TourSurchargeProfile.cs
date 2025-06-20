using AutoMapper;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Mapper;

public class TourSurchargeProfile : Profile
{
    public TourSurchargeProfile()
    {
        CreateMap<TourSurcharge, TourSurchargeViewModel>()
            .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour.Name));

        CreateMap<TourSurchargeViewModel, TourSurcharge>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}