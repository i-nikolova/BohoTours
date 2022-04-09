namespace BohoTours.Services.Data.Transports
{
    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;

    public class TransportsService : ITransportsService
    {
        private readonly IDeletableEntityRepository<Transport> transportsRepository;

        public TransportsService(IDeletableEntityRepository<Transport> transportsRepository)
        {
            this.transportsRepository = transportsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.transportsRepository.AllAsNoTracking().To<T>().ToList();
        }
    }
}
