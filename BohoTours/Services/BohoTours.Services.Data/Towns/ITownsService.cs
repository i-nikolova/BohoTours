using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BohoTours.Services.Data
{
    public interface ITownsService
    {
        IEnumerable<T> GetAll<T>();

        Task<(int id, string name)> Create(int countryId, string name);
    }
}
