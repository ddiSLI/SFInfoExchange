using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SFObjectValue
    {
        public string Id { get; set; }
        public string ObjectName { get; set; }
        public string ObjectRecordId { get; set; }
        public string ModifiedField { get; set; }
        public string ModifiedValue { get; set; }

    }
}
