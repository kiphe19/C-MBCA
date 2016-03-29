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
            [EditorHttpName("id_dt")]
            public int id { get; set; }
            [EditorHttpName("tg")]
            public string tgl { get; set; }
            //public int id_vessel { get; set; }
            [EditorHttpName("stb")]
            public decimal standby { get; set; }
            [EditorHttpName("ld")]
            public decimal loading { get; set; }
            [EditorHttpName("stm")]
            public decimal steaming { get; set; }
            [EditorHttpName("dt")]
            public decimal downtime { get; set; }
            //public int id_unit { get; set; }
            //public decimal duration { get; set; }
            [EditorHttpName ("mob")]
            public int mob_status { get; set; }
            [EditorHttpName("fuel_t")]
            public decimal fuel_tot { get; set; }
            //public string user_log { get; set; }
            //public string date_input { get; set; }
        }
        //public class unit_table
        //{
        //    public int id { get; set; }
        //    public string name { get; set; }

        //}
        public class vessel_table
        {
            [EditorHttpName("id_ves")]
            public int id { get; set; }
            [EditorHttpName("nm_ves")]
            public string name { get; set; }
        }
        
    }
}