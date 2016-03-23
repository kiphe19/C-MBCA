using DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class DailyUnitActivityModel
    {
        public int id { get; set; }

        public string user_unit { get; set; }
        public Int32 id_unit { get; set; }
        public decimal durasi { get; set; }
        public int jarak { get; set; }
        public int hit { get; set; }

    }
}