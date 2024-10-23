using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.RestServices.Code.AuthEntity
{
    public  interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}
