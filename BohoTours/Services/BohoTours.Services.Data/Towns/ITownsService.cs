using System;
using System.Collections.Generic;
using System.Text;

namespace BohoTours.Services.Data
{
    public interface ITownsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
