using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class ActivityModel
    {
        [EditorHttpName("activity_name")]
        public string name { get; set; }
        [EditorHttpName("activity_ket")]
        public string ket { get; set; }
    }
}