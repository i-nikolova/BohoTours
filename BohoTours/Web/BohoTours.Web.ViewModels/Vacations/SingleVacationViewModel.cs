namespace BohoTours.Web.ViewModels.Vacations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Feedbacks;

    public class SingleVacationViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string CountryName { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string TownsVisited { get; set; }

        public decimal VacationMinPrice => this.VacationPrices.Min(x => x.Price);

        public string TransportType { get; set; }

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
                .ForMember(m => m.Images, opt => opt.MapFrom(hi => hi.VacationImages.Where(x => !x.IsDeleted).Select(x => x.ImageUrl)))
                .ForMember(x => x.TransportType, opt => opt.MapFrom(x => x.Transport.TransportType));
        }
    }
}
