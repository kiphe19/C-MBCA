using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class DrillModel
    {
        public class drilling_table
        {
            public Int32 id { get; set; }
            public Int32 id_unit { get; set; }
            public string well { get; set; }
            public string afe { get; set; }
            public string tgl { get; set; }
            public string t_start { get; set; }
            public string t_end { get; set; }
            public float durasi { get; set; }
        }
        public class unit_table
        {
            public Int32 id { get; set; }
            public string name { get; set; }
        }

       
    }
}