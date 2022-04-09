namespace BohoTours.Web.ViewModels.Hotels
{
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class HotelRoomPriceViewModel : IMapFrom<HotelRoomPrice>
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Range(1, double.MaxValue)]
        public decimal PricePerNight { get; set; }

        public decimal PricePerPerson => this.PricePerNight * 0.70M;

        public bool IsDeleted { get; set; }
    }
}
