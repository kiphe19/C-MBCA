using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class UnitModel
    {
        public Int32 id { get; set; }
        [EditorHttpName("unit_name")]
        public string name { get; set; }
        [EditorHttpName("unit_cat")]
        public string cat { get; set; }
        [EditorHttpName("unit_distance")]
        public Int64 distance { get; set; }
        [EditorHttpName("unit_ket")]
        public string ket { get; set; }
    }
}