namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class HotelBooking : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(DataConstants.EmailMaxLength)]
        public string Email { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int HotelRoomPriceId { get; set; }

        [ForeignKey(nameof(HotelRoomPriceId))]
        public HotelRoomPrice Hotel { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
