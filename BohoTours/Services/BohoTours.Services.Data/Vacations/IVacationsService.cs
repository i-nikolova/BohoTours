namespace BohoTours.Services.Data.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BohoTours.Web.ViewModels.Vacations;

    public interface IVacationsService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetRecommended<T>();

        int GetCount();

        T GetById<T>(int id);

        Task<int> Create(CreateVacationViewModel vacation);

        Task Edit(EditVacationViewModel vacation);

        Task Delete(int vacationId);
    }
}
