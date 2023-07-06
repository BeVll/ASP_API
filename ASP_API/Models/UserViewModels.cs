using ASP_API.Data.Entities.Identity;

namespace ASP_API.Models
{
    public class UserViewModels
    {
        public class UserItemViewModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Image { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class CreateUserViewModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public IFormFile Image { get; set; }
            public string Role { get; set; }
        }
        public class EditUserViewModel
        {

            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public IFormFile Image { get; set; }
            public string Role { get; set; }
        }
    }
}
