using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class TempDailyModel
    {
        public class temp_daily
        {
            public Int32 id { get; set; }

            //[EditorHttpName("duration")]
            public decimal duration { get; set; }

            //[EditorHttpName("user_unit")]
            //public string user_unit { get; set; }
            public string id_unit { get; set; }
            

        }
        public class unit_table
        {
            public string name { get; set; }

        }

    }
}