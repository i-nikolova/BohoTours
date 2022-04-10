namespace BohoTours.Web.ViewComponents
{
    using System.Linq;

    using BohoTours.Services.Data.Hotels;
    using BohoTours.Web.ViewModels.Hotels;
    using Microsoft.AspNetCore.Mvc;

    public class HotelRatingsViewComponent : ViewComponent
    {
        private readonly IHotelsService hotelsService;

        public HotelRatingsViewComponent(IHotelsService hotelsService)
        {
            this.hotelsService = hotelsService;
        }

        public IViewComponentResult Invoke(InvokeRequest request)
        {
            var ratings = this.hotelsService.GetReviews<HotelRatingsReviewViewModel>(request.Id);

            var ratingInfo = new HotelRatingsViewModel()
            {
                Id = request.Id,
                HotelName = request.HotelName,
                HotelRatingsReviews = ratings.ToList(),
                Rating = ratings.ToList().Average(x => x.Rating),
                RatingsCount = ratings.Count(),
            };

            return this.View(ratingInfo);
        }

        public class InvokeRequest
        {
            public int Id { get; set; }

            public string HotelName { get; set; }
        }
    }
}
