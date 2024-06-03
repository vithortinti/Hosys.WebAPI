namespace Hosys.Application.Data.Outputs.File
{
    public class CreateFileHistoryDTO
    {
        public Guid UserId { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? ContentType { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}