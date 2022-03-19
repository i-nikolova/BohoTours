namespace BohoTours.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Models;

    public class HotelRoom : BaseDeletableModel<int>
    {
        [Required]
        public string RoomType { get; set; }

        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; }

        public int MaxCapacity { get; set; }

        public IEnumerable<HotelRoomPrice> HotelRoomPrices { get; set; } = new HashSet<HotelRoomPrice>();
    }
}
