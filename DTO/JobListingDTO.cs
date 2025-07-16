namespace ServiceLayer.DTO
{
    public class JobListingDTO
    {
        public int EmployerId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Qualifications { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    }
}
