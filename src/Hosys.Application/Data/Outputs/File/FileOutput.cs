namespace Hosys.Application.Data.Outputs.File
{
    public class FileOutput
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string Extension { get; set; }
        public required FileStream FileStream { get; set; }
        public required string ContentType { get; set; }
    }
}