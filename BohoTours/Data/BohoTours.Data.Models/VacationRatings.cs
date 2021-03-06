namespace BohoTours.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BohoTours.Data.Common.Models;

    public class VacationRatings : BaseDeletableModel<int>
    {
        public int VacationId { get; set; }

        [ForeignKey(nameof(VacationId))]
        public Vacation Vacation { get; set; }

        public int Rating { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
