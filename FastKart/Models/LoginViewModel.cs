using System.ComponentModel.DataAnnotations;

namespace FastKart.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
