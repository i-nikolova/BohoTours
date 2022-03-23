namespace BohoTours.Services.Data.CloudinaryHelper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryExtension
    {
        public static async Task<List<string>> UploadAsync(Cloudinary cloudinary, ICollection<IFormFile> images)
        {
            List<string> list = new List<string>();

            foreach (var image in images)
            {
                byte[] destinationImage;

                await using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();

                await using var destinationStream = new MemoryStream(destinationImage);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(image.FileName, destinationStream),
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                list.Add(uploadResult.Uri.AbsoluteUri);
            }

            return list;
        }
    }
}
