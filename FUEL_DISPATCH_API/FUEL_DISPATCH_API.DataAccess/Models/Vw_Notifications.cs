using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Vw_Notifications
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Titulo { get; set; }
        public string? Mensaje { get; set; }
    }
}
