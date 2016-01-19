using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables;

namespace chevron.Models
{
    public class CurrencyModel
    {
        public Int32 id { get; set; }
        [EditorHttpName("currency_name")]
        public string name { get; set; }
        [EditorHttpName("currency_value")]
        public Int32 value { get; set; }
        public String last_up { get; set; }
    }
}