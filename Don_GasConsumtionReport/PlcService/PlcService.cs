using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public static class PlcService
    {
        public static int probaIncrementare;
        public static List<PlcObjectModel> ListaPlc = new List<PlcObjectModel>();

        // Functie verificare daca exista PLC in lista in functie de nume
        public static bool IsCreatedPlcByName(string numePlc)
        {
            if (ListaPlc.Count > 0)
            //if (ListaPlc != null)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc.PlcName == numePlc) return true;
                }
            }
            return false;
        }

        // Functie verificare daca exista PLC in lista in functie de ip
        public static bool IsCreatedPlcByIp(string ipPlc)
        {
            if (ListaPlc.Count > 0)
                //if (ListaPlc != null)
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
                //if (ListaPlc != null)
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
                //if (ListaPlc != null)
            {
                foreach (PlcObjectModel plc in ListaPlc)
                {
                    if (plc._client.IP == ipPlc) return plc;
                }
            }
            return null;
        }

        // IP-uri PLC: "172.16.4.104" "10.0.0.11" "10.0.0.13"
        //Functie creare Plc 
        public static void CreatePlc(string plcName, S7.Net.CpuType cpuType, string ip, short rack, short slot)
        {
            // Verificare daca daca exista plc Creat in lista
            if (IsCreatedPlcByIp(ip) || IsCreatedPlcByName(plcName)) return;
            PlcObjectModel plc = new PlcObjectModel(plcName, cpuType, ip, rack, slot);
            // Creaza plc nou in lista
            ListaPlc.Add(plc);
            // Set Adresa variabila citire index in functie de nume
            switch (plc.PlcName)
            {
                case "PlcCuptor":
                    plc.AdresaIndexGaz = "MD130";
                    break;
                case "PlcGaddaF2":
                    plc.AdresaIndexGaz = "DB10.DBD46"; //Dint
                    break;
                case "PlcGaddaF4":
                    plc.AdresaIndexGaz = "DB10.DBD46"; //Dint
                    break;
                default:
                    break;
            }
        }

        // Functie stergere Plc dupa nume
        public static void DeletePlcByName(string numePlc)
        {
            if (IsCreatedPlcByName(numePlc))
                ListaPlc.Remove(GetPlcByName(numePlc));
        }

        // Functie Conectare Plc dupa nume
        public static void ConnectPlcByName(string numePlc)
        {
            if (IsCreatedPlcByName(numePlc)) GetPlcByName(numePlc).ConnectPlc();
        }

        // Functie Deconectare Plc dupa nume
        public static void DeConnectPlcByName(string numePlc)
        {
            if (IsCreatedPlcByName(numePlc)) GetPlcByName(numePlc).DeconnectPlc();
        }

        // Functie verificare Conexiune Plc dupa nume
        public static bool IsConnectedPlcByName(string numePlc)
        {
            if (IsCreatedPlcByName(numePlc)) return GetPlcByName(numePlc).IsConnected;
            return false;
        }

        // Functie Verificare Adresa IP
        public static bool IsAvailableIpAdress(string ip)
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(ip, 200);
            if (reply.Status.ToString() == "Success")
                return true;
            return false;
        }

        // Functie refresh values of plc in list
        public static void RefreshValuesListaPlc()
        {
            foreach (PlcObjectModel plc in ListaPlc)
            {
                if (plc.IsConnected) plc.RefreshValues();
            }
        }
    }
}
