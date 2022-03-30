namespace BohoTours.Services.Data.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Vacations;

    public interface IVacationsService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetRecommended<T>();

        IEnumerable<T> GetRecommendedByContinent<T>(string continetnCode);

        int GetCount();

        T GetById<T>(int id);

        Task<int> Create(CreateVacationViewModel vacation);

        Task Edit(EditVacationViewModel vacation);

        Task Delete(int vacationId);

        Task AddFeedback(FeedbackViewModel feedback);

        T GetReviews<T>(int hotelId);
    }
}
