using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using BohoTours.Data.Models;
using BohoTours.Services.Mapping;

namespace BohoTours.Web.ViewModels.Countries
{
    public class CountryViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
