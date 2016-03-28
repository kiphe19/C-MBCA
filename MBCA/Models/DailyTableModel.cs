using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DailyTableModel
    {
        public class daily_table
        {
            public int id { get; set; }
            public string tgl { get; set; }
            public int id_vessel { get; set; }
            public decimal standby { get; set; }
            public decimal loading { get; set; }
            public decimal steaming { get; set; }
            public decimal downtime { get; set; }
            public int id_unit { get; set; }
            public decimal duration { get; set; }
            public decimal distance { get; set; }
            public decimal fuel_tot { get; set; }
            public string user_log { get; set; }
            public string date_input { get; set; }
        }
        public class unit_table
        {
            public int id { get; set; }
            public string name { get; set; }

        }
        public class vessel_table
        {
            [EditorHttpName("id_kapal")]
            public int id { get; set; }
            public string name { get; set; }
        }
        
    }
}