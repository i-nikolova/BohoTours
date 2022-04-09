namespace BohoTours.Data.Models
{
    using BohoTours.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HotelImages : BaseDeletableModel<int>
    {
        [Required]
        public string ImageUrl { get; set; }

        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; }
    }
}
