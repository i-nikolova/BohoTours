namespace BohoTours.Web.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IViewComponentResult Invoke(int id)
        {
            var rating = this.vacationsService.GetReviews<VacationRatingsViewModel>(id);
            return this.View(rating);
        }
    }
}
