using BohoTours.Web.Areas.Administration.Models.Bookings;

namespace BohoTours.Web.Areas.Administration.Controllers
{
    using BohoTours.Services.Data.Bookings;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class BookingsController : AdministrationController
    {
        private readonly IBookingsService bookingsService;

        public BookingsController(IBookingsService bookingsService)
        {
            this.bookingsService = bookingsService;
        }

        public IActionResult AllHotelBookings()
        {
            var bookings = this.bookingsService.GetAllHotelsBookings<HotelBookingViewModel>();

            return this.View(bookings);
        }

        public async Task<IActionResult> Delete(int id, int type)
        {
            await this.bookingsService.Delete(id, type);

            if (type == 1)
            {
                return this.RedirectToAction("AllHotelBookings");
            }

            return this.RedirectToAction("AllVacationBookings");
        }

        public async Task<IActionResult> ConfirmBooking(int id, int type)
        {
            await this.bookingsService.Confirm(id, type);

            if (type == 1)
            {
                return this.RedirectToAction("AllHotelBookings");
            }

            return this.RedirectToAction("AllVacationBookings");
        }

        public IActionResult AllVacationBookings()
        {
            var bookings = this.bookingsService.GetAllVacationBookings<VacationBookingViewModel>();

            return this.View(bookings);
        }
    }
}
