using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.Emails
{
    public class CreateEmailTemplateViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string From { get; set; }

        [MaxLength(988)]
        public string? Subject { get; set; }

        [MaxLength(5000)]
        public string Body { get; set; }
    }
}
