namespace BohoTours.Web.ViewModels.Vacations
{
    using BohoTours.Data.Common.Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateVacationViewModel
    {
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

        public int ContinentId { get; set; }

        [Range(1, byte.MaxValue)]
        public byte Duration { get; set; }

        public string CountryName { get; set; }

        [Display(Name = "Town")]
        public string TownName { get; set; }

        [Required]
        public ICollection<IFormFile> Images { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Towns { get; set; }

        public IEnumerable<SelectListItem> Continents { get; set; }

        public IEnumerable<SelectListItem> Transports { get; set; }

        public ICollection<VacationPriceViewModel> VacationPrices { get; set; } = new List<VacationPriceViewModel>();

        public int[] TownsVisited { get; set; }
    }
}
