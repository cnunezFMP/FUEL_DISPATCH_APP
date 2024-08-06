using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;
/// <summary>
/// Esta es la clase de las compañias
/// </summary>
public partial class Companies
{
    public int Id { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public string? FullAddress { get; set; }

    [Required] public string? PostalCode { get; set; }
    [Phone, Required] public string? PhoneNumber { get; set; }
    [Phone] public string? PhoneNumber2 { get; set; }

    [Required] public string? CompanyRNC { get; set; }

    [Required] public string? EmailAddress { get; set; }
    public string? EmailAddress2 { get; set; }

    public string? Website { get; set; }
    [Required] public string? Industry { get; set; }
    public DateTime? DateEstablished { get; set; }
    [Required] public string? CEOFounder { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual ICollection<Zone>? Zones { get; set; } = new HashSet<Zone>();
    [JsonIgnore] public virtual ICollection<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
    [JsonIgnore] public virtual ICollection<Road>? Roads { get; set; } = new List<Road>();
    [JsonIgnore] public virtual ICollection<ArticleDataMaster>? Articles { get; set; } = new List<ArticleDataMaster>();
    [JsonIgnore] public virtual ICollection<User>? Users { get; set; } = new List<User>();
    [JsonIgnore] public virtual ICollection<WareHouse>? WareHouses { get; set; } = new List<WareHouse>();
    [JsonIgnore] public virtual ICollection<UsersCompanies>? UsersCompanies { get; set; } = new List<UsersCompanies>();
    [JsonIgnore] public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
    [JsonIgnore] public virtual ICollection<BranchOffices>? BranchOffices { get; set; } = new List<BranchOffices>();

    public CompanySAPParams? CompanySAPParams { get; set; }
}
