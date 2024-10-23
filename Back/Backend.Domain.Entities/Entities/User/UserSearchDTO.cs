namespace Backend.Domain.Entities.Entities.User
{
    public class UserSearchDTO
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
    }
}
