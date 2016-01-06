using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DistanceModel
    {
        [EditorHttpName("distance_name")]
        public string name { get; set; }
        public Int64 distance { get; set; }
    }
}