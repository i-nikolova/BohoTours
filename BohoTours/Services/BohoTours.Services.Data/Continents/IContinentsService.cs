namespace BohoTours.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IContinentsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
