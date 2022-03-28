namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Models;

    public class HotelRoomPrice : BaseDeletableModel<int>
    {
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public HotelRoom Room { get; set; }

        public DateTime Date { get; set; }

        public decimal PricePerNight { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
