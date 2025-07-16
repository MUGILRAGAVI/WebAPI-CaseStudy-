namespace ServiceLayer.DTO
{
    public class ApplicationDTO
    {
        public int JobId { get; set; }

        public int JobSeekerId { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public string Notes { get; set; } = string.Empty;
    }
}
