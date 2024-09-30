using System.ComponentModel.DataAnnotations;

namespace FastKart.Models
{
    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]

        public string ConfirmPassword { get; set; }


    }
}
