namespace BohoTours.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using BohoTours.Data;
    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Data.Repositories;
    using BohoTours.Services.Data.Bookings;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.Areas.Administration.Models.Bookings;
    using BohoTours.Web.ViewModels;
    using BohoTours.Web.ViewModels.Bookings;
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class BookingsServiceTests : IDisposable
    {
        private readonly IDeletableEntityRepository<HotelBooking> hotelBookingsRepository;
        private readonly IDeletableEntityRepository<VacationBooking> vacationsBookingsRepository;
        private readonly IDeletableEntityRepository<HotelRoomPrice> hotelRoomPriceRepository;
        private readonly IDeletableEntityRepository<VacationPrice> vacationPriceRepository;
        private readonly ApplicationDbContext dbContext;

        public BookingsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);

            this.hotelBookingsRepository = new EfDeletableEntityRepository<HotelBooking>(this.dbContext);
            this.vacationsBookingsRepository = new EfDeletableEntityRepository<VacationBooking>(this.dbContext);
            this.hotelRoomPriceRepository = new EfDeletableEntityRepository<HotelRoomPrice>(this.dbContext);
            this.vacationPriceRepository = new EfDeletableEntityRepository<VacationPrice>(this.dbContext);
        }

        [Fact]
        public void GetAllHotelBookingsWorkCorrectly()
        {
            var mockRepository = new Mock<IDeletableEntityRepository<HotelBooking>>();

            mockRepository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(new List<HotelBooking>()
            {
                new HotelBooking
                {
                    Id = 1,
                    IsDeleted = false,
                    FirstName = "Irina",
                    LastName = "Nikolova",
                    Email = "irina@abv.bg",
                    Price = 150,
                    Duration = 5,
                    StartDate = default,
                    EndDate = default,
                    HotelRoomPriceId = 1,
                    Hotel = new HotelRoomPrice
                    {
                        Room = new HotelRoom
                        {
                            RoomType = "Test room type",
                            Hotel = new Hotel()
                            {
                                Name = "Test",
                            },
                        },
                    },
                    BookingStatus = (BookingStatus)0,
                },
            }.AsQueryable());

            var service = new BookingsService(mockRepository.Object, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            var result = service.GetAllHotelsBookings<HotelBookingViewModel>();
            Assert.Single(result);
        }

        [Fact]
        public void GetAllVacationBookingsWorkCorrectly()
        {
            var mockRepository = new Mock<IDeletableEntityRepository<VacationBooking>>();

            mockRepository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(new List<VacationBooking>()
            {
                new VacationBooking()
                {
                    Id = 1,
                    IsDeleted = false,
                    FirstName = "Irina",
                    LastName = "Nikolova",
                    Email = "irina@abv.bg",
                    Price = 150,
                    People = 5,
                    StartDate = default,
                    EndDate = default,
                    VacationPriceId = 1,
                    VacationPrice = new VacationPrice
                    {
                        Vacation = new Vacation()
                        {
                            Name = "test",
                        },
                        Date = default,
                        Price = 150,
                    },
                    BookingStatus = (BookingStatus)0,
                },
            }.AsQueryable());

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, mockRepository.Object);

            var result = service.GetAllVacationBookings<VacationBookingViewModel>();
            Assert.Single(result);
        }

        [Fact]
        public async Task ConfirmShouldchangeStatusForHotelBookings()
        {
            var hotelRoom = new HotelRoomPrice
            {
                Id = 1,
                Room = new HotelRoom()
                {
                    RoomType = "test",
                    MaxCapacity = 1,
                    Hotel = new Hotel()
                    {
                        Name = "tralala",
                    },
                },
                Date = default,
                PricePerNight = 100,
            };

            var booking = new HotelBooking()
            {
                Id = 1,
                IsDeleted = false,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                Duration = 5,
                StartDate = default,
                EndDate = default,
                HotelRoomPriceId = 1,
                Hotel = hotelRoom,
                BookingStatus = (BookingStatus)1,
            };

            await this.hotelRoomPriceRepository.AddAsync(hotelRoom);
            await this.hotelBookingsRepository.AddAsync(booking);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Confirm(1, 1);

            var result = service.GetAllHotelsBookings<HotelBookingViewModel>().FirstOrDefault().BookingStatus;

            Assert.Equal((BookingStatus)2, result);
        }

        [Fact]
        public async Task ConfirmShouldchangeStatusForVacationBookings()
        {
            var vacationPrice = new VacationPrice()
            {
                Id = 1,
                Date = default,
                Price = 100,
                Vacation = new Vacation()
                {
                    Name = "tralalal",
                },
            };

            var booking = new VacationBooking()
            {
                Id = 1,
                IsDeleted = false,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                People = 5,
                StartDate = default,
                EndDate = default,
                VacationPriceId = 1,
                VacationPrice = vacationPrice,
                BookingStatus = (BookingStatus)1,
            };

            await this.vacationPriceRepository.AddAsync(vacationPrice);
            await this.vacationsBookingsRepository.AddAsync(booking);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Confirm(1, 2);

            var result = service.GetAllVacationBookings<VacationBookingViewModel>().FirstOrDefault().BookingStatus;

            Assert.Equal((BookingStatus)2, result);
        }

        [Fact]
        public async Task DeleteShouldchangeStatusForHotelBookings()
        {
            var hotelRoom = new HotelRoomPrice
            {
                Id = 1,
                Room = new HotelRoom()
                {
                    RoomType = "test",
                    MaxCapacity = 1,
                    Hotel = new Hotel()
                    {
                        Name = "tralala",
                    },
                },
                Date = default,
                PricePerNight = 100,
            };

            var booking = new HotelBooking()
            {
                Id = 1,
                IsDeleted = false,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                Duration = 5,
                StartDate = default,
                EndDate = default,
                HotelRoomPriceId = 1,
                Hotel = hotelRoom,
                BookingStatus = (BookingStatus)1,
            };

            await this.hotelRoomPriceRepository.AddAsync(hotelRoom);
            await this.hotelBookingsRepository.AddAsync(booking);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Delete(1, 1);

            var result = service.GetAllHotelsBookings<HotelBookingViewModel>().FirstOrDefault().BookingStatus;

            Assert.Equal((BookingStatus)3, result);
        }

        [Fact]
        public async Task DeleteShouldchangeStatusForVacationBookings()
        {
            var vacationPrice = new VacationPrice()
            {
                Id = 1,
                Date = default,
                Price = 100,
                Vacation = new Vacation()
                {
                    Name = "tralalal",
                },
            };

            var booking = new VacationBooking()
            {
                Id = 1,
                IsDeleted = false,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                People = 5,
                StartDate = default,
                EndDate = default,
                VacationPriceId = 1,
                VacationPrice = vacationPrice,
                BookingStatus = (BookingStatus)1,
            };

            await this.vacationPriceRepository.AddAsync(vacationPrice);
            await this.vacationsBookingsRepository.AddAsync(booking);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Delete(1, 2);

            var result = service.GetAllVacationBookings<VacationBookingViewModel>().FirstOrDefault().BookingStatus;

            Assert.Equal((BookingStatus)3, result);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectlyForHotelBookings()
        {
            var hotelRoom = new HotelRoomPrice
            {
                Id = 1,
                Room = new HotelRoom()
                {
                    RoomType = "test",
                    MaxCapacity = 1,
                    Hotel = new Hotel()
                    {
                        Name = "tralala",
                    },
                },
                Date = default,
                PricePerNight = 100,
            };

            var booking = new CreateHotelBookingViewModel()
            {
                EntityId = 1,
                EntityPriceId = 1,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                Duration = 5,
                StartDate = default,
                EndDate = default,
            };

            await this.hotelRoomPriceRepository.AddAsync(hotelRoom);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Create(booking);

            var result = service.GetAllHotelsBookings<HotelBookingViewModel>().FirstOrDefault().BookingStatus;

            Assert.Equal((BookingStatus)1, result);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectlyForVacationBookings()
        {
            var vacationPrice = new VacationPrice()
            {
                Id = 1,
                Date = default,
                Price = 100,
                Vacation = new Vacation()
                {
                    Name = "tralalal",
                },
            };

            var booking = new CreateVacationBookingViewModel()
            {
                EntityId = 1,
                EntityPriceId = 1,
                FirstName = "Irina",
                LastName = "Nikolova",
                Email = "irina@abv.bg",
                Price = 150,
                People = 5,
                StartDate = default,
                EndDate = default,
            };

            await this.vacationPriceRepository.AddAsync(vacationPrice);
            await this.dbContext.SaveChangesAsync();

            var service = new BookingsService(this.hotelBookingsRepository, this.hotelRoomPriceRepository, this.vacationPriceRepository, this.vacationsBookingsRepository);

            await service.Create(booking);

            var result = service.GetAllVacationBookings<VacationBookingViewModel>().FirstOrDefault()!.BookingStatus;

            Assert.Equal((BookingStatus)1, result);
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
                this.dbContext?.Dispose();
                this.hotelBookingsRepository.Dispose();
                this.hotelRoomPriceRepository.Dispose();
                this.vacationPriceRepository.Dispose();
                this.vacationsBookingsRepository.Dispose();
            }
        }
    }
}
