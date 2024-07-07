using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Emails
{
    public class CreateEmailTemplateViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string From { get; set; }

        [MaxLength(988, ErrorMessage = "Subject must be at most 988 characters long")]
        public string? Subject { get; set; }

        [MaxLength(5000, ErrorMessage = "Body must be at most 5000 characters long")]
        public string Body { get; set; }
    }
}
