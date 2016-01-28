using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class TempDailyModel
    {
        public Int32 id { get; set; }

        [EditorHttpName("duration")]
        public decimal duration { get; set; }

        [EditorHttpName("user_unit")]
        public string user_unit { get; set; }

    }
}