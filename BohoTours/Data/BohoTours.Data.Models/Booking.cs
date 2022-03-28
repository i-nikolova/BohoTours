namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BohoTours.Data.Common.Models;

    public class Booking : BaseDeletableModel<int>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }

        public int HotelRoomPriceId { get; set; }

        [ForeignKey(nameof(HotelRoomPriceId))]
        public HotelRoomPrice Hotel { get; set; }

        public int VacationPriceId { get; set; }

        [ForeignKey(nameof(VacationPriceId))]
        public VacationPrice Vacation { get; set; }

        public BookingStatus BookingStatus { get; set; }

    }
}
