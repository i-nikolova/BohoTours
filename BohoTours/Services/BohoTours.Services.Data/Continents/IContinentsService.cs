using System;
using System.Collections.Generic;
using System.Text;

namespace BohoTours.Services.Data
{
    public interface IContinentsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
