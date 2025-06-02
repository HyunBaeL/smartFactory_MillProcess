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
        public double Thickness { get; set; }
        public double Hardness { get; set; }
        public double Strength { get; set; }
        public double CompressionRatio { get; set; }
        public double ErrorRatio { get; set; }
        public List<int> Errors { get; set; } = new List<int>();
        public List<int> CompleteCount { get; set; } = new List<int>();
    }
}
