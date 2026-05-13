using System.ComponentModel.DataAnnotations;

namespace BookstoreAdoNetApp.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Book title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author name is required")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 100000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Publication year is required")]
        [Range(1000, 9999, ErrorMessage = "Enter a valid year")]
        public int PublicationYear { get; set; }
    }
}
