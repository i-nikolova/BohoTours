namespace BohoTours.Web.Areas.Administration.Models.Bookings
{
    using System;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelBookingViewModel : IMapFrom<HotelBooking>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string HotelName { get; set; }

        public string RoomType { get; set; }

        public string LastName { get; set; }

        public int Duration { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public BookingStatus BookingStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HotelBooking, HotelBookingViewModel>()
                .ForMember(m => m.HotelName, opt => opt.MapFrom(x => x.Hotel.Room.Hotel.Name))
                .ForMember(m => m.RoomType, opt => opt.MapFrom(x => x.Hotel.Room.RoomType));
        }
    }
}
