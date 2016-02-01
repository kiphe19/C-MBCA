using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class DailyTableModel
    {
        public string tgl { get; set; }
        public string vessel { get; set; }
        public decimal standby { get; set; }
        public decimal loading { get; set; }
        public decimal steaming { get; set; }
        public decimal downtime { get; set; }
        public string user_unit { get; set; }
        public decimal duration { get; set; }
        public decimal distance { get; set; }
        public decimal fuel_tot { get; set; }
        public string user_log { get; set; }
        public string date_input { get; set; }
    }
}