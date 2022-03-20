﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BohoTours.Data.Common.Repositories;
using BohoTours.Data.Models;
using BohoTours.Services.Mapping;

namespace BohoTours.Services.Data.Hotels
{
    public class CountriesService : ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;

        public CountriesService(IDeletableEntityRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.countryRepository.AllAsNoTracking().To<T>().ToList();
        }

        public async Task<(int id, string name)> Create(int continentId, string name)
        {
            var country = new Country()
            {
                ContinentId = continentId,
                Name = name,
            };

            await this.countryRepository.AddAsync(country);
            await this.countryRepository.SaveChangesAsync();

            return (country.Id, country.Name);
        }
    }
}
