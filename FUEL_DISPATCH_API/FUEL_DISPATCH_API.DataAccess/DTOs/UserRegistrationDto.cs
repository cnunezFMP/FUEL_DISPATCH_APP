using FUEL_DISPATCH_API.DataAccess.ValueGenerators;
using System.ComponentModel.DataAnnotations;

namespace FUEL_DISPATCH_API.DataAccess.DTOs
{
    public class UserRegistrationDto
    {
        public string? Email { get; set; }
        [Required] public string FullName { get; set; } = null!;

        [Required] public string Username { get; set; } = null!;

        [Required] public string Password { get; set; } = null!;

        [Required] public string PhoneNumber { get; set; } = null!;

        [Required] public DateTime? BirthDate { get; set; }

        [Required] public string FullDirection { get; set; } = null!;
        public int? DriverId { get; set; }
        public int? CompanyId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
