namespace BohoTours.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Services.Data;
    using BohoTours.Web.ViewModels.Towns;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("api/[controller]")]
    [ApiController]
    public class TownsController : ControllerBase
    {
        private readonly ITownsService townsService;

        public TownsController(ITownsService townsService)
        {
            this.townsService = townsService;
        }

        [HttpGet]
        public IEnumerable<TownViewModel> GetAll(int countryId)
        {
            return this.townsService.GetAll<TownViewModel>().Where(x => x.CountryId == countryId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTownViewModel town, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (this.townsService.GetAll<TownViewModel>().Any(x => x.Name.ToLower() == town.TownName.ToLower()))
            {
                this.ModelState.AddModelError(nameof(CreateTownViewModel.TownName), "This town already exists!");

                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(this.ControllerContext);
            }

            if (town.AddedCountryId == 0)
            {
                this.ModelState.AddModelError(nameof(CreateTownViewModel.AddedCountryId), "Please select country!");

                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(this.ControllerContext);
            }

            var (id, name) = await this.townsService.Create(town.AddedCountryId, town.TownName);

            return this.Ok((id, name));
        }
    }
}
