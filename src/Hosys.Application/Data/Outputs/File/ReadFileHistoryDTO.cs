namespace Hosys.Application.Data.Outputs.File;

public class ReadFileHistoryDTO
{
    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public string? FileExtension { get; set; }
    public DateTime CreatedAt { get; set; }
}
