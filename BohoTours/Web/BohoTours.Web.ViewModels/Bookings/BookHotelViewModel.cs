using BohoTours.Web.ViewModels.Hotels;
using System;

namespace BohoTours.Web.ViewModels.Bookings
{
    public class BookHotelViewModel
    {
        public int HotelRoomPriceId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Duration { get; set; }

        public DateTime StartDate { get; set; }

    }
}
