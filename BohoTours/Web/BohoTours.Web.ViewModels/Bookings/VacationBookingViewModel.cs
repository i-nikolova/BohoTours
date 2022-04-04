namespace BohoTours.Web.Areas.Administration.Models.Bookings
{
    using System;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class VacationBookingViewModel : IMapFrom<VacationBooking>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string VacationName { get; set; }

        public string LastName { get; set; }

        public int People { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public BookingStatus BookingStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<VacationBooking, VacationBookingViewModel>()
                .ForMember(m => m.VacationName, opt => opt.MapFrom(x => x.VacationPrice.Vacation.Name));
        }
    }
}
