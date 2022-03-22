namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditHotelViewModel : IMapFrom<Hotel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LAT { get; set; }

        public string LON { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int TownId { get; set; }

        public string TownName { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int ContinentId { get; set; }

        public string ContinentName { get; set; }

        public ICollection<HotelRoomViewModel> HotelRooms { get; set; } = new List<HotelRoomViewModel>();

        [Required]
        public ICollection<IFormFile> Images { get; set; }

        public ICollection<ImportedImagesViewModel> ImportedImages { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Continents { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hotel, EditHotelViewModel>()
                .ForMember(m => m.CountryName, opt => opt.MapFrom(x => x.Town.Country.Name))
                .ForMember(m => m.CountryId, opt => opt.MapFrom(x => x.Town.Country.Id))
                .ForMember(m => m.ContinentId, opt => opt.MapFrom(x => x.Town.Country.ContinentId))
                .ForMember(m => m.ContinentId, opt => opt.MapFrom(x => x.Town.Country.Continent.Id))
                .ForMember(m => m.ImportedImages, opt => opt.MapFrom(hi => hi.HotelImages.Select(x => x.ImageUrl)));
        }
    }
}
