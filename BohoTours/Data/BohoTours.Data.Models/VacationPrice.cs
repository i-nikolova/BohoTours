namespace BohoTours.Data.Models
{
    using BohoTours.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class VacationPrice : BaseDeletableModel<int>
    {
        public int VacationId { get; set; }

        [ForeignKey(nameof(VacationId))]
        public Vacation Vacation { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public ICollection<VacationBooking> Bookings { get; set; } = new HashSet<VacationBooking>();
    }
}
