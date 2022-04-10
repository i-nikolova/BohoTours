namespace BohoTours.Services.Data.Hotels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Mapping;

    public class TownsService : ITownsService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;

        public TownsService(IDeletableEntityRepository<Town> townsRepository)
        {
            this.townsRepository = townsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.townsRepository.AllAsNoTracking().To<T>().ToList();
        }

        public async Task<(int Id, string Name)> Create(int countryId, string name)
        {
            var town = new Town()
            {
                CountryId = countryId,
                Name = name,
            };

            await this.townsRepository.AddAsync(town);
            await this.townsRepository.SaveChangesAsync();

            return (town.Id, town.Name);
        }
    }
}
