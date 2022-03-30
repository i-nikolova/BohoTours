namespace BohoTours.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Services.Data.Bookings;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Bookings;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        public IActionResult AllHotelBookings()
        {
            var bookings = this.bookingsService.GetAllHotelsBookings<HotelBookingViewModel>();

            return this.View(bookings);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, int type)
        {
            await this.bookingsService.Delete(id, type);

            if (type == 1)
            {
                return this.RedirectToAction("AllHotelBookings");
            }

            return this.RedirectToAction("AllVacationBookings");
        }

        [Authorize]
        public async Task<IActionResult> ConfirmBooking(int id, int type)
        {
            await this.bookingsService.Confirm(id, type);

            if (type == 1)
            {
                return this.RedirectToAction("AllHotelBookings");
            }

            return this.RedirectToAction("AllVacationBookings");
        }

        [Authorize]
        public IActionResult AllVacationBookings()
        {
            var bookings = this.bookingsService.GetAllVacationBookings<VacationBookingViewModel>();

            return this.View(bookings);
        }

        [Authorize]
        public IActionResult BookHotel(int id)
        {
            var hotel = this.hotelsService.GetById<SingleHotelViewModel>(id);

            return this.View(hotel.HotelRooms);
        }

        [Authorize]
        public IActionResult BookVacation(int id)
        {
            var vacation = this.vacationsService.GetById<SingleVacationViewModel>(id);

            return this.View(vacation.VacationPrices);
        }

        [Authorize]
        public IActionResult AddHotelBooking(int modelId, int modelPriceId, int price, DateTime startDate)
        {
            var roomInfo = this.hotelsService.GetRoomInfo(modelPriceId);

            var booking = new CreateHotelBookingViewModel
            {
                EntityPriceId = modelPriceId,
                Price = price,
                StartDate = startDate,
                RoomType = roomInfo.RoomType,
                HotelName = roomInfo.HotelName,
                EntityId = modelId,
            };

            return this.View(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddHotelBooking([FromForm] CreateHotelBookingViewModel viewModel)
        {
            await this.bookingsService.Create(viewModel);

            return this.Redirect("/Bookings/Success");
        }

        [Authorize]
        public IActionResult AddVacationBooking(int modelId, int modelPriceId, int price, DateTime startDate)
        {
            var vacation = this.vacationsService.GetById<SingleVacationViewModel>(modelId);

            var booking = new CreateVacationBookingViewModel
            {
                EntityPriceId = modelPriceId,
                Price = price,
                VacantionName = vacation.Name,
                VacantionDuration = vacation.Duration,
                StartDate = startDate,
                EntityId = modelId,
            };

            return this.View(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddVacationBooking([FromForm] CreateVacationBookingViewModel viewModel)
        {
            await this.bookingsService.Create(viewModel);

            return this.Redirect("/Bookings/Success");
        }

        public IActionResult Success()
        {
            return this.View();
        }
    }
}
