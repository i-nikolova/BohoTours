namespace BohoTours.Services.Data.Transports
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITransportsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
