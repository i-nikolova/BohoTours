namespace BohoTours.Web.ViewModels.Towns
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class TownViewModel : IMapFrom<Town>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}
