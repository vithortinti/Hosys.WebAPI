namespace Hosys.Domain.Models.User
{
    /// <summary>
    /// The class UserRecovery is responsible for representing the user recovery in the domain model of the application.
    /// It is used to recover the account of a user based on the recovery key or the change password code.
    /// </summary>
    public class UserRecovery
    {
        public required Guid UserId { get; set; }
        public required string RecoveryKey { get; set; }
        public string? ChangePasswordCode { get; set; }
        public DateTime? ChangePasswordCodeExpiration { get; set; }
    }
}