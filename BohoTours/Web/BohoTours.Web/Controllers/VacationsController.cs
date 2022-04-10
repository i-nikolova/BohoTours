namespace BohoTours.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Models;
    using BohoTours.Services.Data;
    using BohoTours.Services.Data.Transports;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Countries;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Towns;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Routing;

    public class VacationsController : Controller
    {
        private readonly ICountriesService countriesService;
        private readonly ITownsService townsService;
        private readonly IVacationsService vacationsService;

        public VacationsController(ICountriesService countriesService, ITownsService townsService, IVacationsService vacationsService)
        {
            this.countriesService = countriesService;
            this.townsService = townsService;
            this.vacationsService = vacationsService;
        }

        public IActionResult All(int country, int[] towns, string searchTerm, Sorting sorting, int id = 1)
        {
            var itemsPerPage = 12;

            var vacations = this.vacationsService.GetAll<VacationInListViewModel>().AsQueryable();
            var vacationsCount = this.vacationsService.GetCount();

            if (sorting != 0)
            {
                vacations = sorting switch
                {
                    Sorting.Name => vacations.OrderBy(x => x.Name),
                    Sorting.PriceAsc => vacations.OrderBy(x => x.VacationMinPrice),
                    Sorting.PriceDesc => vacations.OrderByDescending(x => x.VacationMinPrice),
                    Sorting.Country => vacations.OrderBy(x => x.CountryName),
                    _ => vacations,
                };
            }

            if (country != 0 || !string.IsNullOrWhiteSpace(searchTerm) || towns.Length != 0)
            {
                if (country != 0)
                {
                    vacations = vacations.Where(x => x.CountryId == country).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    vacations = vacations.Where(x =>
                        x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        x.CountryName.ToLower().Contains(searchTerm.ToLower())).AsQueryable();
                }

                if (towns.Length != 0)
                {
                    var townsListSelected = new List<string>();

                    foreach (var townId in towns)
                    {
                        townsListSelected.Add(this.townsService.GetAll<TownViewModel>().Where(x => x.Id == townId).Select(x => x.Name).FirstOrDefault());
                    }

                    vacations = vacations.Where(x => x.TownsVisited.Split(", ", StringSplitOptions.RemoveEmptyEntries).Intersect(townsListSelected).Count() == townsListSelected.Count);
                }

                vacationsCount = vacations.Count();
            }

            vacations = vacations.Skip((id - 1) * itemsPerPage).Take(itemsPerPage);

            var countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });
            var townsListToAdd = this.townsService.GetAll<TownViewModel>().Where(x => x.CountryId == country).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            var viewModel = new VacationsListViewModel()
            {
                ItemsPerPage = 12,
                PageNumber = id,
                Vacations = vacations,
                VacationsCount = vacationsCount,
                SearchTerm = searchTerm,
                Country = country,
                Countries = countries,
                Sorting = sorting,
                TownsList = townsListToAdd,
                SelectedTowns = towns,
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int id, FeedbackViewModel feedback)
        {
            if (feedback.ModelId != 0)
            {
                id = feedback.ModelId;
            }

            var vacation = this.vacationsService.GetById<SingleVacationViewModel>(id);

            if (feedback == null)
            {
                vacation.Feedback = new FeedbackViewModel();
            }
            else
            {
                vacation.Feedback = feedback;
            }

            this.ViewData["Title"] = vacation.Name;

            return this.View(vacation);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LeaveFeedback(FeedbackViewModel feedback)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction($"Details", new RouteValueDictionary(feedback));
            }

            await this.vacationsService.AddFeedback(feedback);
            return this.RedirectToAction($"Details", new { id = feedback.ModelId });
        }

        public IActionResult Reviews(int id)
        {
            var ratings = this.vacationsService.GetReviews<VacationRatingsReviewViewModel>(id);

            var ratingInfo = new VacationRatingsViewModel
            {
                HotelName = this.vacationsService.GetById<SingleVacationViewModel>(id)
                    .Name,
                HotelRatingsReviews = ratings.ToList(),
                Rating = ratings.ToList()
                    .Average(x => x.Rating),
                RatingsCount = ratings.Count(),
            };

            return this.View(ratingInfo);
        }
    }
}
