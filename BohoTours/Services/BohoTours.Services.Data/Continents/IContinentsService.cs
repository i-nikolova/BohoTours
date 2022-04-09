namespace BohoTours.Services.Data
{
    using System.Collections.Generic;

    public interface IContinentsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
