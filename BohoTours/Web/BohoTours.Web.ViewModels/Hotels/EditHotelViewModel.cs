namespace BohoTours.Web.ViewModels.Hotels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditHotelViewModel : IMapFrom<Hotel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.HotelNameMaxLength, MinimumLength = DataConstants.HotelNameMinLength)]
        public string Name { get; set; }

        [Required]
        public string LAT { get; set; }

        [Required]
        public string LON { get; set; }

        [Required]
        [StringLength(DataConstants.DescriptionMaxLength, MinimumLength = DataConstants.DescriptionMixLength)]
        public string Description { get; set; }

        [Display(Name = "Cover image")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Town")]
        public int TownId { get; set; }

        public string TownName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int ContinentId { get; set; }

        public string ContinentName { get; set; }

        public ICollection<HotelRoomViewModel> HotelRooms { get; set; } = new List<HotelRoomViewModel>();

        public ICollection<IFormFile> Images { get; set; }

        [Required]
        public ICollection<ImportedHotelsImagesViewModel> ImportedImages { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Continents { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hotel, EditHotelViewModel>()
                .ForMember(m => m.CountryName, opt => opt.MapFrom(x => x.Town.Country.Name))
                .ForMember(m => m.CountryId, opt => opt.MapFrom(x => x.Town.Country.Id))
                .ForMember(m => m.ContinentId, opt => opt.MapFrom(x => x.Town.Country.ContinentId))
                .ForMember(m => m.ImportedImages, opt => opt.MapFrom(hi => hi.HotelImages));
        }
    }
}
