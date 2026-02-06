using System.ComponentModel.DataAnnotations;
using Vidly.Models;
using Vidly.Dtos; // Add this if MembershipTypeDto is in Vidly.Dtos namespace

namespace Vidly.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set;}
        public string? Name {get; set;}
        public bool IsSubscribedToNewsletter { get; set;}
        public byte MembershipTypeId { get; set; }

        public MembershipTypeDto? MembershipType {get; set;}

        [Min18YearsIfAMember]
        public DateTime? Birthdate {get; set;}
    }
}