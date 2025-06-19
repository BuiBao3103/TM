using AutoMapper;
using TM.Models.Entities;
using TM.Models.ViewModels;

public class TourPassengerProfile : Profile
{
    public TourPassengerProfile()
    {
        CreateMap<TourPassengerViewModel, Passenger>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                DateOnly.FromDateTime(src.DateOfBirth)))
            .ForMember(dest => dest.Tour, opt => opt.Ignore()) // tránh AutoMapper cố map navigation property
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeleteAt, opt => opt.Ignore());
               //     .ForMember(dest => dest.TourId, opt => opt.MapFrom(src =>
               //src.TourId));

    }
}
