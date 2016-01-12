using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DailyActivityModel
    {
        public Int32 id { get; set; }
        [EditorHttpName("daily_date")]
        public string tgl { get; set; }
        [EditorHttpName("daily_vessel")]
        public string vessel { get; set; }
        [EditorHttpName("daily_activity")]
        public string activity { get; set; }
        [EditorHttpName("daily_duration")]
        public decimal duration { get; set; }
        [EditorHttpName("daily_unit")]
        public string unit { get; set; }
        [EditorHttpName("daily_fuel")]
        public Int32 fuel { get; set; }
    }
}