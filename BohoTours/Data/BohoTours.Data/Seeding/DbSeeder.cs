namespace BohoTours.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class DbSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Continents.Any())
            {
                string continents = File.ReadAllText(@"./SeedDbJson/" + nameof(continents) + ".json");
                var continentsJson = JsonConvert.DeserializeObject<IEnumerable<Continent>>(continents);

                await dbContext.Continents.AddRangeAsync(continentsJson);

                string countries = File.ReadAllText(@"./SeedDbJson/" + nameof(countries) + ".json");
                var countriesJson = JsonConvert.DeserializeObject<IEnumerable<Country>>(countries);

                string towns = File.ReadAllText(@"./SeedDbJson/" + nameof(towns) + ".json");
                var townsJson = JsonConvert.DeserializeObject<IEnumerable<Town>>(towns);

                foreach (var country in countriesJson)
                {
                    foreach (var town in townsJson.Where(x => x.CountryId == country.Id))
                    {
                        country.Towns.Add(town);
                    }
                }

                await dbContext.Countries.AddRangeAsync(countriesJson);
                await dbContext.Towns.AddRangeAsync(townsJson);

                await dbContext.SaveChangesAsync();

                string transport = File.ReadAllText(@"./SeedDbJson/" + nameof(transport) + ".json");
                var transportJson = JsonConvert.DeserializeObject<IEnumerable<Transport>>(transport);
                foreach (var transport1 in transportJson)
                {
                    transport1.Id = 0;
                    await dbContext.Transports.AddAsync(transport1);
                }

                await dbContext.SaveChangesAsync();

                string hotels = File.ReadAllText(@"./SeedDbJson/" + nameof(hotels) + ".json");
                var hotelsJson = JsonConvert.DeserializeObject<IEnumerable<Hotel>>(hotels);

                string hotelsRoom = File.ReadAllText(@"./SeedDbJson/" + nameof(hotelsRoom) + ".json");
                var hotelsRoomJson = JsonConvert.DeserializeObject<IEnumerable<HotelRoom>>(hotelsRoom);

                string hotelsRoomPrices = File.ReadAllText(@"./SeedDbJson/" + nameof(hotelsRoomPrices) + ".json");
                var hotelsRoomPricesJson = JsonConvert.DeserializeObject<IEnumerable<HotelRoomPrice>>(hotelsRoomPrices);

                string hotelsImages = File.ReadAllText(@"./SeedDbJson/" + nameof(hotelsImages) + ".json");
                var hotelsImagesJson = JsonConvert.DeserializeObject<IEnumerable<HotelImages>>(hotelsImages);

                string hotelsRatings = File.ReadAllText(@"./SeedDbJson/" + nameof(hotelsRatings) + ".json");
                var hotelsRatingsJson = JsonConvert.DeserializeObject<IEnumerable<HotelRatings>>(hotelsRatings);

                string hotelsBookings = File.ReadAllText(@"./SeedDbJson/" + nameof(hotelsBookings) + ".json");
                var hotelsBookingsJson = JsonConvert.DeserializeObject<IEnumerable<HotelBooking>>(hotelsBookings);

                foreach (var room in hotelsRoomJson)
                {
                    foreach (var roomPrice in hotelsRoomPricesJson.Where(x => x.RoomId == room.Id))
                    {
                        foreach (var booking in hotelsBookingsJson.Where(x => x.HotelRoomPriceId == roomPrice.Id))
                        {
                            booking.Id = 0;
                            roomPrice.Bookings.Add(booking);
                        }

                        roomPrice.Id = 0;
                        room.HotelRoomPrices.Add(roomPrice);
                    }
                }

                foreach (var hotel in hotelsJson)
                {
                    foreach (var room in hotelsRoomJson.Where(x => x.HotelId == hotel.Id))
                    {
                        room.Id = 0;
                        hotel.HotelRooms.Add(room);
                    }

                    foreach (var image in hotelsImagesJson.Where(x => x.HotelId == hotel.Id))
                    {
                        image.Id = 0;
                        hotel.HotelImages.Add(image);
                    }

                    foreach (var rating in hotelsRatingsJson.Where(x => x.HotelId == hotel.Id))
                    {
                        rating.Id = 0;
                        hotel.HotelRatings.Add(rating);
                    }

                    hotel.Id = 0;
                    dbContext.Hotels.Add(hotel);
                }

                await dbContext.SaveChangesAsync();

                string vacations = File.ReadAllText(@"./SeedDbJson/" + nameof(vacations) + ".json");
                var vacationsJson = JsonConvert.DeserializeObject<IEnumerable<Vacation>>(vacations);

                string vacationPrices = File.ReadAllText(@"./SeedDbJson/" + nameof(vacationPrices) + ".json");
                var vacationPricesJson = JsonConvert.DeserializeObject<IEnumerable<VacationPrice>>(vacationPrices);

                string vacationsImages = File.ReadAllText(@"./SeedDbJson/" + nameof(vacationsImages) + ".json");
                var vacationsImagesJson = JsonConvert.DeserializeObject<IEnumerable<VacationImages>>(vacationsImages);

                string vacationsRatings = File.ReadAllText(@"./SeedDbJson/" + nameof(vacationsRatings) + ".json");
                var vacationsRatingsJson = JsonConvert.DeserializeObject<IEnumerable<VacationRatings>>(vacationsRatings);

                string vacationsBookings = File.ReadAllText(@"./SeedDbJson/" + nameof(vacationsBookings) + ".json");
                var vacationsBookingsJson = JsonConvert.DeserializeObject<IEnumerable<VacationBooking>>(vacationsBookings);

                foreach (var vacation in vacationsJson)
                {
                    vacation.Transport = transportJson.FirstOrDefault();

                    vacation.TownsVisited.Add(countriesJson.FirstOrDefault(x => x.Id == vacation.CountryId).Towns.FirstOrDefault());

                    foreach (var vacationPrice in vacationPricesJson.Where(x => x.VacationId == vacation.Id))
                    {
                        foreach (var vacationBooking in vacationsBookingsJson.Where(x => x.VacationPriceId == vacationPrice.Id))
                        {
                            vacationBooking.Id = 0;
                            vacationPrice.Bookings.Add(vacationBooking);
                        }

                        vacationPrice.Id = 0;
                        vacation.VacationPrices.Add(vacationPrice);
                    }

                    foreach (var image in vacationsImagesJson.Where(x => x.VacationId == vacation.Id))
                    {
                        image.Id = 0;
                        vacation.VacationImages.Add(image);
                    }

                    foreach (var rating in vacationsRatingsJson.Where(x => x.VacationId == vacation.Id))
                    {
                        rating.Id = 0;
                        vacation.VacationsRatings.Add(rating);
                    }

                    vacation.Id = 0;
                    dbContext.Vacations.Add(vacation);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
