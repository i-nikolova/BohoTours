namespace BohoTours.Services.Data.Bookings
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBookingsService
    {
        IEnumerable<T> GetAll<T>();

    }
}
