using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class UnitDistanceDailyModel
    {
        public class unit_table
        {
            public Int32 Id { get; set; }
            public string name { get; set; }
            public string afe { get; set; }
            public string ket { get; set; }

        }

        public class unit_distance_table
        {
            public Int32 id { get; set; }
            public Int32 id_unit { get; set; }
            public Int32 id_mainunit { get; set; }
            public Int64 distance { get; set; }
            public string tgl { get; set; }

        }

        public class mainunit_table
        {
            public Int32 id { get; set; }
            public string nama { get; set; }
        }
        
       
    }
}