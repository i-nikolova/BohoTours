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
    using BohoTours.Web.ViewModels.Continenst;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ContinentsServiceTest : IDisposable
    {
        private readonly EfDeletableEntityRepository<Continent> continentRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly ContinentsService continentsService;

        public ContinentsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.continentRepository = new EfDeletableEntityRepository<Continent>(this.dbContext);
            this.continentsService = new ContinentsService(this.continentRepository);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var continent = new Continent
            {
                Name = $"Continent test",
                ContinentCode = "EU",
            };

            await this.dbContext.Continents.AddAsync(continent);
            await this.dbContext.SaveChangesAsync();

            var result = this.continentsService.GetAll<ContinentViewModel>();
            Assert.Single(result);
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
                this.continentRepository.Dispose();
            }
        }
    }
}
