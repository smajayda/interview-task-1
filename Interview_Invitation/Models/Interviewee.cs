using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Invitation.Models
{
    public class Interviewee
    {
        public int IntervieweeId { get; set; }
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNumber { get; set; }

        public string? CvFilePath { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile CvFile { get; set; }


        // Foreign key
        public int InterviewId { get; set; }
        public Interview Interview { get; set; }
    }
}
