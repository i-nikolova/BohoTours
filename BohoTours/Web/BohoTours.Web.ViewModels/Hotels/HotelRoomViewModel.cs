﻿using System.ComponentModel.DataAnnotations;
using BohoTours.Data.Common.Constants;

namespace BohoTours.Web.ViewModels.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelRoomViewModel : IMapFrom<HotelRoom>
    {
        [Required]
        [StringLength(DataConstants.RoomTypeMaxLength, MinimumLength = DataConstants.RoomTypeMinLength)]
        public string RoomType { get; set; }

        [Range(1, 4)]
        public int MaxCapacity { get; set; }

        public bool isDeleted { get; set; }

        public ICollection<HotelRoomPriceViewModel> HotelRoomPrices { get; set; } = new List<HotelRoomPriceViewModel>();

    }
}
