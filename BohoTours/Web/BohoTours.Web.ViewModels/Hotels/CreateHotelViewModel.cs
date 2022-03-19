using BohoTours.Data.Models;

namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Common.Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateHotelViewModel
    {
        [Required]
        [StringLength(DataConstants.HotelNameMaxLength,MinimumLength = DataConstants.HotelNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DataConstants.DescriptionMaxLength, MinimumLength = DataConstants.DescriptionMixLength)]
        public string Description { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public string LAT { get; set; }

        [Required]
        public string LON { get; set; }

        [Required]
        public ICollection<IFormFile> Images { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<HotelRoomViewModel> HotelRooms { get; set; } = new List<HotelRoomViewModel>();
    }
}
