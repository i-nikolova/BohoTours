namespace BohoTours.Web.ViewModels.Feedbacks
{
    using System.ComponentModel.DataAnnotations;

    public class FeedbackViewModel
    {
        public int ModelId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
