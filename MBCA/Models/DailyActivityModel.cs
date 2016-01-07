using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DailyActivityModel
    {
        [EditorHttpName("daily_date")]
        public string tgl { get; set; }
        [EditorHttpName("daily_vessel")]
        public string vessel { get; set; }
        [EditorHttpName("daily_activity")]
        public string activity { get; set; }
        [EditorHttpName("daily_duration")]
        public decimal duration { get; set; }
    }
}