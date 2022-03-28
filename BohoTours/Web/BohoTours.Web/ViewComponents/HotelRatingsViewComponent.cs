namespace BohoTours.Web.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IViewComponentResult Invoke(int id)
        {
            var ratiing = this.hotelsService.GetReviews<HotelRatingsViewModel>(id);
            return this.View(ratiing);
        }
    }
}
