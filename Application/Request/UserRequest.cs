using System.ComponentModel.DataAnnotations;

namespace Domino.Application.Request
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
