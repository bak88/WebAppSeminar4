using WebAppSeminar4.DTO;
using WebAppSeminar4.Models;

namespace WebAppSeminar4.Abstraction
{
    public interface IUserRepository
    {
        int AddUser(UserDTO userDTO);
        RoleId CheckUser(LoginDTO loginDTO);
        UserDTO Authenticate(LoginDTO loginDTO);
    }
}
