using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MovieReservationSystem.ViewModels;

namespace MovieReservationSystem.Services
{
    public class FilesUpload
    {
        public static async Task<string> UploadImage(IWebHostEnvironment host,IFormFile ImageFile)
        {
            var path = Path.Combine(host.WebRootPath, "Images");
            var filePath = Path.Combine(path,ImageFile.FileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fs);
            }
            return ImageFile.FileName;
        }
    }
}
