﻿namespace BohoTours.Data.Models
{
    using BohoTours.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HotelRoomPrice : BaseDeletableModel<int>
    {
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public HotelRoom Room { get; set; }

        public DateTime Date { get; set; }

        public decimal PricePerNight { get; set; }

        public ICollection<HotelBooking> Bookings { get; set; } = new HashSet<HotelBooking>();
    }
}
