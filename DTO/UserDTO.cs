using WebAppSeminar4.Models;

namespace WebAppSeminar4.DTO
{
    public class UserDTO
    { 
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRoleDTO Role { get; set; }
    }
}
