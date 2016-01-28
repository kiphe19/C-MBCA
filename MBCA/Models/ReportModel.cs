using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class ReportModel
    {
        public Int32 vessel_id { get; set; }
        public string vessel_name { get; set; }
        public string unit { get; set; }
        public string date { get; set; }
        public Decimal fuel_liter { get; set; }
        public Decimal fuel_usd { get; set; }
        public Decimal fuel_rp { get; set; }
        public Decimal standby_time { get; set; }
        public Decimal steaming_time { get; set; }
        public Decimal load_time { get; set; }
        public Decimal down_time { get; set; }
        public Decimal charter_usd { get; set; }
        public Decimal charter_rp { get; set; }
        public Decimal demob_usd { get; set; }
        public Decimal demob_rp { get; set; }
    }
}