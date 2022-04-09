namespace BohoTours.Web.ViewModels.Continenst
{

    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class ContinentViewModel : IMapFrom<Continent>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
