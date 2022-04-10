namespace BohoTours.Web.ViewModels.Towns
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class CreateTownViewModel : IMapFrom<Town>, IHaveCustomMappings
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int AddedCountryId { get; set; }

        [Required]
        [StringLength(DataConstants.TownMaxLength, MinimumLength = DataConstants.TownMinLength)]
        public string TownName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Town, CreateTownViewModel>()
                .ForMember(m => m.AddedCountryId, opt => opt.MapFrom(x => x.CountryId));
        }
    }
}
