namespace BohoTours.Services.Data.Bookings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BohoTours.Web.ViewModels.Bookings;

    public interface IBookingsService
    {
        IEnumerable<T> GetAllHotelsBookings<T>();

        IEnumerable<T> GetAllVacationBookings<T>();

        Task Create(CreateHotelBookingViewModel model);

        Task Create(CreateVacationBookingViewModel model);

        Task Delete(int id, int type);

        Task Confirm(int id, int type);
    }
}
