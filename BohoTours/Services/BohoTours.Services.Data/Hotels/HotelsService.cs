using System.Threading.Tasks;
using BohoTours.Services.Data.CloudinaryHelper;
using BohoTours.Web.ViewModels.Hotels;
using CloudinaryDotNet;

namespace BohoTours.Services.Data.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class HotelsService : IHotelsService
    {
        private readonly IDeletableEntityRepository<Hotel> hotelsRepository;
        private readonly Cloudinary cloudinary;

        public HotelsService(IDeletableEntityRepository<Hotel> hotelsRepository, Cloudinary cloudinary)
        {
            this.hotelsRepository = hotelsRepository;
            this.cloudinary = cloudinary;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.hotelsRepository.AllAsNoTracking().OrderByDescending(x => x.Id).To<T>().ToList();
        }

        public IEnumerable<string> GetAllHotelTowns()
        {
            return this.hotelsRepository.AllAsNoTracking().Select(x => x.Town.Name).Distinct().ToList();
        }

        public IEnumerable<T> GetRecommended<T>()
        {
            var random = new Random();
            var list = this.GetAll<T>().ToArray();
            var recommendedHotels = new List<T>();

            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(list.Count());
                recommendedHotels.Add(list[index]);
            }

            return recommendedHotels;
        }

        public int GetCount()
        {
            return this.hotelsRepository.AllAsNoTracking().Count();
        }

        public T GetById<T>(int id)
        {
            return this.hotelsRepository.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public async Task<int> CreateHotel(CreateHotelViewModel hotelModel)
        {
            var imageUrls = await CloudinaryExtension.UploadAsync(this.cloudinary, hotelModel.Images);

            var hotel = new Hotel
            {
                Name = hotelModel.Name,
                LAT = hotelModel.LAT,
                LON = hotelModel.LON,
                Description = hotelModel.Description,
                ImagePath = imageUrls.First(),
                TownId = hotelModel.TownId,
                HotelRooms = hotelModel.HotelRooms.Where(x => !x.isDeleted).Select(x => new HotelRoom
                {
                    RoomType = x.RoomType,
                    MaxCapacity = x.MaxCapacity,
                    HotelRoomPrices = x.HotelRoomPrices.Where(hrp => !hrp.isDeleted).Select(hrp => new HotelRoomPrice
                    {
                        Date = hrp.Date,
                        PricePerNight = hrp.PricePerNight,
                    }).ToList(),
                }).ToList(),
                HotelImages = imageUrls.Select(x => new HotelImages()
                {
                    ImageUrl = x,
                }).ToList(),
            };

            await this.hotelsRepository.AddAsync(hotel);
            await this.hotelsRepository.SaveChangesAsync();

            return hotel.Id;
        }
    }
}
