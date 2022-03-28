namespace BohoTours.Web.ViewModels.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Models;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class VacationsListViewModel
    {
        public IEnumerable<VacationInListViewModel> Vacations { get; set; }

        [Display(Name = "Search by town")]
        public int[] Towns { get; set; }

        [Display(Name = "Search by country")]
        public int Country { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> TownsList { get; set; }

        public int[] SelectedTowns { get; set; }

        public string SearchTerm { get; set; }

        [Display(Name = "Order by")]
        public Sorting Sorting { get; set; }

        public int PageNumber { get; set; }

        public int VacationsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public bool HasPrevious => this.PageNumber > 1;

        public bool HasNext => this.PageNumber < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.VacationsCount / this.ItemsPerPage);
    }
}
