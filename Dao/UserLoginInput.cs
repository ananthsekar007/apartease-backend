using System.ComponentModel.DataAnnotations;

namespace apartease_backend.Dao
{
    public class UserLoginInput
    {
        [EmailAddress]
        public string Email { get; set;} = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
