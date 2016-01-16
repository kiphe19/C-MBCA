using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class ReportModelCS
    {
        public Int32 vessel_id { get; set; }
        public String vessel_name { get; set; }
        public Decimal duration { get; set; }
        public int fuel { get; set; }
        public Int32 distance { get; set; }
        public string activity { get; set; }
        public Int32 jml { get; set; }
        public string unit { get; set; }
    }
}