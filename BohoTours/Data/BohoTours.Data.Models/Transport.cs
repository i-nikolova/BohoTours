namespace BohoTours.Data.Models
{

    using BohoTours.Data.Common.Models;

    public class Transport : BaseDeletableModel<int>
    {
        public string TransportType { get; set; }
    }
}
