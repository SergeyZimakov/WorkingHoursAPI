using System.ComponentModel.DataAnnotations;

namespace Core.View.Auth
{
    public class TokenView
    {
        [Required]
        public string Token { get; set; }
    }
}
