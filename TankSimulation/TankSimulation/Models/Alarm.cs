using System;
using System.Collections.Generic;
using System.Text;

namespace TankSimulation.Models
{
    public class Alarm
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Adress { get; set; }
        public DateTime DateTime { get; set; }
    }
}
