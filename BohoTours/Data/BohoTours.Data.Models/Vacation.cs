namespace BohoTours.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class Vacation : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.VacationNameMaxLength)]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public byte Duration { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public int TransportId { get; set; }

        public Transport Transport { get; set; }

        public ICollection<Town> TownsVisited { get; set; } = new HashSet<Town>();

        public ICollection<VacationPrice> VacationPrices { get; set; } = new HashSet<VacationPrice>();

        public ICollection<VacationImages> VacationImages { get; set; } = new HashSet<VacationImages>();

        public ICollection<VacationRatings> VacationsRatings { get; set; } = new HashSet<VacationRatings>();
    }
}
