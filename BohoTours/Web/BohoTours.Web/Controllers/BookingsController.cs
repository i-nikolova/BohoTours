namespace BohoTours.Web.Controllers
{
    using BohoTours.Services.Data.Bookings;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BookingsController : BaseController
    {
        private readonly IBookingsService bookingsService;
        private readonly IVacationsService vacationsService;
        private readonly IHotelsService hotelsService;

        public BookingsController(IBookingsService bookingsService, IVacationsService vacationsService, IHotelsService hotelsService)
        {
            this.bookingsService = bookingsService;
            this.vacationsService = vacationsService;
            this.hotelsService = hotelsService;
        }

        public IActionResult BookHotel(int id)
        {
            var hotel = this.hotelsService.GetById<SingleHotelViewModel>(id);

            return this.View(hotel.HotelRooms);
        }
    }
}
