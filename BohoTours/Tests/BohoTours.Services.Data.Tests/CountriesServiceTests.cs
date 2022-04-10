namespace BohoTours.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using BohoTours.Data;
    using BohoTours.Data.Models;
    using BohoTours.Data.Repositories;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels;
    using BohoTours.Web.ViewModels.Countries;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CountriesServiceTests : IDisposable
    {
        private readonly EfDeletableEntityRepository<Country> countryRepository;
        private readonly ApplicationDbContext dbContext;

        public CountriesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.countryRepository = new EfDeletableEntityRepository<Country>(this.dbContext);
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var countries = new List<Country>();
            for (int i = 0; i < 5; i++)
            {
                var country = new Country()
                {
                    Name = $"Country No:{i}",
                };
                countries.Add(country);
            }

            await this.dbContext.Countries.AddRangeAsync(countries);
            await this.dbContext.SaveChangesAsync();

            var service = new CountriesService(this.countryRepository);
            var result = service.GetAll<CountryViewModel>();
            Assert.Equal(5, result.Count());
        }

        [Theory]
        [InlineData(1, "Bulgaria")]
        [InlineData(2, "China")]
        [InlineData(3, "USA")]
        public async Task CreateWorksCorrectly(int continentId, string countryName)
        {
            var service = new CountriesService(this.countryRepository);
            var (id, name) = await service.Create(continentId, countryName);

            Assert.Equal(countryName, name);
            Assert.Equal(1, id);
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
                this.countryRepository.Dispose();
            }
        }
    }
}
