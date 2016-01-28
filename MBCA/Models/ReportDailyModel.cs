using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class ReportDailyModel
    {
        public string tanggal { get; set; }
        public string vessel { get; set; }
        public string user_unit { get; set; }
        public decimal fuel_litre { get; set; }
        public decimal fuel_price { get; set; }
        public int fuel_curr { get; set; }
        public decimal charter_price { get; set; }
        public decimal mob_price { get; set; }
        public int charter_curr { get; set; }

    }
}