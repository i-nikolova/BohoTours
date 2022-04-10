namespace BohoTours.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using BohoTours.Data;
    using BohoTours.Data.Models;
    using BohoTours.Data.Repositories;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Hotels;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class HotelsServicesTests : IDisposable
    {
        private readonly EfDeletableEntityRepository<Hotel> hotelRepository;
        private readonly EfDeletableEntityRepository<HotelRoom> hotelRoomRepository;
        private readonly EfDeletableEntityRepository<HotelRoomPrice> hotelRoomPricelRepository;
        private readonly EfDeletableEntityRepository<HotelImages> hotelImagesRepository;
        private readonly EfDeletableEntityRepository<HotelRatings> hotelRatingsRepository;
        private readonly Cloudinary cloudinary;
        private readonly ApplicationDbContext dbContext;
        private readonly HotelsService hotelsService;

        public HotelsServicesTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.hotelRepository = new EfDeletableEntityRepository<Hotel>(this.dbContext);
            this.hotelImagesRepository = new EfDeletableEntityRepository<HotelImages>(this.dbContext);
            this.hotelRatingsRepository = new EfDeletableEntityRepository<HotelRatings>(this.dbContext);
            this.hotelRoomPricelRepository = new EfDeletableEntityRepository<HotelRoomPrice>(this.dbContext);
            this.hotelRoomRepository = new EfDeletableEntityRepository<HotelRoom>(this.dbContext);
            this.cloudinary = new Cloudinary(new Account("di60uhnbt", "824675687264417", "R_ZOdFwj2El8AsUZKTvb0dXbgAI"));
            this.hotelsService = new HotelsService(this.hotelRepository, this.hotelRoomRepository, this.hotelRoomPricelRepository, this.hotelImagesRepository, this.cloudinary, this.hotelRatingsRepository);

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Veligrad"),
            };

            var hotelSofia = CreateHotel("Sofia", "Sofia");
            hotelSofia.IsDeleted = true;
            hotels.Add(hotelSofia);

            hotels.Add(CreateHotel("Hemus", "Sofia"));
            hotels.Add(CreateHotel("Infinity", "Velingrad"));

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetAll<HotelInListViewModel>();

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetByIdWorkCorrectly()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
                CreateHotel("Sofia", "Sofia"),
                CreateHotel("Hemus", "Sofia"),
                CreateHotel("Infinity", "Velingrad"),
            };

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetById<SingleHotelViewModel>(3);
            Assert.Equal("Sofia", result.Name);
        }

        [Fact]
        public async Task GetCountWorkCorrectly()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
            };

            var hotelSofia = CreateHotel("Sofia", "Sofia");
            hotelSofia.IsDeleted = true;
            hotels.Add(hotelSofia);

            hotels.Add(CreateHotel("Hemus", "Sofia"));
            hotels.Add(CreateHotel("Infinity", "Velingrad"));

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetCount();
            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetAllHotelTownsWorkCorrectly()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
            };

            var hotelSofia = CreateHotel("Sofia", "Sofia");
            hotelSofia.IsDeleted = true;
            hotels.Add(hotelSofia);

            hotels.Add(CreateHotel("Hemus", "Sofia"));
            hotels.Add(CreateHotel("Infinity", "Velingrad"));

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetAllHotelTowns();
            Assert.Equal(3, result.Count());
            Assert.Contains("Sofia", result);
            Assert.Contains("Bansko", result);
            Assert.Contains("Velingrad", result);
        }

        [Fact]
        public async Task GetRecommendedShouldReturnFourHotels()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
            };

            var hotelSofia = CreateHotel("Sofia", "Sofia");
            hotelSofia.IsDeleted = true;
            hotels.Add(hotelSofia);

            hotels.Add(CreateHotel("Hemus", "Sofia"));
            hotels.Add(CreateHotel("Infinity", "Velingrad"));

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetRecommended<HotelInListViewModel>();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetRecommendedByContinentShouldReturnTwoHotels()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
            };

            var hotelSofia = CreateHotel("Sofia", "Sofia");
            hotelSofia.IsDeleted = true;
            hotels.Add(hotelSofia);

            hotels.Add(CreateHotel("Hemus", "Sofia"));
            hotels.Add(CreateHotel("Infinity", "Velingrad"));

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var result = this.hotelsService.GetRecommendedByContinent<HotelInListViewModel>("EU");
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            await this.dbContext.Towns.AddRangeAsync(new Town()
            {
                Name = "Bansko",
                Country = new Country()
                {
                    Name = "Bulgaria",
                    Continent = new Continent()
                    {
                        ContinentCode = "EU",
                        Name = "Eutope",
                    },
                },
            });

            await this.dbContext.SaveChangesAsync();

            await using var stream = File.OpenRead("africa.png");
            FormFile file = new(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png",
            };

            var hotelCreateModel = new CreateHotelViewModel
            {
                Name = "Grand Hotel Bansko",
                Description = "test test test test test test tetest test test test test test tetest test test test test test te",
                TownId = 1,
                CountryName = "bulgaria",
                TownName = "test test test",
                CountryId = 1,
                ContinentId = 1,
                LAT = "10.025534",
                LON = "10.025534",
                Images = new List<IFormFile>()
                    {
                        file,
                        file,
                    },
                Countries = new List<SelectListItem>(),
                Continents = new List<SelectListItem>(),
                HotelRooms = new List<HotelRoomViewModel>()
                    {
                        new HotelRoomViewModel()
                        {
                            RoomType = "Standart",
                            MaxCapacity = 2,
                            HotelRoomPrices = new List<HotelRoomPriceViewModel>()
                            {
                                new HotelRoomPriceViewModel()
                                {
                                    Date = DateTime.Now,
                                    PricePerNight = 100,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Date = DateTime.Now,
                                    PricePerNight = 150,
                                },
                            },
                        },
                        new HotelRoomViewModel()
                        {
                            RoomType = "Premium",
                            MaxCapacity = 2,
                            HotelRoomPrices = new List<HotelRoomPriceViewModel>()
                            {
                                new HotelRoomPriceViewModel()
                                {
                                    Date = DateTime.Now,
                                    PricePerNight = 1000,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Date = DateTime.Now,
                                    PricePerNight = 1500,
                                },
                            },
                        },
                    },
            };

            var result = await this.hotelsService.Create(hotelCreateModel);

            Assert.Equal(1, result);
            Assert.Equal("Grand Hotel Bansko", this.hotelsService.GetById<HotelInListViewModel>(result).Name);
        }

        [Fact]
        public async Task EditShouldWorkCorrectly()
        {
            var hotel = CreateHotel("Ivan Rilski", "Bansko");

            await this.dbContext.Hotels.AddRangeAsync(hotel);

            await this.dbContext.SaveChangesAsync();
            var edited = new EditHotelViewModel
            {
                Id = 1,
                Name = "EditedHotel",
                LAT = hotel.LAT,
                LON = hotel.LON,
                Description = hotel.Description,
                TownId = 1,
                ImagePath = "blq.jpeg",
                ImportedImages = new List<ImportedHotelsImagesViewModel>
                {
                    new ImportedHotelsImagesViewModel
                    {
                        Id = 1,
                        ImageUrl = "test.jpg",
                        IsImageDeleted = true,
                    },
                    new ImportedHotelsImagesViewModel
                    {
                        Id = 2,
                        ImageUrl = "test1.jpg",
                        IsImageDeleted = true,
                    },
                    new ImportedHotelsImagesViewModel
                    {
                        Id = 3,
                        ImageUrl = "test2.jpg",
                        IsImageDeleted = false,
                    },
                },
                HotelRooms = new List<HotelRoomViewModel>()
                {
                    new HotelRoomViewModel()
                        {
                            Id = 1,
                            RoomType = "Standart",
                            MaxCapacity = 4,
                            HotelRoomPrices = new List<HotelRoomPriceViewModel>()
                            {
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 1,
                                    PricePerNight = 100,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 2,
                                    PricePerNight = 500,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 3,
                                    PricePerNight = 120,
                                    Date = DateTime.Now,
                                    IsDeleted = true,
                                },
                            },
                            IsDeleted = true,
                        },
                    new HotelRoomViewModel()
                        {
                            Id = 2,
                            RoomType = "Premium",
                            MaxCapacity = 4,
                            HotelRoomPrices = new List<HotelRoomPriceViewModel>()
                            {
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 1,
                                    PricePerNight = 1000,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 2,
                                    PricePerNight = 5000,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPriceViewModel()
                                {
                                    Id = 3,
                                    PricePerNight = 1200,
                                    Date = DateTime.Now,
                                },
                            },
                        },
                    new HotelRoomViewModel()
                    {
                        Id = 3,
                        RoomType = "Test",
                        MaxCapacity = 4,
                        HotelRoomPrices = new List<HotelRoomPriceViewModel>()
                        {
                            new HotelRoomPriceViewModel()
                            {
                                Id = 1,
                                PricePerNight = 1000,
                                Date = DateTime.Now,
                            },
                            new HotelRoomPriceViewModel()
                            {
                                Id = 2,
                                PricePerNight = 5000,
                                Date = DateTime.Now,
                            },
                            new HotelRoomPriceViewModel()
                            {
                                Id = 3,
                                PricePerNight = 1200,
                                Date = DateTime.Now,
                            },
                        },
                    },
                },
            };

            await this.hotelsService.Edit(edited);

            var hotelResult = this.hotelsService.GetById<SingleHotelViewModel>(1);
            Assert.Equal("EditedHotel", hotelResult.Name);
            Assert.Contains(hotelResult.HotelRooms, x => x.RoomType == "Test");
            Assert.Equal(2, hotelResult.HotelRooms.Count(x => !x.IsDeleted));
            Assert.Equal(1, hotelResult.Images.Count);
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectly()
        {
            var hotels = new List<Hotel>
            {
                CreateHotel("Grand Hotel Bansko", "Bansko"),
                CreateHotel("Grand Hotel Velingrad", "Velingrad"),
            };

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            await this.hotelsService.Delete(2);

            var result = this.hotelsService.GetCount();
            var hotel = this.hotelsService.GetById<HotelInListViewModel>(1);
            Assert.Equal(1, result);
            Assert.Equal("Grand Hotel Bansko", hotel.Name);
        }

        [Fact]
        public async Task AddFeedbackShouldAddToReviews()
        {
            var hotels = new List<Hotel> { CreateHotel("Grand Hotel Bansko", "Bansko") };

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var feedBack = new FeedbackViewModel
            {
                ModelId = 1,
                Rating = 5,
                Name = "Irina",
                Email = "irina@abv.bg",
                Message = "Good!",
            };

            var feedBackTwo = new FeedbackViewModel
            {
                ModelId = 1,
                Rating = 1,
                Name = "Ivan",
                Email = "ivan@abv.bg",
                Message = "Bad!",
            };

            await this.hotelsService.AddFeedback(feedBack);
            await this.hotelsService.AddFeedback(feedBackTwo);

            var result = this.hotelsService.GetById<SingleHotelViewModel>(1);

            Assert.True(result.HasReviews);
        }

        [Fact]
        public async Task GetReviewsShouldReturnCorrectReviews()
        {
            var hotels = new List<Hotel> { CreateHotel("Grand Hotel Bansko", "Bansko") };

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var feedBack = new FeedbackViewModel
            {
                ModelId = 1,
                Rating = 5,
                Name = "Irina",
                Email = "irina@abv.bg",
                Message = "Good!",
            };

            var feedBackTwo = new FeedbackViewModel
            {
                ModelId = 1,
                Rating = 2,
                Name = "Ivan",
                Email = "ivan@abv.bg",
                Message = "Bad!",
            };

            await this.hotelsService.AddFeedback(feedBack);
            await this.hotelsService.AddFeedback(feedBackTwo);

            var result = this.hotelsService.GetReviews<HotelRatingsReviewViewModel>(1);

            Assert.Equal(3, result.Count());
            Assert.Equal(4, result.Average(x => x.Rating));
        }

        [Fact]
        public async Task GetRoomInfoShouldReturnCorectInfo()
        {
            var hotels = new List<Hotel> { CreateHotel("Grand Hotel Bansko", "Bansko") };

            await this.dbContext.Hotels.AddRangeAsync(hotels);

            await this.dbContext.SaveChangesAsync();

            var (hotelName, roomType) = this.hotelsService.GetRoomInfo(1);

            Assert.Equal("Grand Hotel Bansko", hotelName);
            Assert.Equal("Standart", roomType);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.dbContext?.Database.EnsureDeleted();
                this.dbContext?.Dispose();
                this.hotelRepository.Dispose();
                this.hotelImagesRepository.Dispose();
                this.hotelRoomRepository.Dispose();
                this.hotelRoomPricelRepository.Dispose();
                this.hotelRatingsRepository.Dispose();
            }
        }

        private static Hotel CreateHotel(string name, string town)
        {
            return new Hotel()
            {
                Name = name,
                Description = $"Description for this hotel",
                IsDeleted = false,
                LAT = "10.255555",
                LON = "10.255555",
                HotelImages = new List<HotelImages>()
                    {
                        new HotelImages()
                            {
                                ImageUrl = "test.jpg",
                            },
                        new HotelImages()
                        {
                            ImageUrl = "test1.jpg",
                        },
                        new HotelImages()
                        {
                            ImageUrl = "test2.jpg",
                        },
                    },
                HotelRooms = new List<HotelRoom>()
                    {
                        new HotelRoom()
                        {
                            RoomType = "Standart",
                            MaxCapacity = 4,
                            HotelRoomPrices = new List<HotelRoomPrice>()
                            {
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 100,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 500,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 120,
                                    Date = DateTime.Now,
                                },
                            },
                        },
                        new HotelRoom()
                        {
                            RoomType = "Premium",
                            MaxCapacity = 4,
                            HotelRoomPrices = new List<HotelRoomPrice>()
                            {
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 1000,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 5000,
                                    Date = DateTime.Now,
                                },
                                new HotelRoomPrice()
                                {
                                    PricePerNight = 1200,
                                    Date = DateTime.Now,
                                },
                            },
                        },
                    },
                Town = new Town()
                {
                    Name = town,
                    Country = new Country()
                    {
                        Name = "Bulgaria",
                        Continent = new Continent()
                        {
                            Name = "Europe",
                            ContinentCode = "EU",
                        },
                    },
                },
                HotelRatings = new List<HotelRatings>()
                    {
                        new HotelRatings()
                        {
                            Name = "Irina",
                            Message = "Perfect",
                            Rating = 5,
                            Email = "irina@abv.bg",
                        },
                    },
            };
        }
    }
}
