using System.Runtime.CompilerServices;

namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelsListViewModel
    {
        public IEnumerable<HotelInListViewModel> Hotels { get; set; }

        [Display(Name = "Search by town")]
        public string Town { get; set; }

        public IEnumerable<string> Towns { get; set; }

        public string SearchTerm { get; set; }

        [Display(Name = "Order by")]
        public HotelSorting Sorting { get; set; }

        public int PageNumber { get; set; }

        public int HotelCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public bool HasPrevious => this.PageNumber > 1;

        public bool HasNext => this.PageNumber < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.HotelCount / this.ItemsPerPage);
    }
}
