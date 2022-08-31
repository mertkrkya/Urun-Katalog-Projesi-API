using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.Models
{
    public class GenericConfigModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
    public class Brand : GenericConfigModel
    {
    }
    public class Color : GenericConfigModel
    {

    }
}
