namespace BohoTours.Web.Areas.Administration.Controllers
{
    using BohoTours.Services.Data;
    using BohoTours.Services.Data.Transports;
    using BohoTours.Services.Data.Vacations;
    using BohoTours.Web.ViewModels.Continenst;
    using BohoTours.Web.ViewModels.Countries;
    using BohoTours.Web.ViewModels.Towns;
    using BohoTours.Web.ViewModels.Transports;
    using BohoTours.Web.ViewModels.Vacations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;
    using System.Threading.Tasks;

    public class VacationsController : AdministrationController
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

        public async Task<IActionResult> Delete(int id)
        {
            await this.vacationsService.Delete(id);

            return this.Redirect($"/Vacations/All");
        }
    }
}
