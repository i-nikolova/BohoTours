namespace BohoTours.Services.Data.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Data.CloudinaryHelper;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Hotels;
    using CloudinaryDotNet;

    public class HotelsService : IHotelsService
    {
        private readonly IDeletableEntityRepository<Hotel> hotelsRepository;
        private readonly IDeletableEntityRepository<HotelRoom> hotelRoomsRepository;
        private readonly IDeletableEntityRepository<HotelRoomPrice> hotelRoomsPricesRepository;
        private readonly IDeletableEntityRepository<HotelImages> hotelImagesRepository;

        private readonly Cloudinary cloudinary;

        public HotelsService(IDeletableEntityRepository<Hotel> hotelsRepository, IDeletableEntityRepository<HotelRoom> hotelRoomsRepository, IDeletableEntityRepository<HotelRoomPrice> hotelRoomsPricesRepository, IDeletableEntityRepository<HotelImages> hotelImagesRepository, Cloudinary cloudinary)
        {
            this.hotelsRepository = hotelsRepository;
            this.hotelRoomsRepository = hotelRoomsRepository;
            this.hotelRoomsPricesRepository = hotelRoomsPricesRepository;
            this.hotelImagesRepository = hotelImagesRepository;
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
                HotelRooms = hotelModel.HotelRooms.Where(x => !x.IsDeleted).Select(x => new HotelRoom
                {
                    RoomType = x.RoomType,
                    MaxCapacity = x.MaxCapacity,
                    HotelRoomPrices = x.HotelRoomPrices.Where(hrp => !hrp.IsDeleted).Select(hrp => new HotelRoomPrice
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

        public async Task EditHotel(EditHotelViewModel hotelModel)
        {
            var hotel = this.hotelsRepository.All().FirstOrDefault(x => x.Id == hotelModel.Id);

            foreach (var room in hotelModel.HotelRooms)
            {
                var existingRoom = this.hotelRoomsRepository.All().FirstOrDefault(x => x.HotelId == hotelModel.Id && x.Id == room.Id);

                if (existingRoom == null)
                {
                    hotel.HotelRooms.Add(new HotelRoom
                    {
                        RoomType = room.RoomType,
                        MaxCapacity = room.MaxCapacity,
                        HotelRoomPrices = room.HotelRoomPrices.Where(rp => !rp.IsDeleted).Select(x => new HotelRoomPrice()
                        {
                            Date = x.Date,
                            PricePerNight = x.PricePerNight,
                        }).ToList(),
                    });
                }
                else
                {
                    if (room.IsDeleted)
                    {
                        foreach (var roomPrice in this.hotelRoomsPricesRepository.All().Where(x => x.RoomId == existingRoom.Id))
                        {
                            this.hotelRoomsPricesRepository.HardDelete(roomPrice);
                        }

                        this.hotelRoomsRepository.HardDelete(existingRoom);
                    }
                    else
                    {
                        existingRoom.RoomType = room.RoomType;
                        existingRoom.MaxCapacity = room.MaxCapacity;

                        foreach (var roomPrice in room.HotelRoomPrices)
                        {
                            var existingRoomPrice = this.hotelRoomsPricesRepository.All().FirstOrDefault(x => x.RoomId == existingRoom.Id && x.Id == roomPrice.Id);

                            if (existingRoomPrice == null)
                            {
                                existingRoom.HotelRoomPrices.Add(new HotelRoomPrice
                                {
                                    Date = roomPrice.Date,
                                    PricePerNight = roomPrice.PricePerNight,
                                });
                            }
                            else
                            {
                                if (roomPrice.IsDeleted)
                                {
                                    this.hotelRoomsPricesRepository.HardDelete(existingRoomPrice);
                                }
                            }
                        }
                    }
                }

                hotel.Name = hotelModel.Name;
                hotel.LAT = hotelModel.LAT;
                hotel.LON = hotelModel.LON;
                hotel.Description = hotelModel.Description;
                hotel.TownId = hotelModel.TownId;

                var hotelImages = this.hotelImagesRepository.All().Where(x => x.HotelId == hotelModel.Id);

                foreach (var image in hotelModel.ImportedImages)
                {
                    var existingImage = this.hotelImagesRepository.All().FirstOrDefault(x => x.HotelId == hotelModel.Id && image.Id == x.Id);

                    if (image.IsImageDeleted)
                    {
                        this.hotelImagesRepository.HardDelete(existingImage);
                    }
                }

                if (hotelModel.Images != null)
                {
                    var imageUrls = await CloudinaryExtension.UploadAsync(this.cloudinary, hotelModel.Images);
                    foreach (var item in imageUrls.Select(x => new HotelImages() { ImageUrl = x }))
                    {
                        hotel.HotelImages.Add(item);
                    }
                }

                if (hotelModel.ImportedImages.Where(x => !x.IsImageDeleted).ToList().Count == 1)
                {
                    hotel.ImagePath = hotelModel.ImportedImages.FirstOrDefault(x => !x.IsImageDeleted).ImageUrl;
                }
                else
                {
                    hotel.ImagePath = hotelModel.ImagePath;
                }

                this.hotelsRepository.Update(hotel);
                await this.hotelsRepository.SaveChangesAsync();
            }
        }
    }
}
