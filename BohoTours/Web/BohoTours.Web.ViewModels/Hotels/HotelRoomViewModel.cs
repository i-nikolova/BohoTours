namespace BohoTours.Web.ViewModels.Hotels
{
    using BohoTours.Data.Common.Constants;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class HotelRoomViewModel : IMapFrom<HotelRoom>
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        [Required]
        [StringLength(DataConstants.RoomTypeMaxLength, MinimumLength = DataConstants.RoomTypeMinLength)]
        public string RoomType { get; set; }

        [Range(1, 4)]
        public int MaxCapacity { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<HotelRoomPriceViewModel> HotelRoomPrices { get; set; } = new List<HotelRoomPriceViewModel>();
    }
}
