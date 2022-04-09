namespace BohoTours.Web.Controllers
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Data;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using System.Linq;
    using System.Threading.Tasks;

    public class HotelsController : Controller
    {
        private readonly IHotelsService hotelsService;
        private readonly ITownsService townsService;
        private readonly ICountriesService countriesService;
        private readonly IContinentsService continentsService;

        public HotelsController(IHotelsService hotelsService, ITownsService townsService, ICountriesService countriesService, IContinentsService continentsService)
        {
            this.hotelsService = hotelsService;
            this.townsService = townsService;
            this.countriesService = countriesService;
            this.continentsService = continentsService;
        }

        public IActionResult All(string town, string searchTerm, Sorting sorting, int id = 1)
        {
            var itemsPerPage = 12;

            var hotels = this.hotelsService.GetAll<HotelInListViewModel>().AsQueryable();
            var hotelsCount = this.hotelsService.GetCount();

            if (sorting != 0)
            {
                hotels = sorting switch
                {
                    Sorting.Name => hotels.OrderBy(x => x.Name),
                    Sorting.PriceAsc => hotels.OrderBy(x => x.RoomMinPrice),
                    Sorting.PriceDesc => hotels.OrderByDescending(x => x.RoomMinPrice),
                    Sorting.Country => hotels.OrderBy(x => x.TownName),
                    _ => hotels,
                };
            }

            if (!string.IsNullOrWhiteSpace(town) || !string.IsNullOrWhiteSpace(searchTerm))
            {
                if (!string.IsNullOrWhiteSpace(town))
                {
                    hotels = hotels.Where(x => x.TownName == town).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    hotels = hotels.Where(x =>
                        x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        x.CountryName.ToLower().Contains(searchTerm.ToLower()) ||
                        x.TownName.ToLower().Contains(searchTerm.ToLower())).AsQueryable();
                }

                hotelsCount = hotels.Count();
            }

            hotels = hotels.Skip((id - 1) * itemsPerPage).Take(itemsPerPage);

            var towns = this.townsService.GetAll<TownViewModel>().Select(x => x.Name);

            var viewModel = new HotelsListViewModel()
            {
                ItemsPerPage = 12,
                PageNumber = id,
                Hotels = hotels,
                HotelCount = hotelsCount,
                SearchTerm = searchTerm,
                Towns = towns,
                Town = town,
                Sorting = sorting,
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int id, FeedbackViewModel feedback)
        {
            if (feedback.ModelId != 0)
            {
                id = feedback.ModelId;
            }

            var hotel = this.hotelsService.GetById<SingleHotelViewModel>(id);

            if (feedback == null)
            {
                hotel.Feedback = new FeedbackViewModel();
            }
            else
            {
                hotel.Feedback = feedback;
            }

            this.ViewData["Title"] = hotel.Name;

            return this.View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(FeedbackViewModel feedback)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction($"Details", new RouteValueDictionary(feedback));
            }

            await this.hotelsService.AddFeedback(feedback);
            return this.RedirectToAction($"Details", new { id = feedback.ModelId });
        }

        public IActionResult Reviews(int id)
        {
            var ratings = this.hotelsService.GetReviews<HotelRatingsReviewViewModel>(id);

            var ratingInfo = new HotelRatingsViewModel()
            {
                HotelName = this.hotelsService.GetById<SingleHotelViewModel>(id).Name,
                HotelRatingsReviews = ratings.ToList(),
                Rating = ratings.ToList().Average(x => x.Rating),
                RatingsCount = ratings.Count(),
            };

            return this.View(ratingInfo);
        }
    }
}
