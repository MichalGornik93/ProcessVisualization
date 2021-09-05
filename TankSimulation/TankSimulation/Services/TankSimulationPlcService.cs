using PlcService.Sharp7;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TankSimulation.Helpers;

namespace TankSimulation.Services
{
    class TankSimulationPlcService :BaseS7PlcService
    {
        public float TankLevel { get; private set; }
        public float ParameterPumpsSpeed { get; private set; }
        public float ParameterFlowSpeed { get; private set; }

        private float _realPumpsSpeed;
        public float RealPumpsSpeed
        {
            get { return _realPumpsSpeed; }
            set 
            {
                Random random = new Random();
                _realPumpsSpeed = value + Convert.ToSingle(random.NextDouble(-0.25, 0.25));
                if (_realPumpsSpeed < 0)
                    _realPumpsSpeed = value;
            }
        }

        private float _realFlowSpeed;

        public float RealFlowSpeed
        {
            get { return _realFlowSpeed; }
            set 
            {
                Random random = new Random();
                _realFlowSpeed = value + Convert.ToSingle(random.NextDouble(-0.25, 0.25));
                if (_realFlowSpeed < 0)
                    _realFlowSpeed = value;
            }
        }

        public bool AutoState { get; private set; }
        public bool FlowState { get; private set; }
        public bool PumpsState { get; private set; }
        public bool AlarmGlobal { get; set; }
        public bool AlarmPumpSpeedHigh { get; set; }
        public bool AlarmFlowSpeedHigh { get; set; }

        public TankSimulationPlcService(): base() {}

        internal override void DbRead()
        {
            lock (base._locker)
            {
                var buffer1 = new byte[24];
                int result1 = _client.DBRead(1, 0, buffer1.Length, buffer1);
                if (result1 == 0) //If no error
                {
                    //Casting byte array to value type
                    TankLevel = S7.GetRealAt(buffer1, 2);
                    ParameterPumpsSpeed = S7.GetRealAt(buffer1, 10);
                    ParameterFlowSpeed = S7.GetRealAt(buffer1, 6);
                    AutoState = S7.GetBitAt(buffer1, 14, 0);
                    PumpsState = S7.GetBitAt(buffer1, 14, 1);
                    FlowState = S7.GetBitAt(buffer1, 14, 2);
                    RealFlowSpeed = S7.GetRealAt(buffer1, 16);
                    RealPumpsSpeed = S7.GetRealAt(buffer1, 20);
                }
                else
                {
                    throw new Exception(" Read error S7-1200 error: " + _client.ErrorText(result1) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }

                var buffer2 = new byte[1];
                int result2 = _client.DBRead(3, 0, buffer2.Length, buffer2);
                if (result2 == 0) //If no error
                {
                    //Casting byte array to value type
                    
                    AlarmGlobal = S7.GetBitAt(buffer2, 0, 0);
                    AlarmPumpSpeedHigh = S7.GetBitAt(buffer2, 0, 1);
                    AlarmFlowSpeedHigh = S7.GetBitAt(buffer2, 0, 2);
                }
                else
                {
                    throw new Exception(" Read error S7-1200 error: " + _client.ErrorText(result2) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
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
                    Thread.Sleep(500);
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
                    Thread.Sleep(500);
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
