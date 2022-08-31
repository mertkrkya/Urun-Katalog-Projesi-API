using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunKatalogProjesi.Data.Configurations
{
    public class SystemOptionConfig
    {
        public int BlockAccessFailedCount { get; set; }
        public string MaxFileKbSize { get; set; }
        public string LogFileDirectory { get; set; }
        public string ImportPhotoDirectory { get; set; }

    }
    public class EmailConfig
    {
        public string EmailAccount { get; set; }
        public string EmailPassword { get; set; }
        public string EmailHost { get; set; }
        public int EmailPort { get; set; }
        public string EmailDisplayName { get; set; }

    }
}
