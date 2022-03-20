using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BohoTours.Services.Data
{
    public interface ICountriesService
    {
        IEnumerable<T> GetAll<T>();

        Task<(int id, string name)> Create(int continentId, string name);
    }
}
