namespace BohoTours.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Common.Models;

    public class VacationBooking : BaseDeletableModel<int>
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
        public int People { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int VacationPriceId { get; set; }

        [ForeignKey(nameof(VacationPriceId))]
        public VacationPrice VacationPrice { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
