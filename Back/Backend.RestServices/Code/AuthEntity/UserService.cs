using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.RestServices.Code.AuthEntity
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User {  Username = "user", Password = "user" }
        };

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

         
            if (user == null)
                return null;

          
            user.Password = null;
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _users.Select(x => {
                x.Password = null;
                return x;
            }));
        }
    }
}
