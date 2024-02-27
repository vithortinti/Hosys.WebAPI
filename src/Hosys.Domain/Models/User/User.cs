using System.Diagnostics.CodeAnalysis;

namespace Hosys.Domain.Models.User
{
    /// <summary>
    /// The class User is responsible for representing a user in the domain model of the application. 
    /// </summary>
    public class User
    {
        public required Guid Id { get; set; }
        public required int PublicId { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string NickName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}