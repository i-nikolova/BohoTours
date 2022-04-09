namespace BohoTours.Web.ViewModels.Hotels
{
    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Feedbacks;
    using System.Collections.Generic;
    using System.Linq;

    public class SingleHotelViewModel : IMapFrom<Hotel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LAT { get; set; }

        public string LON { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string TownName { get; set; }

        public string CountryName { get; set; }

        public bool HasReviews { get; set; }

        public FeedbackViewModel Feedback { get; set; }

        public ICollection<HotelRoomViewModel> HotelRooms { get; set; } = new List<HotelRoomViewModel>();

        public ICollection<string> Images { get; set; } = new List<string>();

        public decimal RoomMinPrice => this.HotelRooms.SelectMany(r => r.HotelRoomPrices).Min(hrp => hrp.PricePerNight);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hotel, SingleHotelViewModel>()
                .ForMember(m => m.CountryName, opt => opt.MapFrom(x => x.Town.Country.Name))
                .ForMember(m => m.HasReviews, opt => opt.MapFrom(x => x.HotelRatings.Count > 0))
                .ForMember(m => m.Images, opt => opt.MapFrom(hi => hi.HotelImages.Where(x => !x.IsDeleted).Select(x => x.ImageUrl)))
                .ForMember(
                    m => m.RoomMinPrice,
                    opt => opt.MapFrom(x => x.HotelRooms.SelectMany(r => r.HotelRoomPrices).Min(hrp => hrp.PricePerNight)));
        }
    }
}
