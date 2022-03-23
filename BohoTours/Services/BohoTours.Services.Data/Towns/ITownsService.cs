namespace BohoTours.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITownsService
    {
        IEnumerable<T> GetAll<T>();

        Task<(int Id, string Name)> Create(int countryId, string name);
    }
}
