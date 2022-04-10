namespace BohoTours.Web.ViewModels.Vacations
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class VacationRatingsReviewViewModel : IMapFrom<VacationRatings>
    {
        public int Rating { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
