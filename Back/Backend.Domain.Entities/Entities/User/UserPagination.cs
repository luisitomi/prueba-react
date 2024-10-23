using System.Collections.Generic;

namespace Backend.Domain.Entities.Entities.User
{
    public class UserPagination
    {
        public int TotalElementos { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
