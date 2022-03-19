using System;
using System.Collections.Generic;
using System.Text;
using BohoTours.Data.Models;
using BohoTours.Services.Mapping;

namespace BohoTours.Web.ViewModels.Towns
{
    public class TownViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
