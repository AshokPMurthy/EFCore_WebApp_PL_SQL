using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using EFCore_WebApp_PL_SQL.DAL;
using EFCore_WebApp_PL_SQL.Models;
using Oracle.ManagedDataAccess.Client;

namespace EFCore_WebApp_PL_SQL.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PlSqlContext _context; // refrences the "DbContext"

        // Constructor that initializes our "_context" via injection.
        public IndexModel(PlSqlContext context)
        {
            _context = context;
        }

        // Properties for Searching
        [BindProperty(SupportsGet = true)]  // binds form values and query strings with same name as property
        public string SearchString { get; set; } // text users enter in the search text box. 

        // Drop-Down-Box for filtering by Department names.
        public SelectList DeptNames { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DeptName { get; set; }

        // property that's read by the Razor page "Index.cshtml"
        public IList<EmpListByDeptName> EmpData { get; set; }

        // The method that is called when the website first comes up (http://localhost:####)
        public async Task OnGetAsync()
        {
            // Execute the "EMP_LIST_BY_DEPTNAME" PL/SQL proc, and 
            // obtain a DataTable of customer data to display.
            using (OracleConnection con = (OracleConnection)_context.Database.GetDbConnection())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        // Get a list of all Department Names
                        cmd.CommandText = "SELECT Department_Name FROM DEPARTMENTS ORDER BY 1";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            var depts = new List<string>(); ;
                            while (rdr.Read())
                            {
                                depts.Add(rdr["Department_Name"].ToString());
                            }
                            DeptNames = new SelectList(depts);
                        }

                        // Invoke the PL/SQL procedure (with an input parameter)
                        cmd.CommandText = "EMP_LIST_BY_DEPTNAME"; // name of PL/SQL proc in "learn" schema
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Specify Department Name to PL/SQL proc as “INPUT” parm.
                        OracleParameter deptname = new OracleParameter("deptname", OracleDbType.Varchar2, ParameterDirection.Input);
                        deptname.Value = string.IsNullOrEmpty(DeptName) ? " " : DeptName;
                        cmd.Parameters.Add(deptname);

                        // Specify parameter that holds the DataSet OUTPUT by PL/SQL procedure.
                        OracleParameter dataset = new OracleParameter("dataset", OracleDbType.RefCursor, ParameterDirection.Output);
                        cmd.Parameters.Add(dataset);

                        DataSet ds = new DataSet();
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(ds);

                        DataTable dt = ds.Tables[0];

                        // If a "SearchString" was input, then filter the LAST_NAME by "SearchString"
                        if (!string.IsNullOrEmpty(SearchString))
                        {
                            dt = dt.AsEnumerable()
                             .Where(r => r.Field<string>("LAST_NAME").ToLower() == SearchString.ToLower())
                             .CopyToDataTable();
                        }

                        // now convert the DataTable into an IList<EmpListByDeptName>
                        var result = dt.AsEnumerable().Select(row =>
                            new EmpListByDeptName
                            {
                                State_province = row.Field<string>("State_province"),
                                Country_Id = row.Field<string>("Country_Id"),
                                Department_Name = row.Field<string>("Department_Name"),
                                Last_Name = row.Field<string>("Last_Name"),
                                Job_Title = row.Field<string>("Job_Title"),
                                Salary = row.Field<double?>("Salary"),
                                Commission_Pct = row.Field<float?>("Commission_Pct")
                            });

                        // and load it into class property "EmpData"
                        EmpData = await Task.FromResult(result.ToList());
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }//end- using (OracleCommand cmd = con.CreateCommand())

            }//end- using (OracleConnection con = (OracleConnection)_context.Database.GetDbConnection())

        }//end- OnGetAsync()

    }//end- class IndexModel

}//end- namespace
