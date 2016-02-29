using DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class MainUnitModel
    {
        public Int32 id { get; set; }
        //[EditorHttpName("mainunit_name")]
        public string nama { get; set; }
        public string description { get; set; }
    }
}