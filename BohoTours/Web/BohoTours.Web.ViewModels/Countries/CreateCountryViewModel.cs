namespace BohoTours.Web.ViewModels.Countries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

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
