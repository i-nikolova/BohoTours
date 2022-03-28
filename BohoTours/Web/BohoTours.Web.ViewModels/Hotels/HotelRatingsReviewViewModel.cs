namespace BohoTours.Web.ViewModels.Hotels
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelRatingsReviewViewModel : IMapFrom<HotelRatings>
    {
        public int Rating { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
