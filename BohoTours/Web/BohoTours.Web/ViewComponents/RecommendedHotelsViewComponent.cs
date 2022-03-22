namespace BohoTours.Web.Views.Shared.Components.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Services.Data.Hotels;
    using BohoTours.Web.ViewModels.Hotels;
    using Microsoft.AspNetCore.Mvc;

    public class RecommendedHotelsViewComponent : ViewComponent
    {
        private readonly IHotelsService hotelsService;

        public RecommendedHotelsViewComponent(IHotelsService hotelsService)
        {
            this.hotelsService = hotelsService;
        }

        public IViewComponentResult Invoke()
        {
           var recommendedHotels = this.hotelsService.GetRecommended<HotelInListViewModel>();
           return this.View(recommendedHotels);
        }
    }
}
