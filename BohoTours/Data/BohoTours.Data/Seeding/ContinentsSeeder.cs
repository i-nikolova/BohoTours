namespace BohoTours.Data.Seeding
{
    using BohoTours.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContinentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Continents.Any())
            {
                return;
            }

            await dbContext.Continents.AddAsync(new Continent() { Name = "Europe", ContinentCode = "EU" });
            await dbContext.Continents.AddAsync(new Continent() { Name = "Asia", ContinentCode = "AS" });
            await dbContext.Continents.AddAsync(new Continent() { Name = "North america", ContinentCode = "NA" });
            await dbContext.Continents.AddAsync(new Continent() { Name = "South america", ContinentCode = "SA" });
        }
    }
}
