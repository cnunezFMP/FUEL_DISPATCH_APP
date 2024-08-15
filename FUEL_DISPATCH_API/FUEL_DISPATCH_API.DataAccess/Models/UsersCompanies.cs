//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//namespace FUEL_DISPATCH_API.DataAccess.Models
//{
//    public class UsersCompanies
//    {
//        [Required] public int UserId { get; set; }
//        [Required] public int CompanyId { get; set; }
//        public string? CreatedBy { get; set; }
//        public DateTime? CreatedAt { get; set; }
//        public string? UpdatedBy { get; set; }
//        public DateTime? UpdatedAt { get; set; }
//        [JsonIgnore] public virtual User? User { get; set; }
//        [JsonIgnore] public virtual Companies? Company { get; set; }
//    }
//}
