using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }
    [Required] public string FullName { get; set; } = null!;

    [Required] public string Username { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;

    [Required] public string PhoneNumber { get; set; } = null!;

    [Required] public DateTime BirthDate { get; set; }

    public string FullDirection { get; set; } = null!;

    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? DriverId { get; set; }
    [JsonIgnore] public virtual Driver? Driver { get; set; }

    [JsonIgnore] public virtual ICollection<UsersRols> UsersRols { get; set; } = new List<UsersRols>();
    [JsonIgnore] public virtual ICollection<UsersBranchOffices> UsersBranchOffices { get; set; } = new List<UsersBranchOffices>();
    [JsonIgnore] public virtual ICollection<Role>? Rols { get; set; }

    [JsonIgnore] public virtual ICollection<UsersCompanies> UsersCompanies { get; set; } = new List<UsersCompanies>();
    [JsonIgnore] public virtual ICollection<Companies> Companies { get; set; } = new List<Companies>();
    [JsonIgnore] public virtual ICollection<BranchOffices> BranchOffices { get; set; } = new List<BranchOffices>();
}
