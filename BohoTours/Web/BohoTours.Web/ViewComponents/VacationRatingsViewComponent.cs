namespace BohoTours.Web.ViewComponents
{
    using System.Linq;

    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc;

    public class VacationRatingsViewComponent : ViewComponent
    {
        private readonly IVacationsService vacationsService;

        public VacationRatingsViewComponent(IVacationsService vacationsService)
        {
            this.vacationsService = vacationsService;
        }

        public IViewComponentResult Invoke(InvokeRequest request)
        {
            var ratings = this.vacationsService.GetReviews<VacationRatingsReviewViewModel>(request.Id);

            var ratingInfo = new VacationRatingsViewModel()
            {
                Id = request.Id,
                HotelName = request.VacationName,
                HotelRatingsReviews = ratings.ToList(),
                Rating = ratings.ToList().Average(x => x.Rating),
                RatingsCount = ratings.Count(),
            };

            return this.View(ratingInfo);
        }

        public class InvokeRequest
        {
            public int Id { get; set; }

            public string VacationName { get; set; }
        }
    }
}
