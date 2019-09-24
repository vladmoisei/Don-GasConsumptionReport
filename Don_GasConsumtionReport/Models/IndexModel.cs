using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public class IndexModel
    {
        public int Id { get; set; }
        public string Data { get; set; }
        [DisplayName("Nume Plc")]
        public string PlcName { get; set; }
        [DisplayName("Index gaz")]
        public int IndexValue { get; set; }
        [DisplayName("Consum gaz")]
        public int GazValue { get; set; }
    }
}
