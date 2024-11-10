using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.DeviceBorrowingRequest
{
    public class RequestBorrowingDeviceDto
    {
        [Required]
        public List<int> DeviceIds { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [BorrowDateValidation]
        public DateTime BorrowDate { get; set; }

        [ReturnDateValidation("BorrowDate")]
        public DateTime? ReturnDate { get; set; }

        public class BorrowDateValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is DateTime borrowDate && borrowDate > DateTime.Now)
                    return ValidationResult.Success;
                
                return new ValidationResult("Borrow date must be in the future.");
            }
        }

        public class ReturnDateValidationAttribute : ValidationAttribute
        {
            private readonly string _borrowDatePropertyName;

            public ReturnDateValidationAttribute(string borrowDatePropertyName)
            {
                _borrowDatePropertyName = borrowDatePropertyName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var borrowDateProperty = validationContext.ObjectType.GetProperty(_borrowDatePropertyName);
                if (borrowDateProperty == null)
                    return new ValidationResult($"Unknown property: {_borrowDatePropertyName}");

                var borrowDateValue = borrowDateProperty.GetValue(validationContext.ObjectInstance, null) as DateTime?;
                if (value is DateTime returnDate && borrowDateValue.HasValue && returnDate > borrowDateValue.Value)
                    return ValidationResult.Success;
                
                return new ValidationResult("Return date must be after the borrow date.");
            }
        }
    }
}
