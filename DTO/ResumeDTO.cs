namespace ServiceLayer.DTO
{
    public class ResumeDTO
    {
        public int UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = "application/pdf";
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    }
}
