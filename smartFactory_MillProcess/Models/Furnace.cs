using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.WPF;

namespace smartFactory_MillProcess.Models
{
    public class Furnace
    {
        public int DisplayTemperature { get; set; }  // 🔹 사용자가 입력한 온도
        public int GeneratedTemperature { get; set; }  // 🔹 난수로 생성된 온도
        public double AverageTemperature { get; set; }  // 🔹 60초 후 계산된 평균 온도
        public List<double> TemperatureHistory { get; set; } = new List<double>();
        // 
        public List<double> TimeHistory { get; set; } = new List<double>();
        public string UserInput { get; set; }

        public WpfPlot WpfPlot { get; set; }
        
        
    }
    
}
