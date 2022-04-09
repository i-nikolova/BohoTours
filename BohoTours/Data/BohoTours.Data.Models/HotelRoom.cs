namespace BohoTours.Data.Models
{
    using BohoTours.Data.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HotelRoom : BaseDeletableModel<int>
    {
        [Required]
        public string RoomType { get; set; }

        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; }

        public int MaxCapacity { get; set; }

        public ICollection<HotelRoomPrice> HotelRoomPrices { get; set; } = new HashSet<HotelRoomPrice>();
    }
}
