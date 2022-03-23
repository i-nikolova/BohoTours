namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

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
