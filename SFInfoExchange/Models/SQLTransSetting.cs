using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SQLTransSetting
    {
        public string TransId { get; set; }
        public string BuiltTime { get; set; }
        public string TransParams { get; set; }
        public string TransDesc { get; set; }
        public string DataSource { get; set; }
        public string Destination { get; set; }

    }
}
