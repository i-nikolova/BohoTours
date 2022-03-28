namespace BohoTours.Services.Data.Bookings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class BookingsService : IBookingsService
    {
        private readonly IDeletableEntityRepository<Booking> bookingsRepository;

        public IEnumerable<T> GetAll<T>()
        {
            return this.bookingsRepository.AllAsNoTracking().Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).To<T>().ToList();
        }

    }
}
