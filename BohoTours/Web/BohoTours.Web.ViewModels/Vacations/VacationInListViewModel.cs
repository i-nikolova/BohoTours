namespace BohoTours.Web.ViewModels.Vacations
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class VacationInListViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string CountryName { get; set; }

        public int CountryId { get; set; }

        public string Description { get; set; }

        public string TownsVisited { get; set; }

        public decimal VacationMinPrice => this.VacationPrices.Min(x => x.Price);

        public ICollection<VacationPriceViewModel> VacationPrices { get; set; }

        public string TransportType { get; set; }

        public byte Duration { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vacation, VacationInListViewModel>()
                .ForMember(x => x.TownsVisited, opt => opt.MapFrom(x => string.Join(", ", x.TownsVisited.Select(t => t.Name))))
                .ForMember(x => x.TransportType, opt => opt.MapFrom(x => x.Transport.TransportType))
                .ForMember(x => x.CountryName, opt => opt.MapFrom(x => x.Country.Name));
        }
    }
}
