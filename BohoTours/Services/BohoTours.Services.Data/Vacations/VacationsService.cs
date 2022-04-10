namespace BohoTours.Services.Data.Vacations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BohoTours.Data.Common.Repositories;
    using BohoTours.Data.Models;
    using BohoTours.Services.Data.CloudinaryHelper;
    using BohoTours.Services.Mapping;
    using BohoTours.Web.ViewModels.Feedbacks;
    using BohoTours.Web.ViewModels.Vacations;
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;

    public class VacationsService : IVacationsService
    {
        private readonly IDeletableEntityRepository<Vacation> vacationsRepostory;
        private readonly IDeletableEntityRepository<VacationPrice> vacationPricesRepository;
        private readonly IDeletableEntityRepository<VacationImages> vacationImagesRepository;
        private readonly IDeletableEntityRepository<VacationRatings> vacationRatingsRepository;
        private readonly IDeletableEntityRepository<Town> townsRepository;

        private readonly Cloudinary cloudinary;

        public VacationsService(IDeletableEntityRepository<Vacation> vacationsRepostory, Cloudinary cloudinary, IDeletableEntityRepository<VacationPrice> vacationPricesRepository, IDeletableEntityRepository<VacationRatings> vacationRatingsRepository, IDeletableEntityRepository<VacationImages> vacationImagesRepository = null, IDeletableEntityRepository<Town> townsRepository = null)
        {
            this.vacationsRepostory = vacationsRepostory;
            this.cloudinary = cloudinary;
            this.vacationPricesRepository = vacationPricesRepository;
            this.vacationRatingsRepository = vacationRatingsRepository;
            this.vacationImagesRepository = vacationImagesRepository;
            this.townsRepository = townsRepository;
        }

        public async Task<int> Create(CreateVacationViewModel vacationModel)
        {
            var towns = new List<Town>();

            foreach (var townId in vacationModel.TownsVisited)
            {
                var town = this.townsRepository.All().FirstOrDefault(x => x.Id == townId);

                towns.Add(town);
            }

            var imageUrls = await CloudinaryExtension.UploadAsync(this.cloudinary, vacationModel.Images);

            var vacation = new Vacation()
            {
                Name = vacationModel.Name,
                Description = vacationModel.Description,
                ImagePath = imageUrls.First(),
                CountryId = vacationModel.CountryId,
                Duration = vacationModel.Duration,
                TransportId = vacationModel.TransportId,
                Summary = vacationModel.Summary,
                VacationPrices = vacationModel.VacationPrices.Select(x => new VacationPrice
                {
                    Price = x.Price,
                    Date = x.Date,
                }).ToList(),
                VacationImages = imageUrls.Select(x => new VacationImages()
                {
                    ImageUrl = x,
                }).ToList(),
                CreatedOn = DateTime.UtcNow,
                TownsVisited = towns,
            };

            await this.vacationsRepostory.AddAsync(vacation);
            await this.vacationsRepostory.SaveChangesAsync();

            return vacation.Id;
        }

        public async Task Delete(int vacationId)
        {
            var hotel = this.vacationsRepostory.All().FirstOrDefault(x => x.Id == vacationId);

            this.vacationsRepostory.Delete(hotel);

            foreach (var image in hotel.VacationImages)
            {
                this.vacationImagesRepository.Delete(image);
            }

            foreach (var room in hotel.VacationPrices)
            {
                this.vacationPricesRepository.Delete(room);
            }

            await this.vacationsRepostory.SaveChangesAsync();
        }

        public async Task Edit(EditVacationViewModel vacationModel)
        {
            var vacation = this.vacationsRepostory.All().FirstOrDefault(x => x.Id == vacationModel.Id);

            foreach (var price in vacationModel.VacationPrices)
            {
                var existingPrice = this.vacationPricesRepository.All().FirstOrDefault(x => x.VacationId == vacationModel.Id && price.Id == x.Id);

                if (existingPrice == null)
                {
                    vacation.VacationPrices.Add(new VacationPrice
                    {
                        Date = price.Date,
                        Price = price.Price,
                    });
                }
                else
                {
                    if (price.IsDeleted)
                    {
                        this.vacationPricesRepository.Delete(existingPrice);
                    }
                    else
                    {
                        existingPrice.Date = price.Date;
                        existingPrice.Price = price.Price;
                    }
                }
            }

            vacation.Description = vacationModel.Description;
            vacation.Duration = vacationModel.Duration;
            vacation.TransportId = vacationModel.TransportId;
            vacation.Summary = vacationModel.Summary;

            var currTowns = this.townsRepository.All().Include(x => x.Vacations).Where(x => x.Vacations.Any(x => x.Id == vacationModel.Id));

            foreach (var town in vacationModel.TownsVisited)
            {
                if (!currTowns.Select(x => x.Id).Contains(town))
                {
                    this.townsRepository.All().FirstOrDefault(x => x.Id == town).Vacations.Add(vacation);
                }
            }

            foreach (var town in currTowns)
            {
                if (!vacationModel.TownsVisited.ToList().Contains(town.Id))
                {
                    town.Vacations.Remove(vacation);
                }
            }

            vacation.CountryId = vacationModel.CountryId;
            vacation.Name = vacationModel.Name;

            var vacationImages = this.vacationImagesRepository.All().Where(x => x.VacationId == vacationModel.Id);

            foreach (var image in vacationModel.ImportedImages)
            {
                var existingImage = this.vacationImagesRepository.All().FirstOrDefault(x => x.VacationId == vacationModel.Id && image.Id == x.Id);

                if (image.IsImageDeleted)
                {
                    this.vacationImagesRepository.Delete(existingImage);
                }
            }

            if (vacationModel.Images != null)
            {
                var imageUrls = await CloudinaryExtension.UploadAsync(this.cloudinary, vacationModel.Images);
                foreach (var item in imageUrls.Select(x => new VacationImages() { ImageUrl = x }))
                {
                    vacation.VacationImages.Add(item);
                }
            }

            if (vacationModel.ImportedImages.Where(x => !x.IsImageDeleted).ToList().Count == 1)
            {
                vacation.ImagePath = vacationModel.ImportedImages.FirstOrDefault(x => !x.IsImageDeleted).ImageUrl;
            }
            else
            {
                vacation.ImagePath = vacationModel.ImagePath;
            }

            this.vacationsRepostory.Update(vacation);
            await this.vacationsRepostory.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.vacationsRepostory.AllAsNoTracking().Where(x => !x.IsDeleted).OrderByDescending(x => x.Id).To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.vacationsRepostory.AllAsNoTracking().Where(x => x.Id == id && !x.IsDeleted).To<T>().FirstOrDefault();
        }

        public int GetCount()
        {
            return this.vacationsRepostory.AllAsNoTracking().Where(x => !x.IsDeleted).Count();
        }

        public IEnumerable<T> GetRecommended<T>()
        {
            var random = new Random();
            var list = this.GetAll<T>().ToArray();
            var recommendedVacations = new List<T>();

            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(list.Count());
                recommendedVacations.Add(list[index]);
            }

            return recommendedVacations;
        }

        public async Task AddFeedback(FeedbackViewModel feedback)
        {
            var vacation = this.vacationsRepostory.All().FirstOrDefault(x => x.Id == feedback.ModelId);
            vacation.VacationsRatings.Add(new VacationRatings()
            {
                Name = feedback.Name,
                Email = feedback.Email,
                Message = feedback.Message,
                Rating = feedback.Rating,
            });

            this.vacationsRepostory.Update(vacation);
            await this.vacationsRepostory.SaveChangesAsync();
        }

        public IEnumerable<T> GetReviews<T>(int hotelId)
        {
            return this.vacationRatingsRepository.AllAsNoTracking().Where(x => x.VacationId == hotelId && !x.IsDeleted).To<T>().ToList();
        }

        public IEnumerable<T> GetRecommendedByContinent<T>(string continetnCode)
        {
            var random = new Random();
            var list = this.vacationsRepostory.AllAsNoTracking().Where(x => x.Country.Continent.ContinentCode == continetnCode).To<T>().ToArray();
            var recommendedVacations = new List<T>();

            if (list.Any())
            {
                for (int i = 0; i < 2; i++)
                {
                    int index = random.Next(list.Count());
                    recommendedVacations.Add(list[index]);
                }
            }

            return recommendedVacations;
        }
    }
}
