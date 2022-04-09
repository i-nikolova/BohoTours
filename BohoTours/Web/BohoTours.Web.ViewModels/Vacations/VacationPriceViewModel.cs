namespace BohoTours.Web.ViewModels.Vacations
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;

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
