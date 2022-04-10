namespace BohoTours.Web.ViewModels.Hotels
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class ImportedHotelsImagesViewModel : IMapFrom<HotelImages>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public bool IsImageDeleted { get; set; }
    }
}
