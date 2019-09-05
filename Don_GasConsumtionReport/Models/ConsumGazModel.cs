using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public class ConsumGazModel
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string PlcName { get; set; }
        public int GazValue { get; set; }
    }
}
