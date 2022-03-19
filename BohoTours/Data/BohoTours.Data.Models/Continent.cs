namespace BohoTours.Data.Models
{
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class Continent : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.ContinentNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DataConstants.ContinentCodeMaxLength)]
        public string ContinentCode { get; set; }
    }
}
