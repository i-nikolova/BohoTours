namespace BohoTours.Web.ViewModels.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditVacationViewModel : IMapFrom<Vacation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.HotelNameMaxLength, MinimumLength = DataConstants.HotelNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataConstants.DescriptionMaxLength, MinimumLength = DataConstants.DescriptionMixLength)]
        public string Description { get; set; }

        [Required]
        [StringLength(DataConstants.DescriptionMaxLength, MinimumLength = DataConstants.DescriptionMixLength)]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        public int TransportId { get; set; }

        [Required]
        [Display(Name="Cover image")]
        public string ImagePath { get; set; }

        public int ContinentId { get; set; }

        [Range(1, byte.MaxValue)]
        public byte Duration { get; set; }

        public string CountryName { get; set; }
        [Required]
        [Display(Name = "Town")]
        public string TownName { get; set; }

        public ICollection<IFormFile> Images { get; set; }

        [Required]
        public ICollection<ImportedVacationImagesViewModel> ImportedImages { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        public IEnumerable<SelectListItem> Continents { get; set; }

        public IEnumerable<SelectListItem> Transports { get; set; }

        public ICollection<VacationPriceViewModel> VacationPrices { get; set; } = new List<VacationPriceViewModel>();

        public int[] TownsVisited { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Vacation, EditVacationViewModel>()
                .ForMember(m => m.CountryName, opt => opt.MapFrom(x => x.Country.Name))
                .ForMember(m => m.CountryId, opt => opt.MapFrom(x => x.Country.Id))
                .ForMember(m => m.ContinentId, opt => opt.MapFrom(x => x.Country.ContinentId))
                .ForMember(m => m.TownsVisited, opt => opt.MapFrom(x => x.TownsVisited.Select(x => x.Id)))
                .ForMember(m => m.ImportedImages, opt => opt.MapFrom(hi => hi.VacationImages));
        }
    }
}
