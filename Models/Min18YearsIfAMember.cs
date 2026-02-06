using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Min18YearsIfAMember: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // var customer = (Customer)validationContext.ObjectInstance;
            var instance = validationContext.ObjectInstance;
            var membershipTypeIdProperty = validationContext.ObjectType.GetProperty("MembershipTypeId");
            var birthdayProperty = validationContext.ObjectType.GetProperty("Birthdate");
             if (membershipTypeIdProperty == null || birthdayProperty == null)
            {
                return new ValidationResult("MembershipTypeId or Birthdate property not found.");
            }

            var membershipTypeId = (byte)membershipTypeIdProperty.GetValue(instance);
            var birthdate = (DateTime?)birthdayProperty.GetValue(instance);

            if (membershipTypeId == MembershipType.Unknown || membershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            if (birthdate == null)
                return new ValidationResult("Birthdate is required.");

            // if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
            //     return ValidationResult.Success;

            // if (customer.Birthdate == null)
            //     return new ValidationResult("Birthdate is required.");

            var age = DateTime.Today.Year - birthdate.Value.Year;

            return (age >= 18)
                ? ValidationResult.Success
                : new ValidationResult("Customer should be at least 18 years old to go on a membership.");

        }
    }
}