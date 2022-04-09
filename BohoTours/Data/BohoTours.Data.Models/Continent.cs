namespace BohoTours.Data.Models
{
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

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
