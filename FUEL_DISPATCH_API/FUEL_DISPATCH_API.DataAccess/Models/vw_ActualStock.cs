using System.ComponentModel.DataAnnotations;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    // DONE: Hacer los servicios y controlador
    public class vw_ActualStock
    {
        public int WareHouseId { get; set; }
        public int BranchOfficeId { get; set; }
        public string BranchOfficeName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string WareHouseName { get; set; }
        public string WareHouseCode { get; set; }
        [Required] public decimal? MinCapacity { get; set; }
        public int ItemId { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleCode { get; set; }
        public decimal StockQty { get; set; }
    }
}
