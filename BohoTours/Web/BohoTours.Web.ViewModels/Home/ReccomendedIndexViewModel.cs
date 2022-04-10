namespace BohoTours.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Vacations;

    public class ReccomendedIndexViewModel
    {
        public IEnumerable<HotelInListViewModel> Hotels { get; set; }

        public IEnumerable<VacationInListViewModel> Vacations { get; set; }
    }
}
