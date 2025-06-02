using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFactory_MillProcess.Models
{
    public class Machine
    {
        public int Machine_id { get; set; }
        public double Temperature {  get; set; }
        public double Humidity { get; set; }
        public bool Repare_status { get; set; }
        public string RepareText => Repare_status ? "불량" : "양호";
        public bool Durability { get; set; }
        public string DurationText => Durability ? "양호" : "불량";
        public int Contamination_level { get; set; }
        public DateTime Create_date { get; set; }
    }
}

