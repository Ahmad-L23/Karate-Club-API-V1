using System.ComponentModel.DataAnnotations;

namespace Dtos.PersonDTOS
{
    public class CreatePersonDTO
    {
        public int PersonID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "ContactInfo cannot exceed 100 characters")]
        public string? ContactInfo { get; set; }
    }
}
