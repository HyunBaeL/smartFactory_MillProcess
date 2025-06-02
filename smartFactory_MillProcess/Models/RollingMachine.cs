using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFactory_MillProcess.Models
{
    public class RollingMachine
    {
        public int RollSpeed { get; set; }
        public double InitialThickness { get; set; }
        public double FinalThickness { get; set; }
        public double Hardness { get; set; }
        public double Strength { get; set; }
        public double CompressionRatio { get; set; }
        public double ErrorRatio { get; set; }
        public int Errors { get; set; }
        public int CompleteCount { get; set; }
        public string UserInput { get; set; }

    }
}
