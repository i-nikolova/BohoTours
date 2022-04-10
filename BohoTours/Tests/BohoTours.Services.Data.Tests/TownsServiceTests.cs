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
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class TownsServiceTests : IDisposable
    {
        private readonly EfDeletableEntityRepository<Town> townRepository;
        private readonly ApplicationDbContext dbContext;

        public TownsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.townRepository = new EfDeletableEntityRepository<Town>(this.dbContext);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var towns = new List<Town>();
            for (int i = 0; i < 5; i++)
            {
                var town = new Town()
                {
                    Name = $"Town No:{i}",
                };
                towns.Add(town);
            }

            await this.dbContext.Towns.AddRangeAsync(towns);
            await this.dbContext.SaveChangesAsync();

            var service = new TownsService(this.townRepository);
            var result = service.GetAll<TownViewModel>();
            Assert.Equal(5, result.Count());
        }

        [Theory]
        [InlineData(1, "Sofia")]
        [InlineData(2, "Plovdiv")]
        [InlineData(3, "Varna")]
        public async Task CreateWorksCorrectly(int continentId, string townName)
        {
            var service = new TownsService(this.townRepository);
            var (id, name) = await service.Create(continentId, townName);

            Assert.Equal(townName, name);
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
                this.dbContext?.Dispose();
                this.townRepository.Dispose();
            }
        }
    }
}
