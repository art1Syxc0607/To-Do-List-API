namespace DataAccess.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }


        public List<UserTask>? Tasks { get; set; }
    }
}
