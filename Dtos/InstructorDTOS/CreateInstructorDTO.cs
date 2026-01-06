using System.ComponentModel.DataAnnotations;

namespace Dtos.InstructorDTOS
{
    public class CreateInstructorDTO
    {
        public int InstructorID { get; set; }

        public int PersonID { get; set; }

        [MaxLength(100, ErrorMessage = "Qualification cannot exceed 100 characters")]
        public string? Qualification { get; set; }
    }
}
