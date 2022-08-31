using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.Dto
{
    public class ConfigDataDto
    {
        public object Brand { get; set; }
        public object Color { get; set; }
        public object UserStatuses { get; set; }
        public object CategoryStatuses { get; set; }
        public object OfferStatuses { get; set; }
        public object ProductStatuses { get; set; }
    }
}
