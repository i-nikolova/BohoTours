namespace BohoTours.Web.ViewModels.Transports
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class TransportViewModel : IMapFrom<Transport>
    {
        public int Id { get; set; }

        public string TransportType { get; set; }
    }
}
