namespace BohoTours.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.TownMaxLength)]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public ICollection<Hotel> Hotels { get; set; } = new HashSet<Hotel>();

        public ICollection<Vacation> Vacations { get; set; } = new HashSet<Vacation>();
    }
}
