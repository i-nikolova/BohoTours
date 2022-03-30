namespace BohoTours.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Models;
    using BohoTours.Services.Data;
    using BohoTours.Services.Data.Hotels;
    using BohoTours.Services.Data.Transports;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Continenst;
    using BohoTours.Web.ViewModels.Countries;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Towns;
    using BohoTours.Web.ViewModels.Transports;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Routing;

    public class VacationsController : Controller
    {
        private readonly ICountriesService countriesService;
        private readonly IContinentsService continentsService;
        private readonly ITownsService townsService;
        private readonly ITransportsService transportsService;
        private readonly IVacationsService vacationsService;

        public VacationsController(IContinentsService continentsService, ICountriesService countriesService, ITownsService townsService, IVacationsService vacationsService, ITransportsService transportsService)
        {
            this.continentsService = continentsService;
            this.countriesService = countriesService;
            this.townsService = townsService;
            this.vacationsService = vacationsService;
            this.transportsService = transportsService;
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

            if (country != 0 || !string.IsNullOrWhiteSpace(searchTerm) || towns.Count() != 0)
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

                if (towns.Count() != 0)
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

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateVacationViewModel()
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
                Transports = this.transportsService.GetAll<TransportViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.TransportType,
                    Value = x.Id.ToString(),
                }),
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVacationViewModel vacation)
        {
            if (!vacation.VacationPrices.Any(x => !x.IsDeleted))
            {
                this.ModelState.AddModelError(nameof(CreateVacationViewModel.VacationPrices), "Vacation must have at least one price");
            }

            var errors = this.ModelState.ErrorCount;

            if (!this.ModelState.IsValid || errors != 0)
            {
                vacation.Transports = this.transportsService.GetAll<TransportViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.TransportType,
                    Value = x.Id.ToString(),
                });
                vacation.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                vacation.Continents = this.continentsService.GetAll<ContinentViewModel>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });

                if (vacation.CountryId != 0)
                {
                    vacation.CountryName = vacation.Countries.FirstOrDefault(x => x.Value == vacation.CountryId.ToString()).Text;
                    vacation.Towns = this.townsService.GetAll<TownViewModel>().Where(t => t.CountryId == vacation.CountryId).Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });
                }

                vacation.Countries = vacation.Countries.Where(x => x.Value != vacation.CountryId.ToString());
                return this.View(vacation);
            }

            var vacationId = await this.vacationsService.Create(vacation);

            return this.Redirect($"/Vacations/Details/{vacationId}");
        }

        [Authorize]
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

        public IActionResult Edit(int id)
        {
            var vacation = this.vacationsService.GetById<EditVacationViewModel>(id);

            vacation.Continents = this.continentsService.GetAll<ContinentViewModel>().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            vacation.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            vacation.VacationPrices = vacation.VacationPrices.Where(x => !x.IsDeleted).Select(x => new VacationPriceViewModel()
            {
                Id = x.Id,
                Date = x.Date,
                Price = x.Price,
            }).ToList();

            vacation.Transports = this.transportsService.GetAll<TransportViewModel>().Select(x => new SelectListItem()
            {
                Text = x.TransportType,
                Value = x.Id.ToString(),
            }).ToList();

            vacation.Towns = this.townsService.GetAll<TownViewModel>().Where(x => x.CountryId == vacation.CountryId).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();

            vacation.ImportedImages = vacation.ImportedImages.Where(x => !x.IsImageDeleted).ToList();

            return this.View(vacation);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditVacationViewModel vacation)
        {
            if (!vacation.VacationPrices.Any(x => !x.IsDeleted))
            {
                this.ModelState.AddModelError(nameof(EditVacationViewModel.VacationPrices), "Vacation must have at least one price");
            }

            if (!vacation.ImportedImages.Any(x => !x.IsImageDeleted))
            {
                this.ModelState.AddModelError(nameof(EditVacationViewModel.ImportedImages), "Hotel must have at least one image");
            }

            var errors = this.ModelState.ErrorCount;

            if (!this.ModelState.IsValid || errors != 0)
            {
                vacation.Countries = this.countriesService.GetAll<CountryViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });
                vacation.Continents = this.continentsService.GetAll<ContinentViewModel>()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });
                vacation.Transports = this.transportsService.GetAll<TransportViewModel>().Select(x => new SelectListItem()
                {
                    Text = x.TransportType,
                    Value = x.Id.ToString(),
                }).ToList();

                vacation.Towns = this.townsService.GetAll<TownViewModel>().Where(x => x.CountryId == vacation.CountryId).Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                if (vacation.CountryId != 0)
                {
                    vacation.CountryName = vacation.Countries.FirstOrDefault(x => x.Value == vacation.CountryId.ToString()).Text;
                }

                vacation.Countries = vacation.Countries.Where(x => x.Value != vacation.CountryId.ToString());
                return this.View(vacation);
            }

            await this.vacationsService.Edit(vacation);

            return this.Redirect($"/Vacations/Details/{vacation.Id}");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await this.vacationsService.Delete(id);

            return this.Redirect($"/Vacations/All");
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
            return this.View(this.vacationsService.GetReviews<VacationRatingsViewModel>(id));
        }
    }
}
