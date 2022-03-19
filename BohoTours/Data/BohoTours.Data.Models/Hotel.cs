namespace BohoTours.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class Hotel : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.HotelNameMaxLength)]
        public string Name { get; set; }

        public string LAT { get; set; }

        public string LON { get; set; }

        [MaxLength(DataConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        public ICollection<HotelRoom> HotelRooms { get; set; } = new HashSet<HotelRoom>();

        public ICollection<HotelImages> HotelImages { get; set; } = new HashSet<HotelImages>();
    }
}
