using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFactory_MillProcess.Models
{
    public class Users
    {
        public string Id { get; set; }
        public string Pwd { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int EmployeeId { get; set; } // 사원번호
        //public bool delYN { get; set; }

        public override string ToString()
        {
            return Id; 
        }
    }


}
