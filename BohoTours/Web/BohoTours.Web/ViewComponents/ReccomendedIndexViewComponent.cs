namespace BohoTours.Web.ViewComponents
{
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Home;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc;

    public class ReccomendedIndexViewComponent : ViewComponent
    {
        private readonly IHotelsService hotelsService;
        private readonly IVacationsService vacationsService;

        public ReccomendedIndexViewComponent(IHotelsService hotelsService, IVacationsService vacationsService)
        {
            this.hotelsService = hotelsService;
            this.vacationsService = vacationsService;
        }

        public IViewComponentResult Invoke(string continentCode)
        {
            var hotels = this.hotelsService.GetRecommendedByContinent<HotelInListViewModel>(continentCode);
            var vacations = this.vacationsService.GetRecommendedByContinent<VacationInListViewModel>(continentCode);

            var reccomended = new ReccomendedIndexViewModel
            {
                Hotels = hotels,
                Vacations = vacations,
            };

            return this.View(reccomended);
        }
    }
}
