namespace BohoTours.Web.ViewModels.Vacations
{
    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System.Linq;

    public class VacationInListViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string CountryName { get; set; }

        public int CountryId { get; set; }

        public string Description { get; set; }

        public string TownsVisited { get; set; }

        public decimal VacationMinPrice { get; set; }

        public string TrasportType { get; set; }

        public byte Duration { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vacation, VacationInListViewModel>()
                .ForMember(x => x.TownsVisited, opt => opt.MapFrom(x => string.Join(", ", x.TownsVisited.Select(t => t.Name))))
                .ForMember(x => x.VacationMinPrice, opt => opt.MapFrom(x => x.VacationPrices.Select(x => x.Price).Min()))
                .ForMember(x => x.TrasportType, opt => opt.MapFrom(x => x.Transport.TransportType));
        }
    }
}
