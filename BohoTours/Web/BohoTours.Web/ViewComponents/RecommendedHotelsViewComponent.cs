using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BohoTours.Services.Data.Hotels;
using BohoTours.Web.ViewModels.Hotels;

namespace BohoTours.Web.Views.Shared.Components.Hotels
{
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
