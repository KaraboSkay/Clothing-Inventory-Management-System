using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Assignment3_Backend.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
