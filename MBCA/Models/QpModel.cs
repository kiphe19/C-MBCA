using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class Datum
    {
        public string vessel { get; set; }
        public string tgl { get; set; }
        public IList<double> data { get; set; }
    }

    public class QpModel
    {
        public IList<Datum> data { get; set; }
        public IList<string> unit { get; set; }
    }
}