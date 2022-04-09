namespace BohoTours.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        IEnumerable<T> GetAll<T>();

        Task<(int Id, string Name)> Create(int continentId, string name);
    }
}
