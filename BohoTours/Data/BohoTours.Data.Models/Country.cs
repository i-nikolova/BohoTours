namespace BohoTours.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.CountryMaxLength)]
        public string Name { get; set; }

        public int ContinentId { get; set; }

        [ForeignKey(nameof(ContinentId))]
        public Continent Continent { get; set; }

        public ICollection<Town> Towns { get; set; } = new HashSet<Town>();
    }
}
