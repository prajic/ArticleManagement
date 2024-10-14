using ArticlesManagement.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlesManagement.Application.Services
{
    public class ImageService: IImageService
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            using (var newMemoryStream = new MemoryStream())
            {
                imageFile.CopyTo(newMemoryStream);

                //// Just an example. Commented out part is how to upload an image to AWS S3:

                //var uploadRequest = new TransferUtilityUploadRequest
                //{
                //    InputStream = newMemoryStream,
                //    Key = fileName,
                //    BucketName = _bucketName,
                //    ContentType = imageFile.ContentType
                //};
                //var transferUtility = new TransferUtility(_s3Client);
                //await transferUtility.UploadAsync(uploadRequest);

                //return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";

            }

            return fileName;
        }
    }
}
