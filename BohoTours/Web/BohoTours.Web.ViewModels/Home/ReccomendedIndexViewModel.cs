using BohoTours.Web.ViewModels.Hotels;
using BohoTours.Web.ViewModels.Vacations;
using System.Collections.Generic;

namespace BohoTours.Web.ViewModels.Home
{
    public class ReccomendedIndexViewModel
    {
        public IEnumerable<HotelInListViewModel> Hotels { get; set; }
        public IEnumerable<VacationInListViewModel> Vacations { get; set; }
    }
}
