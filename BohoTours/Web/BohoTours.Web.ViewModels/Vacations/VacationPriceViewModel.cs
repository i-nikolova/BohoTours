namespace BohoTours.Web.ViewModels.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class VacationPriceViewModel : IMapFrom<VacationPrice>
    {
        public int Id { get; set; }

        public int VacationId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
