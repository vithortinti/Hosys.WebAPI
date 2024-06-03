namespace Hosys.Domain.Models.Files
{
    public class FileHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? ContentType { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}