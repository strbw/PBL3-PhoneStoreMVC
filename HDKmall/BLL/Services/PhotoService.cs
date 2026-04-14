using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HDKmall.BLL.Interfaces;
using HDKmall.Helpers; // Dùng để gọi class CloudinarySettings bạn đã tạo ở Bước 4
using Microsoft.Extensions.Options;

namespace HDKmall.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            // Thiết lập tài khoản
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        // 1. Hàm Thêm/Upload Ảnh
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    // Đoạn này cực hay: Tự động ép kích thước ảnh về 800x800px 
                    // Giúp giao diện web PhoneStore của bạn không bị vỡ khung
                    Transformation = new Transformation().Height(800).Width(800).Crop("fill")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        // 2. Hàm Xóa Ảnh (Mình viết thêm cho bạn để không bị lỗi)
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}