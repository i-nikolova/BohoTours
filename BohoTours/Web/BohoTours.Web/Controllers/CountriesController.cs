namespace BohoTours.Web.Controllers
{
    using BohoTours.Services.Data;
    using BohoTours.Web.ViewModels.Countries;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        [HttpGet]
        public IEnumerable<CountryViewModel> GetAll()
        {
            return this.countriesService.GetAll<CountryViewModel>();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryViewModel country, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (this.GetAll().Any(x => x.Name.ToLower() == country.CountryName.ToLower()))
            {
                this.ModelState.AddModelError(nameof(CreateCountryViewModel.CountryName), "This country already exists!");

                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(this.ControllerContext);
            }

            if (country.ContinentId == 0)
            {
                this.ModelState.AddModelError(nameof(CreateCountryViewModel.ContinentId), "Please select continent!");

                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(this.ControllerContext);
            }

            var (id, name) = await this.countriesService.Create(country.ContinentId, country.CountryName);

            return this.Ok((id, name));
        }
    }
}
