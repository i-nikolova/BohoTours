namespace BohoTours.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using BohoTours.Data;
    using BohoTours.Data.Models;
    using BohoTours.Data.Repositories;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Vacations;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class VacationsServiceTests : IDisposable
    {
        private readonly EfDeletableEntityRepository<Vacation> vacationRepository;
        private readonly EfDeletableEntityRepository<VacationPrice> vacationPriceRepository;
        private readonly EfDeletableEntityRepository<VacationImages> vacationImagesRepository;
        private readonly EfDeletableEntityRepository<VacationRatings> vacationRatingsRepository;
        private readonly EfDeletableEntityRepository<Town> townsRepository;
        private readonly Cloudinary cloudinary;
        private readonly ApplicationDbContext dbContext;
        private readonly VacationsService vacationsService;

        public VacationsServiceTests()
        {
            EfDeletableEntityRepository<Town> townsRepository;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.vacationRepository = new EfDeletableEntityRepository<Vacation>(this.dbContext);
            this.vacationImagesRepository = new EfDeletableEntityRepository<VacationImages>(this.dbContext);
            this.vacationRatingsRepository = new EfDeletableEntityRepository<VacationRatings>(this.dbContext);
            this.vacationPriceRepository = new EfDeletableEntityRepository<VacationPrice>(this.dbContext);
            this.townsRepository = new EfDeletableEntityRepository<Town>(this.dbContext);
            this.cloudinary = new Cloudinary(new Account("di60uhnbt", "824675687264417", "R_ZOdFwj2El8AsUZKTvb0dXbgAI"));
            this.vacationsService = new VacationsService(this.vacationRepository, this.cloudinary, this.vacationPriceRepository, this.vacationRatingsRepository, this.vacationImagesRepository, this.townsRepository);

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Test Vacation Sofia", "Bulgaria"),
                CreateVacation("Test Vacation Plovdiv", "Bulgaria"),
                CreateVacation("Test Vacation Burgas", "Bulgaria"),
                CreateVacation("Test Vacation Ruse", "Bulgaria"),
            };

            var vacationSofia = CreateVacation("Test Vacation Varna", "Bulgaria");
            vacationSofia.IsDeleted = true;
            vacations.Add(vacationSofia);

            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            var result = this.vacationsService.GetAll<VacationInListViewModel>();

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetByIdWorkCorrectly()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Test Vacation Sofia", "Bulgaria"),
            };

            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            var result = this.vacationsService.GetById<VacationInListViewModel>(1);
            Assert.Equal("Test Vacation Sofia", result.Name);
        }

        [Fact]
        public async Task GetCountWorkCorrectly()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Test Vacation Sofia", "Bulgaria"),
                CreateVacation("Test Vacation Plovdiv", "Bulgaria"),
                CreateVacation("Test Vacation Burgas", "Bulgaria"),
                CreateVacation("Test Vacation Ruse", "Bulgaria"),
            };

            var vacationSofia = CreateVacation("Test Vacation Varna", "Bulgaria");
            vacationSofia.IsDeleted = true;
            vacations.Add(vacationSofia);
            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            var result = this.vacationsService.GetCount();
            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetRecommendedShouldReturnFourVacations()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Test Vacation Sofia", "Bulgaria"),
                CreateVacation("Test Vacation Plovdiv", "Bulgaria"),
                CreateVacation("Test Vacation Burgas", "Bulgaria"),
                CreateVacation("Test Vacation Ruse", "Bulgaria"),
            };

            var vacationSofia = CreateVacation("Test Vacation Varna", "Bulgaria");
            vacationSofia.IsDeleted = true;
            vacations.Add(vacationSofia);

            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            var result = this.vacationsService.GetRecommended<VacationInListViewModel>();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetRecommendedByContinentShouldReturnTwovacations()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Test Vacation Sofia", "Bulgaria"),
                CreateVacation("Test Vacation Plovdiv", "Bulgaria"),
            };

            var vacationSofia = CreateVacation("Test Vacation Varna", "Bulgaria");
            vacationSofia.IsDeleted = true;
            vacations.Add(vacationSofia);

            CreateVacation("Test Vacation Burgas", "Bulgaria");
            CreateVacation("Test Vacation Ruse", "Bulgaria");

            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            var result = this.vacationsService.GetRecommendedByContinent<VacationInListViewModel>("EU");
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            await this.dbContext.Transports.AddAsync(new Transport()
            {
                TransportType = "Bus",
            });

            await this.dbContext.Towns.AddRangeAsync(new List<Town>()
            {
                new Town()
            {
                Name = "Bansko",
                Country = new Country()
                {
                    Name = "Bulgaria",
                    Continent = new Continent()
                    {
                        ContinentCode = "EU",
                        Name = "Europe",
                    },
                },
            },
                new Town()
                {
                    Name = "Sofia",
                    Country = new Country()
                    {
                        Name = "Bulgaria",
                        Continent = new Continent()
                        {
                            ContinentCode = "EU",
                            Name = "Europe",
                        },
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

            var vacationCreateModel = new CreateVacationViewModel
            {
                Name = "Test vacation create",
                Description =
                    "test test test test test test tetest test test test test test tetest test test test test test te",
                Summary = null,
                CountryName = "bulgaria",
                TownName = "test test test",
                CountryId = 1,
                TransportId = 1,
                ContinentId = 1,
                Duration = 5,
                Images = new List<IFormFile>()
                    {
                        file,
                        file,
                    },
                Countries = new List<SelectListItem>(),
                Towns = new List<SelectListItem>(),
                Continents = new List<SelectListItem>(),
                Transports = new List<SelectListItem>(),
                VacationPrices = new List<VacationPriceViewModel>()
                    {
                        new VacationPriceViewModel()
                        {
                            Date = DateTime.Now,
                            Price = 100,
                        },
                        new VacationPriceViewModel()
                        {
                            Date = DateTime.Now,
                            Price = 200,
                        },
                    },
                TownsVisited = new int[]
                {
                        1, 2,
                },
            };

            var result = await this.vacationsService.Create(vacationCreateModel);
            var vac = this.vacationsService.GetById<VacationInListViewModel>(result);

            Assert.Equal(1, result);
            Assert.Equal("Test vacation create", this.vacationsService.GetById<VacationInListViewModel>(result).Name);
        }

        [Fact]
        public async Task EditShouldWorkCorrectly()
        {
            var vacation = CreateVacation("Ivan Rilski", "Bansko");

            await this.dbContext.Vacations.AddRangeAsync(vacation);

            await this.dbContext.SaveChangesAsync();
            var edited = new EditVacationViewModel
            {
                Id = 1,
                Name = "Editedvacation",
                Description = vacation.Description,
                Summary = vacation.Summary,
                CountryId = vacation.CountryId,
                TransportId = vacation.TransportId,
                ImagePath = "blq.jpeg",
                Duration = 10,
                ImportedImages = new List<ImportedVacationImagesViewModel>
                {
                    new ImportedVacationImagesViewModel
                    {
                        Id = 1,
                        ImageUrl = "test.jpg",
                    },
                    new ImportedVacationImagesViewModel
                    {
                        Id = 2,
                        ImageUrl = "test.jpg",
                        IsImageDeleted = true,
                    },
                },
                Countries = null,
                Towns = null,
                Continents = null,
                Transports = null,
                VacationPrices = new List<VacationPriceViewModel>()
                {
                    new VacationPriceViewModel()
                    {
                        Id = 1,
                        Date = DateTime.Now,
                        Price = 100,
                    },
                    new VacationPriceViewModel
                    {
                        Id = 2,
                        Date = DateTime.Now,
                        Price = 150,
                        IsDeleted = true,
                    },
                    new VacationPriceViewModel()
                    {
                        Id = 3,
                        Date = DateTime.Now,
                        Price = 500,
                    },
                    new VacationPriceViewModel()
                    {
                        Id = 4,
                        Date = DateTime.Now,
                        Price = 600,
                    },
                },
                TownsVisited = new int[]
                {
                    1,2,
                },
            };

            await this.vacationsService.Edit(edited);

            var vacationResult = this.vacationsService.GetById<SingleVacationViewModel>(1);
            Assert.Equal("Editedvacation", vacationResult.Name);
            Assert.Equal(3, vacationResult.VacationPrices.Count(x => !x.IsDeleted));
            Assert.Equal(1, vacationResult.Images.Count);
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectly()
        {
            var vacations = new List<Vacation>
            {
                CreateVacation("Bansko", "Bulgaria"),
                CreateVacation("Velingrad", "Bulgaria"),
            };

            await this.dbContext.Vacations.AddRangeAsync(vacations);

            await this.dbContext.SaveChangesAsync();

            await this.vacationsService.Delete(2);

            var result = this.vacationsService.GetCount();
            var vacation = this.vacationsService.GetById<VacationInListViewModel>(1);
            Assert.Equal(1, result);
            Assert.Equal("Bansko", vacation.Name);
        }

        [Fact]
        public async Task AddFeedbackShouldAddToReviews()
        {
            var vacations = new List<Vacation> { CreateVacation("Grand vacation Bansko", "Bansko") };

            await this.dbContext.Vacations.AddRangeAsync(vacations);

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

            await this.vacationsService.AddFeedback(feedBack);
            await this.vacationsService.AddFeedback(feedBackTwo);

            var result = this.vacationsService.GetById<SingleVacationViewModel>(1);

            Assert.True(result.HasReviews);
        }

        [Fact]
        public async Task GetReviewsShouldReturnCorrectReviews()
        {
            var vacations = new List<Vacation> { CreateVacation("Grand vacation Bansko", "Bansko") };

            await this.dbContext.Vacations.AddRangeAsync(vacations);

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

            await this.vacationsService.AddFeedback(feedBack);
            await this.vacationsService.AddFeedback(feedBackTwo);

            var result = this.vacationsService.GetReviews<VacationRatingsReviewViewModel>(1);

            Assert.Equal(3, result.Count());
            Assert.Equal(4, result.Average(x => x.Rating));
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
                this.vacationRepository.Dispose();
                this.vacationImagesRepository.Dispose();
                this.townsRepository.Dispose();
                this.vacationRatingsRepository.Dispose();
            }
        }

        private static Vacation CreateVacation(string name, string country)
        {
            return new Vacation
            {
                Name = name,
                Country = new Country()
                {
                    Name = country,
                    Continent = new Continent()
                    {
                        Name = "Europe",
                        ContinentCode = "EU",
                    },
                },
                Duration = 5,
                Description =
                    "Test vacation description,Test vacation description,Test vacation description,Test vacation description",
                Summary =
                    "Test vacation description,Test vacation description,Test vacation description,Test vacation description",
                ImagePath = "testVacation.jpeg",
                Transport = new Transport()
                {
                    TransportType = "Plane",
                },
                TownsVisited = new List<Town>()
                {
                    new Town()
                    {
                        Name = "Sofia",
                    },
                    new Town()
                    {
                        Name = "Plovdiv",
                    },
                },
                VacationPrices = new List<VacationPrice>()
                {
                    new VacationPrice
                    {
                        Date = DateTime.Now,
                        Price = 100,
                    },
                    new VacationPrice
                    {
                        Date = DateTime.Now,
                        Price = 150,
                    },
                },
                VacationImages = new List<VacationImages>()
                {
                    new VacationImages()
                    {
                        ImageUrl = "testImage.jpeg",
                    },
                    new VacationImages()
                    {
                        ImageUrl = "testImage2.jpeg",
                    },
                },
                VacationsRatings = new List<VacationRatings>()
                {
                    new VacationRatings
                    {
                        Rating = 5,
                        Name = "Irina",
                        Email = "irina@abv.bg",
                        Message = "Very Good!",
                    },
                },
            };
        }
    }
}
