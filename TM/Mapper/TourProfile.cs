using AutoMapper;
using TM.Models.Entities;
using TM.Models.ViewModels;

namespace TM.Mapper
{
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            CreateMap<TourInfoViewModel, Tour>();
            CreateMap<Tour, TourInfoViewModel>();
            CreateMap<TourEditViewModel, Tour>();
            CreateMap<Tour, TourEditViewModel>();
        }
    }
}
