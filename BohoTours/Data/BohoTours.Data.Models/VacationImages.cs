namespace BohoTours.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using BohoTours.Data.Common.Models;

    public class VacationImages : BaseDeletableModel<int>
    {
        public string ImageUrl { get; set; }

        public int VacationId { get; set; }

        [ForeignKey(nameof(VacationId))]
        public Vacation Vacation { get; set; }
    }
}
