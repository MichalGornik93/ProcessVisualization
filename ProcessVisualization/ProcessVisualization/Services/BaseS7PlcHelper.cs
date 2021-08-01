using PlcService.Sharp7;
using System;
using System.Timers;

namespace MaintenanceDashboard.Common.PlcService
{
    public abstract class BaseS7PlcHelper
    {
        internal readonly S7Client _client;
        private readonly Timer _timer;
        internal DateTime _lastScanTime;

        internal volatile object _locker = new object();
        public ConnectionStates ConnectionState { get; private set; }
        public TimeSpan ScanTime { get; private set; }
        public event EventHandler ValuesRefreshed;

        public BaseS7PlcHelper() 
        {
            _client = new S7Client();
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed; //Start event
        }

        public void Connect(string ipAddress, int rack, int slot)
        {
            try
            {
                ConnectionState = ConnectionStates.Connecting;
                int result = _client.ConnectTo(ipAddress, rack, slot);
                if (result == 0)
                {
                    ConnectionState = ConnectionStates.Online;
                    _timer.Start();
                }
                else
                {
                    ConnectionState = ConnectionStates.Offline;
                    throw new Exception(" Connection to S7-1200 error: " + _client.ErrorText(result)+" Time: "+ DateTime.Now.ToString("HH:mm:ss"));

                }
                OnValuesRefreshed(); //Subscriber event "ValuesRefreshed"
            }
            catch
            {
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed(); //Subscriber event "ValuesRefreshed"
                throw;
            }
        }

        public void Disconnect()
        {
            if (_client.Connected)
            {
                _timer.Stop();
                _client.Disconnect();
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed(); //Subscriber event "ValuesRefreshed"
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) 
        {
            try
            {
                _timer.Stop();
                ScanTime = DateTime.Now - _lastScanTime;
                DbRead(); //Cyclic refreshing of data block values
                OnValuesRefreshed(); //Subscriber event "ValuesRefreshed"
            }
            finally
            {
                _timer.Start();
            }
            _lastScanTime = DateTime.Now;
        }

        internal virtual void DbRead() {}

        private void OnValuesRefreshed()
        {
            ValuesRefreshed?.Invoke(this, new EventArgs());
        }
    }
}
