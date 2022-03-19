namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelInListViewModel : IMapFrom<Hotel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string TownName { get; set; }

        public string CountryName { get; set; }

        public decimal RoomMinPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Hotel, HotelInListViewModel>()
                .ForMember(
                    m => m.RoomMinPrice,
                    opt => opt.MapFrom(x => x.HotelRooms.SelectMany(r => r.HotelRoomPrices).Min(hrp => hrp.PricePerNight)))
                .ForMember(m => m.CountryName, opt => opt.MapFrom(x => x.Town.Country.Name));
        }
    }
}
