using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Validation
{
    /// <summary>
    /// Кастомний атрибут валідації для перевірки розміру файлу
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    var maxSizeMB = _maxFileSize / 1024 / 1024;
                    return new ValidationResult($"Максимальний розмір файлу: {maxSizeMB} MB");
                }
            }

            return ValidationResult.Success;
        }
    }
}
