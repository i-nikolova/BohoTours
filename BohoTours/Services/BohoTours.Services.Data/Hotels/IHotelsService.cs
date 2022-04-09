namespace BohoTours.Services.Data.Hotels
{
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Hotels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHotelsService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<string> GetAllHotelTowns();

        IEnumerable<T> GetRecommended<T>();

        IEnumerable<T> GetRecommendedByContinent<T>(string continetnCode);

        int GetCount();

        T GetById<T>(int id);

        Task<int> Create(CreateHotelViewModel hotel);

        Task Edit(EditHotelViewModel hotel);

        Task Delete(int hotelId);

        Task AddFeedback(FeedbackViewModel feedback);

        IEnumerable<T> GetReviews<T>(int hotelId);

        (string HotelName, string RoomType) GetRoomInfo(int id);
    }
}
