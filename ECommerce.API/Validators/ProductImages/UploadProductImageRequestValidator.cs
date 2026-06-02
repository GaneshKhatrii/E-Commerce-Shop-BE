using ECommerce.API.DTOs.ProductImages;
using FluentValidation;

namespace ECommerce.API.Validators.ProductImages
{
    public class UploadProductImageRequestValidator : AbstractValidator<UploadProductImageRequest>
    {
        private readonly string[] AllowedExtensions = {
            ".jpg", ".jpeg", ".png", "Webg"
        };

        private bool HaveValidExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return AllowedExtensions.Contains(extension);
        }

        // 2MB, 1 KB = 1024 bytes, 1 MB = 1024 * 1024 bytes
        private const long MaxFileSize = 2 * 1024 * 1024;

        public UploadProductImageRequestValidator()
        {
            RuleFor(x => x.ProductVariantId)
               .NotEmpty()
               .WithMessage("Product variant is required");

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image is required");

            RuleFor(x => x.Image.Length)
                .LessThanOrEqualTo(MaxFileSize)
                .WithMessage("Image size must not exceed 2MB");

            RuleFor(x => x.Image.FileName)
                .Must(HaveValidExtension)
                .WithMessage("Only jpg, jpeg, png, and webp files are allowed");

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0)
                .WithMessage("Display order must be greater than 0");
        }
    }
}
