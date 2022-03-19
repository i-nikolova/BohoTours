namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

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

        public int TransportId { get; set; }

        public Transport Transport { get; set; }

        public ICollection<Town> TownsVisited { get; set; } = new HashSet<Town>();
    }
}
