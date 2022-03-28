using BohoTours.Data.Models;
using BohoTours.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BohoTours.Web.ViewModels.Vacations
{
    public class ImportedVacationImagesViewModel : IMapFrom<VacationImages>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsImageDeleted { get; set; }
    }
}
