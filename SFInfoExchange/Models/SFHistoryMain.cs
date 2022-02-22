using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFInfoExchange.Models
{
    public class SFHistoryMain
    {
        public string Id { get; set; }
        public string HisMainId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectRecordId { get; set; }
        public string ModifiedFields { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedDatetime { get; set; }



    }
}
