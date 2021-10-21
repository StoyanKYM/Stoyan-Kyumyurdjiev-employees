using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SirmaSolutionsLongestPeriodSecond
{
    class Program
    {
        static void Main(string[] args)
        {


            Dictionary<ValueTuple<int, int>, int> employeeDict = new Dictionary<ValueTuple<int, int>, int>();

            // Reads the .txt file.
            List<Employee> employeesList = ReadTextFile("employeeInfo.txt");


            for (int i = 0; i < employeesList.Count - 1; i++)
            {
                for (int j = i + 1; j < employeesList.Count; j++)
                {

                    // Since we compare [i] to [j] we dont need the records where
                    // EmpID is the same for i and j,
                    // ProjectID is different,
                    // DateFrom(start date) date of i is bigger than DateTo(end date) of j,
                    // DateTo of i is earlier than DateFrom of j

                    if ((employeesList[i].EmpID == employeesList[j].EmpID)
                       || (employeesList[i].ProjectID != employeesList[j].ProjectID)
                       || (employeesList[i].DateFrom > employeesList[j].DateTo)
                       || (employeesList[i].DateTo < employeesList[j].DateFrom))
                    {
                        continue;
                    }

                    int totalDaysWorkedTogether;


                    //eg.     2015-01-01                   2016-01-01                   2018-01-01                  2017-01-01
                    if (employeesList[i].DateFrom <= employeesList[j].DateFrom && employeesList[i].DateTo >= employeesList[j].DateTo)
                    {                               
                        totalDaysWorkedTogether = (employeesList[j].DateTo - employeesList[j].DateFrom).Days;
                    }
                    //eg.     2016-01-01                   2015-01-01                   2017-01-01                  2018-01-01
                    else if (employeesList[i].DateFrom >= employeesList[j].DateFrom && employeesList[i].DateTo <= employeesList[j].DateTo)
                    {
                        totalDaysWorkedTogether = (employeesList[i].DateTo - employeesList[i].DateFrom).Days;
                    }
                    //eg.     2016-01-01                   2015-01-01                   2017-01-01                  2016-01-01
                    else if (employeesList[i].DateFrom >= employeesList[j].DateFrom && employeesList[i].DateTo >= employeesList[j].DateTo)
                    {
                        totalDaysWorkedTogether = (employeesList[j].DateTo - employeesList[i].DateFrom).Days;
                    }
                    //eg.     2015-01-01                   2016-01-01                   2017-01-01                  2018-01-01
                    else if (employeesList[i].DateFrom <= employeesList[j].DateFrom && employeesList[i].DateTo <= employeesList[j].DateTo)
                    {
                        totalDaysWorkedTogether = (employeesList[i].DateTo - employeesList[j].DateFrom).Days;
                    }
                    else
                    {
                        totalDaysWorkedTogether = -1;
                    }

                    if (totalDaysWorkedTogether == -1)
                    {
                        continue;
                    }


                    Employee emp1 = new Employee()
                    {
                        EmpID = employeesList[i].EmpID,
                        ProjectID = employeesList[i].ProjectID
                    };

                    Employee emp2 = new Employee()
                    {
                        EmpID = employeesList[j].EmpID,
                        ProjectID = employeesList[j].ProjectID
                    };


                    if (!employeeDict.ContainsKey((emp1.EmpID, emp2.EmpID)))
                    {
                        employeeDict.Add((emp1.EmpID, emp2.EmpID), totalDaysWorkedTogether);
                    }
                    else
                    {
                        employeeDict[(emp1.EmpID, emp2.EmpID)] += totalDaysWorkedTogether;
                    }
                    

                }
            }

            // If dictionary is empty there are no employees that worked together.
            if (employeeDict.Count == 0)
            {
                Console.WriteLine("No employees");
                return;
            }

            // Orders the dictionary by value of days
            var sortedDict = from entry in employeeDict orderby entry.Value descending select entry;

            var longestDays = sortedDict.First();
            
            Console.WriteLine($"The two longest working employees together are with id's: ({longestDays.Key.Item1}) - ({longestDays.Key.Item2}) and with total working days of {longestDays.Value} days.");

        }

        private static List<Employee> ReadTextFile(string fileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            List<Employee> employeesList = new List<Employee>();

            // Reads the first line, otherwise will try to parse it.
            string headerLine = streamReader.ReadLine();
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                string[] lineArgs = line.Split(", ");
                int empID = int.Parse(lineArgs[0]);
                int projectID = int.Parse(lineArgs[1]);
                DateTime dateFrom = DateTime.Parse(lineArgs[2]);
                DateTime dateTo;
                if (lineArgs[3] == "NULL")
                {
                    dateTo = DateTime.UtcNow;
                }
                else
                {
                    dateTo = DateTime.Parse(lineArgs[3]);
                }
                Employee currentEmployee = new Employee(empID, projectID, dateFrom, dateTo);
                employeesList.Add(currentEmployee);
            }


            return employeesList;
        }
    }
}
