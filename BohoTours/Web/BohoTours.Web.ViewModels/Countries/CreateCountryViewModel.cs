namespace BohoTours.Web.ViewModels.Countries
{
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateCountryViewModel : IMapFrom<Country>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ContinentId { get; set; }

        [Required]
        [StringLength(DataConstants.CountryMaxLength, MinimumLength = DataConstants.CountryMinLength)]
        public string CountryName { get; set; }
    }
}
