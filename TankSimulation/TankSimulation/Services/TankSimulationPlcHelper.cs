using PlcService.Sharp7;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TankSimulation.Services
{
    class TankSimulationPlcHelper :BaseS7PlcHelper
    {
        public int TankLevel { get; private set; }
        public TankSimulationPlcHelper(): base() {}

        internal override void DbRead()
        {
            lock (base._locker)
            {
                var buffer = new byte[10];
                int result = _client.DBRead(1, 0, buffer.Length, buffer);
                if (result == 0) //If no error
                {
                    //Casting byte array to value type
                    TankLevel = S7.GetIntAt(buffer, 2);
                }
                else
                {
                    throw new Exception(" Read error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }
        }
        public async Task StartPump()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[1];
                    buffer[0] = 1;                   
                    int result = _client.DBWrite(1, 0, 1, buffer);
                    if (result != 0)
                        throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            });
        }

        public async Task StopPump()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[1];
                    buffer[0] = 0;
                    int result = _client.DBWrite(1, 1, 1, buffer);
                    if (result != 0)
                        throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            });
        }

        public async Task StartAuto()
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[1];
                    buffer[0] = 1;
                    int result = _client.DBWrite(1, 4, 1, buffer);
                    if (result != 0)
                        throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
                }
            });
        }

        public void SetPumps(short value)
        {
            lock (_locker)
            {
                var buffer = new byte[2];
                buffer = BitConverter.GetBytes(value);
                int result = _client.DBWrite(1, 10, buffer.Length, buffer);
                if (result != 0)
                    throw new Exception(" Write error S7-1200 error: " + _client.ErrorText(result) + " Time: " + DateTime.Now.ToString("HH:mm:ss"));
            }
        }
     
    }
}
