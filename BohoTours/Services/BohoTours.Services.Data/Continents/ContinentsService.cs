namespace BohoTours.Services.Data.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class ContinentsService : IContinentsService
    {
        private readonly IDeletableEntityRepository<Continent> continentsRepository;

        public ContinentsService(IDeletableEntityRepository<Continent> continentsRepository)
        {
            this.continentsRepository = continentsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.continentsRepository.AllAsNoTracking().To<T>().ToList();
        }
    }
}
