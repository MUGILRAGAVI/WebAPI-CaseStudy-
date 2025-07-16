namespace ServiceLayer.DTO
{
    public class UserDTO
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "JobSeeker"; // Default role

        // Do not include navigation properties like JobListings, Resume, Applications
        // These should be handled separately in their own DTOs or via additional services
    }
}
