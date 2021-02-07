
using System.ComponentModel.DataAnnotations;


namespace EFCore_WebApp_PL_SQL.Models
{
    public class Department
    {
        [Key]
        public int Department_Id { get; set; }
        public string Department_Name { get; set; }
        public int? Manager_Id { get; set; }
        public int? Location_Id { get; set; }
    }
}
