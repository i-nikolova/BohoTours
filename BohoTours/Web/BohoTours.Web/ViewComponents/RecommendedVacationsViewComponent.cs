namespace BohoTours.Web.ViewComponents
{

    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc;

    public class RecommendedVacationsViewComponent : ViewComponent
    {
        private readonly IVacationsService vacationsService;

        public RecommendedVacationsViewComponent(IVacationsService vacationsService)
        {
            this.vacationsService = vacationsService;
        }

        public IViewComponentResult Invoke()
        {
            var recommendedHotels = this.vacationsService.GetRecommended<VacationInListViewModel>();
            return this.View(recommendedHotels);
        }
    }
}
