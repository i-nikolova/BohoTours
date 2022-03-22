using System;
using System.Collections.Generic;
using System.Text;
using BohoTours.Data.Models;
using BohoTours.Services.Mapping;

namespace BohoTours.Web.ViewModels.Continenst
{
    public class ContinentViewModel : IMapFrom<Continent>
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
