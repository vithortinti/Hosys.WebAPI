using System.ComponentModel.DataAnnotations;

namespace Hosys.Application.Data.Outputs.File;

public class UpdateFileHistoryDTO
{
    public required string? FileName { get; set; }
}
