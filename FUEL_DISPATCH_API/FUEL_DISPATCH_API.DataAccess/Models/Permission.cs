namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<RolsPermissions> RolsPermissions { get; set; } = [];
        public virtual ICollection<Role> Rols { get; set; } = [];
    }
}
