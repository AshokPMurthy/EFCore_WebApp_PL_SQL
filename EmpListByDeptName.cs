using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore_WebApp_PL_SQL.Models
{
    /// <summary>
    /// Class that mirrors fields returned by PL/SQL proc "EMP_LIST_BY_DEPTNAME"
    /// </summary>
    public class EmpListByDeptName
    {
        [Display(Name ="State")]
        public string State_province { get; set; }

        [Display(Name = "Country")]
        public string Country_Id { get; set; }

        [Display(Name = "Department")]
        public string Department_Name { get; set; }

        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        [Display(Name = "Job Title")]
        public string Job_Title { get; set; }

        public double? Salary { get; set; }

        [Display(Name = "Comission %")]
        public float? Commission_Pct { get; set; }
    }
}
