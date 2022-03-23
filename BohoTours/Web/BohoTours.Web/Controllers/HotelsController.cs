namespace BohoTours.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Data;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Web.ViewModels.Continenst;
    using BohoTours.Web.ViewModels.Countries;
    using BohoTours.Web.ViewModels.Hotels;
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

    public class HotelsController : Controller
    {
        private readonly IHotelsService hotelsService;
        private readonly ITownsService townsService;
        private readonly ICountriesService countriesService;
        private readonly IContinentsService continentsService;

        public HotelsController(IHotelsService hotelsService, ITownsService townsService, ICountriesService countriesService, IContinentsService continentsService)
        {
            this.hotelsService = hotelsService;
            this.townsService = townsService;
            this.countriesService = countriesService;
            this.continentsService = continentsService;
        }

        public IActionResult All(string town, string searchTerm, Sorting sorting, int id = 1)
        {
            var itemsPerPage = 12;

            var hotels = this.hotelsService.GetAll<HotelInListViewModel>().AsQueryable();
            var hotelsCount = this.hotelsService.GetCount();

            if (sorting != 0)
            {
                hotels = sorting switch
                {
                    Sorting.Name => hotels.OrderBy(x => x.Name),
                    Sorting.PriceAsc => hotels.OrderBy(x => x.RoomMinPrice),
                    Sorting.PriceDesc => hotels.OrderByDescending(x => x.RoomMinPrice),
                    Sorting.Country => hotels.OrderBy(x => x.TownName),
                    _ => hotels,
                };
            }

            if (!string.IsNullOrWhiteSpace(town) || !string.IsNullOrWhiteSpace(searchTerm))
            {
                if (!string.IsNullOrWhiteSpace(town))
                {
                    hotels = hotels.Where(x => x.TownName == town).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    hotels = hotels.Where(x =>
                        x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        x.CountryName.ToLower().Contains(searchTerm.ToLower()) ||
                        x.TownName.ToLower().Contains(searchTerm.ToLower())).AsQueryable();
                }

                hotelsCount = hotels.Count();
            }

            hotels = hotels.Skip((id - 1) * itemsPerPage).Take(itemsPerPage);

            var towns = this.townsService.GetAll<TownViewModel>().Select(x => x.Name);

            var viewModel = new HotelsListViewModel()
            {
                ItemsPerPage = 12,
                PageNumber = id,
                Hotels = hotels,
                HotelCount = hotelsCount,
                SearchTerm = searchTerm,
                Towns = towns,
                Town = town,
                Sorting = sorting,
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var hotel = this.hotelsService.GetById<SingleHotelViewModel>(id);

            this.ViewData["Title"] = hotel.Name;

            return this.View(hotel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateHotelViewModel()
            {
                Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }),
                Continents = this.continentsService.GetAll<ContinentViewModel>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHotelViewModel hotel)
        {
            foreach (var hotelRoom in hotel.HotelRooms.Where(x => !x.IsDeleted))
            {
                if (!hotelRoom.HotelRoomPrices.Any(x => !x.IsDeleted))
                {
                    this.ModelState.AddModelError(nameof(EditHotelViewModel.HotelRooms), $"Hotel Room Type '{hotelRoom.RoomType}' must have at least one price");
                }
            }

            if (!hotel.HotelRooms.Any(x => !x.IsDeleted))
            {
                this.ModelState.AddModelError(nameof(CreateHotelViewModel.HotelRooms), "Hotel must have at least one room");
            }

            var errors = this.ModelState.ErrorCount;

            if (!this.ModelState.IsValid || !hotel.HotelRooms.Any(x => !x.IsDeleted) || errors != 0)
            {
                hotel.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                hotel.Continents = this.continentsService.GetAll<ContinentViewModel>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });

                if (hotel.CountryId != 0)
                {
                    hotel.CountryName = hotel.Countries.FirstOrDefault(x => x.Value == hotel.CountryId.ToString()).Text;
                }

                if (hotel.TownId != 0)
                {
                    hotel.TownName = this.townsService.GetAll<TownViewModel>().FirstOrDefault(x => x.Id == hotel.TownId).Name;
                }

                hotel.Countries = hotel.Countries.Where(x => x.Value != hotel.CountryId.ToString());
                return this.View(hotel);
            }

            var hotelId = await this.hotelsService.Create(hotel);

            return this.Redirect($"/Hotels/Details/{hotelId}");
        }

        public IActionResult Edit(int id)
        {
            var hotel = this.hotelsService.GetById<EditHotelViewModel>(id);
            hotel.Continents = this.continentsService.GetAll<ContinentViewModel>().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            hotel.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            hotel.HotelRooms = hotel.HotelRooms.Where(x => !x.IsDeleted).Select(x => new HotelRoomViewModel()
            {
                Id = x.Id,
                RoomType = x.RoomType,
                MaxCapacity = x.MaxCapacity,
                HotelRoomPrices = x.HotelRoomPrices.Where(x => !x.IsDeleted).Select(x => new HotelRoomPriceViewModel()
                {
                    Date = x.Date,
                    PricePerNight = x.PricePerNight,
                    Id = x.Id,
                }).ToList(),
            }).ToList();
            hotel.ImportedImages = hotel.ImportedImages.Where(x => !x.IsImageDeleted).ToList();

            return this.View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditHotelViewModel hotel)
        {
            foreach (var hotelRoom in hotel.HotelRooms.Where(x => !x.IsDeleted))
            {
                if (!hotelRoom.HotelRoomPrices.Any(x => !x.IsDeleted))
                {
                    this.ModelState.AddModelError(nameof(EditHotelViewModel.HotelRooms), $"Hotel Room Type {hotelRoom.RoomType} must have at least one price");
                }
            }

            if (!hotel.HotelRooms.Any(x => !x.IsDeleted))
            {
                this.ModelState.AddModelError(nameof(EditHotelViewModel.HotelRooms), "Hotel must have at least one room");
            }

            if (!hotel.ImportedImages.Any(x => !x.IsImageDeleted))
            {
                this.ModelState.AddModelError(nameof(EditHotelViewModel.ImportedImages), "Hotel must have at least one image");
            }

            var errors = this.ModelState.ErrorCount;

            if (!this.ModelState.IsValid || errors != 0)
            {
                hotel.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                hotel.Continents = this.continentsService.GetAll<ContinentViewModel>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });

                if (hotel.CountryId != 0)
                {
                    hotel.CountryName = hotel.Countries.FirstOrDefault(x => x.Value == hotel.CountryId.ToString()).Text;
                }

                if (hotel.TownId != 0)
                {
                    hotel.TownName = this.townsService.GetAll<TownViewModel>().FirstOrDefault(x => x.Id == hotel.TownId).Name;
                }

                hotel.Countries = hotel.Countries.Where(x => x.Value != hotel.CountryId.ToString());
                return this.View(hotel);
            }

            await this.hotelsService.Edit(hotel);

            return this.Redirect($"/Hotels/Details/{hotel.Id}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.hotelsService.Delete(id);

            return this.Redirect($"/Hotels/All");
        }
    }
}
