using Backend.Domain.Entities.Util;

namespace Backend.Domain.Entities.Entities.User
{
    public class UserModel : Entity
    {
        public string User { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Password { get; set; }
    }
}
