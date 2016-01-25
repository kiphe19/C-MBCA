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

        public decimal dur { get; set; }
        //public String user_log { get; set; }
        //public DateTime date_input { get; set; }

    }
}