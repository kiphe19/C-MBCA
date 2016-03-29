using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DailyUnitVesselModel
    {
        public class detail_daily_table
        {
            [EditorHttpName("id_temp")]
            public int id { get; set; }
            [EditorHttpName("id_un")]
            public int id_unit { get; set; }
            [EditorHttpName("dur")]
            public int duration { get; set; }
        }
        public class unit_table
        {
            [EditorHttpName("unit")]
            public string name { get; set; }
        }
    }
}