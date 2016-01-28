using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace chevron.Models
{
    public class FuelModel
    {
        public String tgl { get; set; }
        public Decimal cost_usd { get; set; }
        public Decimal currency_type { get; set; }
    }
}