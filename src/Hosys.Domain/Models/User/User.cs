namespace Hosys.Domain.Models.User
{
    /// <summary>
    /// The class User is responsible for representing a user in the domain model of the application. 
    /// </summary>
    #nullable disable
    public class User
    {
        public Guid Id { get; set; }
        public int PublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserRecovery? UserRecovery { get; set; }
    }
}