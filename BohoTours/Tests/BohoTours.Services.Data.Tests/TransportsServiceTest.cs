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
    using BohoTours.Services.Data.Transports;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels;
    using BohoTours.Web.ViewModels.Continenst;
    using BohoTours.Web.ViewModels.Transports;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class TransportsServiceTest : IDisposable
    {
        private readonly EfDeletableEntityRepository<Transport> transportRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly TransportsService transportsService;

        public TransportsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.dbContext = new ApplicationDbContext(options);
            this.transportRepository = new EfDeletableEntityRepository<Transport>(this.dbContext);
            this.transportsService = new TransportsService(this.transportRepository);
        }

        [Fact]
        public async Task GetAllWorkCorrectly()
        {
            var transport = new Transport
            {
                TransportType = "Bus",
            };

            await this.dbContext.Transports.AddAsync(transport);
            await this.dbContext.SaveChangesAsync();

            var result = this.transportsService.GetAll<TransportViewModel>();
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
                this.transportRepository.Dispose();
            }
        }
    }
}
