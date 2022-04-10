namespace BohoTours.Web.ViewModels.Vacations
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class ImportedVacationImagesViewModel : IMapFrom<VacationImages>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsImageDeleted { get; set; }
    }
}
