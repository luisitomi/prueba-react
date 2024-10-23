using Backend.Domain.Entities.Entities.User;
using Backend.Domain.Entities.Util;
using System.Threading.Tasks;

namespace Backend.Infraestructure.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<Return> Login(UserDTO request);
        Task<Return> Search(UserSearchDTO request);
        Task<Return> Add(UserModel request);
        Task<Return> Delete(Entity request);
    }
}
