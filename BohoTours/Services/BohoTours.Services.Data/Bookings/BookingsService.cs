namespace BohoTours.Services.Data.Bookings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Bookings;

    public class BookingsService : IBookingsService
    {
        private readonly IDeletableEntityRepository<HotelBooking> hotelBookingsRepository;
        private readonly IDeletableEntityRepository<VacationBooking> vacationsBookingsRepository;
        private readonly IDeletableEntityRepository<HotelRoomPrice> hotelRoomPriceRepository;
        private readonly IDeletableEntityRepository<VacationPrice> vacationPriceRepository;

        public BookingsService(IDeletableEntityRepository<HotelBooking> hotelBookingsRepository, IDeletableEntityRepository<HotelRoomPrice> hotelRoomPriceRepository, IDeletableEntityRepository<VacationPrice> vacationPriceRepository, IDeletableEntityRepository<VacationBooking> vacationsBookingsRepository)
        {
            this.hotelBookingsRepository = hotelBookingsRepository;
            this.hotelRoomPriceRepository = hotelRoomPriceRepository;
            this.vacationPriceRepository = vacationPriceRepository;
            this.vacationsBookingsRepository = vacationsBookingsRepository;
        }

        public async Task Confirm(int id, int type)
        {
            if (type == 1)
            {
                var booking = this.hotelBookingsRepository.All().FirstOrDefault(x => x.Id == id);
                booking.BookingStatus = BookingStatus.Approved;

                this.hotelBookingsRepository.Update(booking);
                await this.hotelBookingsRepository.SaveChangesAsync();
            }

            if (type == 2)
            {
                var booking = this.vacationsBookingsRepository.All().FirstOrDefault(x => x.Id == id);
                booking.BookingStatus = BookingStatus.Approved;

                this.vacationsBookingsRepository.Update(booking);
                await this.vacationsBookingsRepository.SaveChangesAsync();
            }
        }

        public async Task Create(CreateHotelBookingViewModel model)
        {
            var hotelRoomPrice = this.hotelRoomPriceRepository.All().Where(x => x.Id == model.EntityPriceId).FirstOrDefault();

            var booking = new HotelBooking()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                StartDate = model.StartDate,
                BookingStatus = (BookingStatus)1,
                Duration = model.Duration,
                Price = model.TotalPrice,
                EndDate = model.EndDate,
            };

            hotelRoomPrice.Bookings.Add(booking);

            this.hotelRoomPriceRepository.Update(hotelRoomPrice);
            await this.hotelBookingsRepository.SaveChangesAsync();
        }

        public async Task Create(CreateVacationBookingViewModel model)
        {
            var vacationPrice = this.vacationPriceRepository.All().Where(x => x.Id == model.EntityPriceId).FirstOrDefault();

            var booking = new VacationBooking()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                StartDate = model.StartDate,
                BookingStatus = (BookingStatus)1,
                People = model.People,
                Price = model.TotalPrice,
                EndDate = model.EndDate,
            };

            vacationPrice.Bookings.Add(booking);

            this.vacationPriceRepository.Update(vacationPrice);
            await this.vacationsBookingsRepository.SaveChangesAsync();
        }

        public async Task Delete(int id, int type)
        {
            if (type == 1)
            {
                var booking = this.hotelBookingsRepository.All().FirstOrDefault(x => x.Id == id);
                booking.BookingStatus = BookingStatus.Deleted;

                this.hotelBookingsRepository.Update(booking);
                this.hotelBookingsRepository.Delete(booking);

                await this.hotelBookingsRepository.SaveChangesAsync();
            }

            if (type == 2)
            {
                var booking = this.vacationsBookingsRepository.All().FirstOrDefault(x => x.Id == id);
                booking.BookingStatus = BookingStatus.Deleted;

                this.vacationsBookingsRepository.Update(booking);
                this.vacationsBookingsRepository.Delete(booking);

                await this.vacationsBookingsRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetAllHotelsBookings<T>()
        {
            return this.hotelBookingsRepository.AllAsNoTrackingWithDeleted().OrderByDescending(x => x.Id).To<T>().ToList();
        }

        public IEnumerable<T> GetAllVacationBookings<T>()
        {
            return this.vacationsBookingsRepository.AllAsNoTrackingWithDeleted().OrderByDescending(x => x.Id).To<T>().ToList();
        }
    }
}
