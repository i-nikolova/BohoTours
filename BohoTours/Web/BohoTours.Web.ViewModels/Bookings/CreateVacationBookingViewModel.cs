namespace BohoTours.Web.ViewModels.Bookings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Common.Constants;

    public class CreateVacationBookingViewModel
    {
        public int Id { get; set; }

        public int EntityId { get; set; }

        public int EntityPriceId { get; set; }

        [Required]
        [StringLength(DataConstants.NameMaxLength, MinimumLength = DataConstants.NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(DataConstants.EmailMaxLength)]
        public string Email { get; set; }

        public string VacantionName { get; set; }

        public int VacantionDuration { get; set; }

        [Required]
        [StringLength(DataConstants.NameMaxLength, MinimumLength = DataConstants.NameMinLength)]
        public string LastName { get; set; }

        [Range(1, 10)]
        public int People { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
