using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Validation
{
    /// <summary>
    /// Кастомний атрибут валідації для перевірки розширення файлу
    /// </summary>
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedFileExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!_extensions.Contains(extension))
                {
                    var allowedExtensions = string.Join(", ", _extensions);
                    return new ValidationResult($"Дозволені розширення файлів: {allowedExtensions}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
