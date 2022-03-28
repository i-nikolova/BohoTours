namespace BohoTours.Services.Data.Hotels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Hotels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Migrations.Operations;

    public interface IHotelsService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<string> GetAllHotelTowns();

        IEnumerable<T> GetRecommended<T>();

        int GetCount();

        T GetById<T>(int id);

        Task<int> Create(CreateHotelViewModel hotel);

        Task Edit(EditHotelViewModel hotel);

        Task Delete(int hotelId);

        Task AddFeedback(FeedbackViewModel feedback);

        T GetReviews<T>(int hotelId);
    }
}
