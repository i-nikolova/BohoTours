namespace BohoTours.Data.Scraping
{
    using AngleSharp.Html.Parser;
    using BohoTours.Data.Models;
    using BohoTours.Data.Seeding;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ScrapingSeeder : ISeeder
    {
        private static readonly List<string> LinksToScrape = new()
        {
            {
                "https://www.tours-club.com/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0-%D0%B2-%D0%B1%D0%B5%D0%BB%D0%B5%D0%BA-2022-%D0%B0%D0%B2%D1%82%D0%BE%D0%B1%D1%83%D1%81%D0%BD%D0%B0-%D0%BF%D1%80%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%B0-%D1%81%D1%8A%D1%81-7-%D0%BD%D0%BE%D1%89%D1%83%D0%B2%D0%BA%D0%B8-%D0%BE%D1%82-%D1%81%D0%BE%D1%84%D0%B8%D1%8F-%D0%B8-%D0%BF%D0%BB%D0%BE%D0%B2%D0%B4%D0%B8%D0%B2/98"
            },
            {
                "https://www.tours-club.com/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0-%D0%B2-%D1%81%D0%B8%D0%B4%D0%B5-%D1%81-%D0%B0%D0%B2%D1%82%D0%BE%D0%B1%D1%83%D1%81-7-%D0%BD%D0%BE%D1%89%D1%83%D0%B2%D0%BA%D0%B8/88"
            },
            {
                "https://www.tours-club.com/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0-%D0%B2-%D0%BB%D0%B0%D1%80%D0%B0-%D1%81-%D0%B0%D0%B2%D1%82%D0%BE%D0%B1%D1%83%D1%81-7-%D0%BD%D0%BE%D1%89%D1%83%D0%B2%D0%BA%D0%B8/108"
            },
            {
                "https://www.tours-club.com/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0/%D0%BF%D0%BE%D1%87%D0%B8%D0%B2%D0%BA%D0%B0-%D0%B2-%D0%B0%D0%BB%D0%B0%D0%BD%D0%B8%D1%8F-%D1%81-%D0%B0%D0%B2%D1%82%D0%BE%D0%B1%D1%83%D1%81-7-%D0%BD%D0%BE%D1%89%D1%83%D0%B2%D0%BA%D0%B8-%D0%B2%D0%B0%D1%80%D0%BD%D0%B0/280"
            },
        };

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var parser = serviceProvider.GetRequiredService<HtmlParser>();
            var client = serviceProvider.GetRequiredService<HttpClient>();

            if (dbContext.Hotels.Any())
            {
                return;
            }

            await this.GatherBasicData(dbContext, parser, client);
        }

        private static async Task<string> GetHtml(string pageUrl, HttpClient client)
        {
            var pageResponse = await client.GetAsync(pageUrl);
            var byteContent = await pageResponse.Content.ReadAsByteArrayAsync();
            var html = Encoding.UTF8.GetString(byteContent);
            return html;
        }

        private async Task GatherBasicData(ApplicationDbContext dbContext, HtmlParser parser, HttpClient client)
        {
            foreach (var link in LinksToScrape)
            {
                var pageHtml = await GetHtml(
                    link, client);
                var pageDocument = await parser.ParseDocumentAsync(pageHtml);

                var listItems = pageDocument.GetElementsByClassName("col-lg-6 col-md-12 col-sm-12 otstap").ToList();

                foreach (var listItem in listItems)
                {
                    var hotel = new Hotel();

                    var listHref = listItem.Children[0];

                    hotel.Name = listHref.QuerySelector("div.hotels-list-title").TextContent;
                    var hotelDestination =
                        listHref.QuerySelector("div.hotels-list-destination").TextContent.Split(", ");

                    var hotelCity = dbContext.Towns.FirstOrDefault(t => t.Name == hotelDestination[0]);
                    var hotelCountry = dbContext.Countries.FirstOrDefault(c => c.Name == hotelDestination[1]);

                    hotel.ImagePath = "https://www.tours-club.com/" + listHref.Children[0].Attributes["data-original"].Value;

                    if (hotelCity != null)
                    {
                        hotel.Town = hotelCity;
                    }
                    else if (hotelCountry == null)
                    {
                        hotel.Town = new Town()
                        {
                            Name = hotelDestination[0],
                            Country = new Country()
                            {
                                Name = hotelDestination[1],
                                Continent = dbContext.Continents.FirstOrDefault(x => x.ContinentCode == "EU"),
                            },
                        };
                    }
                    else
                    {
                        hotel.Town = new Town()
                        {
                            Name = hotelDestination[0],
                            Country = hotelCountry,
                        };
                    }

                    var hotelUrl = "https://www.tours-club.com/" + listHref.Attributes["href"].Value;

                    var hotelHtml = await GetHtml(hotelUrl, client);
                    var hotelDocument = await parser.ParseDocumentAsync(hotelHtml);

                    var hotelImages = hotelDocument.GetElementsByClassName("gallery-img-item").Select(x => "https://www.tours-club.com/" + x.Children[0].Attributes["href"].Value.ToString());

                    foreach (var hotelImage in hotelImages)
                    {
                        hotel.HotelImages.Add(new HotelImages()
                        {
                            ImageUrl = hotelImage,
                        });
                    }

                    var descriptionSection = hotelDocument.GetElementsByClassName("offer-text-containt")
                        .FirstOrDefault().TextContent;

                    var description = string.Empty;

                    var startIndex = descriptionSection.IndexOf("Разположение:") + 13;
                    var endIndexOf = descriptionSection.IndexOf("В стаите:");
                    if (startIndex != -1 && endIndexOf != -1)
                    {
                        hotel.Description = descriptionSection[startIndex..endIndexOf];
                    }

                    hotel.HotelRooms = hotelDocument.GetElementsByClassName("antetka-2").Select(r => r.TextContent).Select(x => new HotelRoom
                    {
                        RoomType = x,
                        Hotel = hotel,
                        MaxCapacity = new Random().Next(2, 5),
                    }).SkipLast(3).ToArray();

                    var mapDetails = hotelDocument.GetElementsByTagName("iframe").ToList().Last();

                    string pattern = @"LAT=\d+.\d+&LON=\d+.\d+";

                    var location = Regex.Match(mapDetails.Attributes["src"].Value, pattern)?.Value;

                    List<HotelRoomPrice> roomPrices = new();

                    if (!string.IsNullOrEmpty(location))
                    {
                        var locEl = location.Split("&");
                        hotel.LAT = locEl[0].Split("=")[1];
                        hotel.LON = locEl[1].Split("=")[1];
                    }

                    for (int i = 0; i < hotel.HotelRooms.Count; i++)
                    {
                        for (DateTime date = new(2022, 6, 1); date < new DateTime(2022, 8, 31); date = date.AddDays(7))
                        {
                            Random rand = new();
                            decimal random = rand.Next(150, 200);

                            var roomPrice = new HotelRoomPrice
                            {
                                Room = hotel.HotelRooms.ToArray()[i],
                                Date = date,
                                PricePerNight = random * (i + 1),
                            };

                            dbContext.HotelRoomPrices.Add(roomPrice);
                        }
                    }

                    dbContext.Hotels.Add(hotel);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
