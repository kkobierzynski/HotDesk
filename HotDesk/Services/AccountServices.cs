using HotDesk.Models;

namespace HotDesk.Services
{
    public interface IAccountServices
    {
        string GenerateJwt(LoginUserDto dto);
    }

    public class AccountServices : IAccountServices
    {        
        public AccountServices()
        {

        }

        public string GenerateJwt(LoginUserDto dto)
        {
            return "init";
        }
    }
}
