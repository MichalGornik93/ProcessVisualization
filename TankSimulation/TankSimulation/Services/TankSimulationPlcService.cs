using PlcService.Sharp7;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TankSimulation.Services
{
    class TankSimulationPlcService :BaseS7PlcService
    {
        public float TankLevel { get; private set; }
        public float PumpsSpeed { get; private set; }
        public float FlowSpeed { get; private set; }
        public bool AutoState { get; private set; }
        public bool FlowState { get; private set; }
        public bool PumpsState { get; private set; }
        public TankSimulationPlcService(): base() {}

        internal override void DbRead()
        {
            lock (base._locker)
            {
                var buffer = new byte[15];
                int result = _client.DBRead(1, 0, buffer.Length, buffer);
                if (result == 0) //If no error
                {
                    //Casting byte array to value type
                    TankLevel = S7.GetRealAt(buffer, 2);
                    PumpsSpeed = S7.GetRealAt(buffer, 10);
                    FlowSpeed = S7.GetRealAt(buffer, 6);
                    AutoState = S7.GetBitAt(buffer, 14, 0);
                    PumpsState = S7.GetBitAt(buffer, 14, 1);
                    FlowState = S7.GetBitAt(buffer, 14, 2);
                }
                else
                {
                    throw new Exception(" Read error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }
        }
        public async Task StartPumpManual()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteBit(1, 0, 0, true);
                    Thread.Sleep(100);
                    WriteBit(1, 0, 0, false);
                }
            });
        }

        public async Task StartFlowManual()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteBit(1, 0, 1, true);
                    Thread.Sleep(100);
                    WriteBit(1, 0, 1, false);
                }
            });
        }

        public async Task StartAuto()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteBit(1, 0, 2, true);
                    Thread.Sleep(30);
                    WriteBit(1, 0, 2, false);
                }
            });
        }

        public async Task StopAuto()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    WriteBit(1, 0, 3, true);
                    Thread.Sleep(30);
                    WriteBit(1, 0, 3, false);
                }
            });
        }

        public void SetPumpsSpeed(double value)
        {
            lock (_locker)
            {
                WriteReal(1, 10, value);
            }
        }

        public void SetFlowSpeed(double value)
        {
            lock (_locker)
            {
                WriteReal(1, 6, value);
            }
        }

        private void WriteBit(int db, int pos, int bit, bool value)
        {
            lock (_locker)
            {
                var buffer = new byte[1];
                S7.SetBitAt(ref buffer, 0, bit, value);
                int result = _client.WriteArea(S7Consts.S7AreaDB, db, pos + bit, buffer.Length, S7Consts.S7WLBit, buffer);
                if (result != 0)
                    throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
            }
        }

        private void WriteInt(int db, int pos, short value)
        {
            lock (_locker)
            {
                var buffer = new byte[2];
                S7.SetIntAt(buffer, 0, value);
                int result = _client.DBWrite(db, pos, buffer.Length, buffer);
                if (result != 0)
                    throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
            }
        }

        private void WriteReal(int db, int pos, double value)
        {
            lock (_locker)
            {
                var buffer = new byte[4];
                S7.SetRealAt(buffer, 0, Convert.ToSingle(value));
                int result = _client.DBWrite(db, pos, buffer.Length, buffer);
                if (result != 0)
                    throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
            }
        }
    }
}
