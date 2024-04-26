using System.ComponentModel.DataAnnotations;

namespace SavorySeasons.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username or Email must be between 3 and 100 characters")]
        [DataType(DataType.Text)] // This is optional, specifies the type of data
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [DataType(DataType.Password)] // This is optional, specifies the type of data
        public string Password { get; set; }
    }
}
