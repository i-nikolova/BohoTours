namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelRatingsViewModel : IMapFrom<Hotel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string HotelName { get; set; }

        public double Rating { get; set; }

        public int RatingsCount { get; set; }

        public IEnumerable<HotelRatingsReviewViewModel> HotelRatingsReviews { get; set; } = new HashSet<HotelRatingsReviewViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hotel, HotelRatingsViewModel>()
                 .ForMember(m => m.HotelName, opt => opt.MapFrom(x => x.Name))
                 .ForMember(m => m.HotelRatingsReviews, opt => opt.MapFrom(x => x.HotelRatings))
                 .ForMember(m => m.Rating, opt => opt.MapFrom(x => x.HotelRatings.Average(x => x.Rating)))
                 .ForMember(m => m.RatingsCount, opt => opt.MapFrom(x => x.HotelRatings.Count()));
        }
    }
}
