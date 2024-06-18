using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Companies
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? FullAddress { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CompanyRNC { get; set; }

    public string? EmailAddress { get; set; }

    public string? Website { get; set; }

    public string? Industry { get; set; }

    public DateOnly? DateEstablished { get; set; }

    public string? CEOFounder { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdateAt { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
    [JsonIgnore]
    public virtual ICollection<User>? Users { get; set; }
    [JsonIgnore]
    public virtual ICollection<UsersCompanies> UsersCompanies { get; set; } = new List<UsersCompanies>();
}
