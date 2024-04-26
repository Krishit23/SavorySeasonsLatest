using Microsoft.AspNetCore.Identity;

namespace SavorySeasons.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
