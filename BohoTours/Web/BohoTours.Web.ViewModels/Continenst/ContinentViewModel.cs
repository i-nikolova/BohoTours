namespace BohoTours.Web.ViewModels.Continenst
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class ContinentViewModel : IMapFrom<Continent>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
