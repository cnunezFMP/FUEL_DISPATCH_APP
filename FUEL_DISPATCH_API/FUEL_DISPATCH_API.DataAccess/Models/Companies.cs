using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models;

public partial class Companies
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? FullAddress { get; set; }

    public string? PostalCode { get; set; }
    [Phone] public string? PhoneNumber { get; set; }
    [Phone] public string? PhoneNumber2 { get; set; }

    public string? CompanyRNC { get; set; }

    public string? EmailAddress { get; set; }
    public string? EmailAddress2 { get; set; }

    public string? Website { get; set; }

    public string? Industry { get; set; }

    public DateTime? DateEstablished { get; set; }

    public string? CEOFounder { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
    [JsonIgnore] public virtual ICollection<ArticleDataMaster> Articles { get; set; } = new List<ArticleDataMaster>();
    [JsonIgnore] public virtual ICollection<User>? Users { get; set; } = new List<User>();
    [JsonIgnore] public virtual ICollection<WareHouse>? WareHouses { get; set; } = new List<WareHouse>();
    [JsonIgnore] public virtual ICollection<UsersCompanies> UsersCompanies { get; set; } = new List<UsersCompanies>();
    [JsonIgnore] public virtual ICollection<BranchOffices> BranchOffices { get; set; } = new List<BranchOffices>();
}
