﻿namespace BohoTours.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Models;

    public class VacationPrice : BaseDeletableModel<int>
    {
        public int VacationId { get; set; }

        [ForeignKey(nameof(VacationId))]
        public Vacation Vacation { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }
    }
}
