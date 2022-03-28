namespace BohoTours.Web.ViewModels.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class VacationRatingsViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string HotelName { get; set; }

        public double Rating { get; set; }

        public int RatingsCount { get; set; }

        public IEnumerable<VacationRatingsReviewViewModel> HotelRatingsReviews { get; set; } = new HashSet<VacationRatingsReviewViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vacation, VacationRatingsViewModel>()
                 .ForMember(m => m.HotelName, opt => opt.MapFrom(x => x.Name))
                 .ForMember(m => m.HotelRatingsReviews, opt => opt.MapFrom(x => x.VacationsRatings))
                 .ForMember(m => m.Rating, opt => opt.MapFrom(x => x.VacationsRatings.Average(x => x.Rating)))
                 .ForMember(m => m.RatingsCount, opt => opt.MapFrom(x => x.VacationsRatings.Count()));
        }
    }
}
