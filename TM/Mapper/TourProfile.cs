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
            CreateMap<TM.Models.ViewModels.TourEditViewModel, Tour>();
            CreateMap<Tour, TM.Models.ViewModels.TourEditViewModel>();
        }
    }
}
