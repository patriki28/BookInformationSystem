using System.ComponentModel.DataAnnotations;

namespace BookInformationSystem.Models.Domain
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
