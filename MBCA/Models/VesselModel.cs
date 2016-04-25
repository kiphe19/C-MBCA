using DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;

namespace chevron.Models
{
    public class VesselModel
    {
        public string id { get; set; }
        [EditorHttpName("nm")]
        public string name { get; set; }
        [EditorHttpName("own")]
        public string vs_owner { get; set; }
        [EditorHttpName("desc")]
        public string vs_desc { get; set; }
    }
}