﻿namespace BohoTours.Web.ViewModels.Vacations
{
    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Feedbacks;
    using System.Collections.Generic;
    using System.Linq;

    public class SingleVacationViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string CountryName { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string TownsVisited { get; set; }

        public decimal VacationMinPrice { get; set; }

        public string TrasportType { get; set; }

        public byte Duration { get; set; }

        public bool HasReviews { get; set; }

        public FeedbackViewModel Feedback { get; set; }

        public ICollection<VacationPriceViewModel> VacationPrices { get; set; }

        public ICollection<string> Images { get; set; } = new List<string>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vacation, SingleVacationViewModel>()
                .ForMember(x => x.TownsVisited, opt => opt.MapFrom(x => string.Join(", ", x.TownsVisited.Select(t => t.Name))))
                .ForMember(m => m.HasReviews, opt => opt.MapFrom(x => x.VacationsRatings.Count > 0))
                .ForMember(x => x.VacationMinPrice, opt => opt.MapFrom(x => x.VacationPrices.Select(x => x.Price).Min()))
                .ForMember(m => m.Images, opt => opt.MapFrom(hi => hi.VacationImages.Where(x => !x.IsDeleted).Select(x => x.ImageUrl)))
                .ForMember(x => x.TrasportType, opt => opt.MapFrom(x => x.Transport.TransportType));
        }
    }
}
