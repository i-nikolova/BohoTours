namespace BohoTours.Web.ViewModels.Hotels
{
    using System.Collections.Generic;

    public class HotelRatingsViewModel
    {
        public int Id { get; set; }

        public string HotelName { get; set; }

        public double Rating { get; set; }

        public int RatingsCount { get; set; }

        public IEnumerable<HotelRatingsReviewViewModel> HotelRatingsReviews { get; set; } = new HashSet<HotelRatingsReviewViewModel>();
    }
}
