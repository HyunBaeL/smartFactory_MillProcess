using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFactory_MillProcess.Models
{
    public class MachineStatus
    {
        public int TaskId { get; set; }
        public double ThicknessResult { get; set; }
        public double HardnessResult { get; set; }
        public double StrenghResult { get; set; }
        public double ReductionRatidResult { get; set; }
        public bool DefectStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
