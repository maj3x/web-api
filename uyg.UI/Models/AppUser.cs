using Microsoft.AspNetCore.Identity;

namespace uyg.UI.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<News> News { get; set; } = new List<News>();
    }
} 