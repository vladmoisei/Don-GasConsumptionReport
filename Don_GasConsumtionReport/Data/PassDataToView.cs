using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public class PassDataToView
    {
        public string Clock { get; set; }
        public bool IsStartedBackgroundService { get; set; }
        public bool IsCreatedPlcCuptor { get; set; }
        public bool IsConnectedPlcCuptor { get; set; }
        public bool IsCreatedPlcGaddaF2{ get; set; }
        public bool IsConnectedPlcGaddaF2 { get; set; }
        public bool IsCreatedPlcGaddaF4 { get; set; }
        public bool IsConnectedPlcGaddaF4 { get; set; }

    }
}
