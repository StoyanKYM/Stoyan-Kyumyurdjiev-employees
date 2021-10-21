using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmaSolutionsLongestPeriodSecond
{
    public class Employee
    {

        public Employee()
        {

        }

        public Employee(int empID, int projectID, DateTime dateFrom, DateTime dateTo)
        {
            EmpID = empID;
            ProjectID = projectID;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        

    }
}
