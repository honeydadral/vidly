using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set;}

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255)]
        public string? Name {get; set;}

        public bool IsSubscribedToNewsletter { get; set;}
    
        public MembershipType? MembershipType{get; set;}

    [Required(ErrorMessage = "Membership Type is required")]
    [Display(Name ="Membership Type")]
    [Range(1, 255)]
    public byte MembershipTypeId { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Min18YearsIfAMember]
        [Required(ErrorMessage = "Birthdate is required")]
        public DateTime? Birthdate {get; set;}
        
    }
}