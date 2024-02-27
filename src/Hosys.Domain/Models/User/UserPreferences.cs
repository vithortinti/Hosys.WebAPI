namespace Hosys.Domain.Models.User
{
    /// <summary>
    /// The class UserPreferences is responsible for representing the user preferences in the domain model of the application.
    /// </summary>
    public class UserPreferences
    {
        public required Guid UserId { get; set; }
        public bool DarkMode { get; set; }
    }
}