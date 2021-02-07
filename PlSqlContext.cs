using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using EFCore_WebApp_PL_SQL.Models;

namespace EFCore_WebApp_PL_SQL.DAL
{
    public class PlSqlContext : DbContext
    {
        //public DbSet<EmpListByDeptName> EmpLists { get; set; }
        public DbSet<Department> Departments { get; set; }

        public PlSqlContext(
            DbContextOptions<PlSqlContext> options) : base(options)
        {
        }
    }

}
