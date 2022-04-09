namespace BohoTours.Services.Data.Transports
{
    using System.Collections.Generic;

    public interface ITransportsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
