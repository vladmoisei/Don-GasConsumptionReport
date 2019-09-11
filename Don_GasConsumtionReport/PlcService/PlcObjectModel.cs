using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Don_GasConsumtionReport
{
    public class PlcObjectModel
    {
        public Plc _client;
        public string PlcName { get; set; }
        public bool IsConnected { get; set; }
        public bool IsEnable { get; set; }
        public bool IsCreated { get; set; }
        public TimeSpan ScanTime { get; set; }
        private DateTime _lastScanTime;
        private volatile object _locker = new object();

        // Adrese Variabile Plc 
        public string AdresaIndexGaz { get; set; } = "";

        // Valoare Variabile Plc
        public uint ValoareIndexGaz { get; set; }

        // Constructor fara parametru
        public PlcObjectModel()
        {
            PlcName = "";
        }

        // Constructor cu parametri, creare PLC Object
        public PlcObjectModel(string plcName, CpuType cpuType, string ip, short rack, short slot)
        {
            PlcName = plcName;
            _client = new Plc(cpuType, ip, rack, slot);
            IsConnected = false;
            IsCreated = true;
        }

        // Functie Verificare Adresa IP
        public bool IsAvailableIpAdress()
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(_client.IP, 200);
            if (reply.Status.ToString() == "Success")
                return true;
            return false;
        }

        // Functie conectare Plc
        public void ConnectPlc()
        {
            try
            {
                var _cancelTasks = new CancellationTokenSource();
                var performTaskCheckAvailability = Task.Run(() =>
                {
                    if (!IsConnected)
                    {
                        if (IsAvailableIpAdress())
                        {
                            _client.Open(); // Deschidere conexiune Plc
                            if (_client.IsConnected) IsConnected = true; // Setare IsConected daca a reusit conexiunea
                        }
                        else return;
                    }
                    else return;
                }, _cancelTasks.Token);
                if (!performTaskCheckAvailability.Wait(TimeSpan.FromSeconds(0.5))) _cancelTasks.Cancel(); // Daca nu mai raspundein timp util se opreste Task
            }
            catch (Exception) { }
        }

        // Functie deconectare Plc
        public void DeconnectPlc()
        {
            try
            {
                var _cancelTasks = new CancellationTokenSource();
                var performTaskCheckAvailability = Task.Run(() =>
                {
                    if (IsConnected)
                    {
                        _client.Close(); // Inchidere conexiune Plc
                        if (!_client.IsConnected) IsConnected = false; // Setare IsConected to false
                    }
                    else return;
                }, _cancelTasks.Token);
                if (!performTaskCheckAvailability.Wait(TimeSpan.FromSeconds(0.5))) _cancelTasks.Cancel(); // Daca nu mai raspundein timp util se opreste Task
            }
            catch (Exception) { }
        }

        // Functie Citire variabile predefinite
        private void ReadTagValues()
        {
            lock (_locker)
            {
                ValoareIndexGaz = (uint)_client.Read(AdresaIndexGaz); // MD114 Index convertit
            }
        }

        // Functie Refresh valori citite
        public void RefreshValues()
        {
            try
            {
                var _cancelTasks = new CancellationTokenSource();
                var performTaskCheckAvailability = Task.Run(() =>
                {
                    if (IsConnected)
                    {
                        _lastScanTime = DateTime.Now;
                        ReadTagValues(); // Citire semnale Plc
                        ScanTime = DateTime.Now - _lastScanTime; // Determinare ScanTime Plc
                    }
                    else return;
                }, _cancelTasks.Token);
                if (!performTaskCheckAvailability.Wait(TimeSpan.FromSeconds(0.5))) _cancelTasks.Cancel(); // Daca nu mai raspundein timp util se opreste Task
            }
            catch (AggregateException exAgg) { IsConnected = false; }
            catch (OperationCanceledException exOPCa) { IsConnected = false; }
            catch (PlcException exPLC) { IsConnected = false; }            
            catch (Exception) { }
        }

    }
}
