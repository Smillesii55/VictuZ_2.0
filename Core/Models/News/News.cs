using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Newss
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; }

        [StringLength(200, ErrorMessage = "Image URL cannot exceed 200 characters.")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
    }
}
