using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class PlcService
    {
        public static int probaIncrementare;
        public static List<PlcObjectModel> ListaPlc = null;

        // Functie verificare daca exista PLC in lista in functie de nume
        public static bool IsCreatedPlcByName(string numePlc)
        {
            if (ListaPlc.Count > 0)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc.PlcName == numePlc) return true;
                }
            }
            return false;
        }

        // Functie verificare daca exista PLC in lista in functie de nume
        public static bool IsCreatedPlcByIp(string ipPlc)
        {
            if (ListaPlc.Count > 0)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc._client.IP == ipPlc) return true;
                }
            }
            return false;
        }

        // Functie GETPlcByName
        public static PlcObjectModel GetPlcByName(string numePlc)
        {
            if (ListaPlc.Count > 0)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc.PlcName == numePlc) return plc;
                }
            }
            return null;
        }

        //Functie GETPlcByIp
        public static PlcObjectModel GetPlcByIp(string ipPlc)
        {
            if (ListaPlc.Count > 0)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc._client.IP == ipPlc) return plc;
                }
            }
            return null;
        }

        //Functie creare Plc 
        public static void CreatePlc(string plcName, S7.Net.CpuType cpuType, string ip, short rack, short slot)
        {
            if ()
            ListaPlc.Add(new PlcObjectModel(plcName, cpuType, ip, rack, slot));
        }

        //Functie creare Plc Cuptor cu propulsie Pc6
        public static void CreatePlcCuptor()
        {
            ListaPlc.Add(new PlcObjectModel("PlcCuptor", S7.Net.CpuType.S7300, "172.16.4.104", 0, 2));
        }

        //Functie creare Plc Cuptor GaddaF2
        public static void CreatePlcGaddaF2()
        {
            ListaPlc.Add(new PlcObjectModel("PlcGaddaF2", S7.Net.CpuType.S7300, "10.0.0.11", 0, 2));
        }

        //Functie creare Plc Cuptor GaddaF4
        public static void CreatePlcGaddaF4()
        {
            ListaPlc.Add(new PlcObjectModel("PlcGaddaF4", S7.Net.CpuType.S7300, "10.0.0.13", 0, 2));
        }
    }
}
