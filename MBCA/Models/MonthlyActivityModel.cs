using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class MonthlyActivityModel
    {
        [EditorHttpName("monthly_date")]
        public string tgl { get; set; }
        [EditorHttpName("monthly_vessel")]
        public string vessel { get; set; }
        [EditorHttpName("monthly_activity")]
        public string activity { get; set; }
        [EditorHttpName("monthly_duration")]
        public decimal duration { get; set; }
        [EditorHttpName("monthly_unit")]
        public string unit { get; set; }
    }
}