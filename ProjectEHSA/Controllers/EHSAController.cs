using ProjectEHSA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ProjectEHSA.Controllers
{
    public class EHSAController : ApiController
    {

        [System.Web.Http.HttpGet]

        public IHttpActionResult GetEmployeeSalary(int employeeId, DateTime startDate, DateTime endDate)
        {
            List<EHSAModel> employeeSalaries = new List<EHSAModel>();

            string connectionString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "ConnectionString.txt");
            string storedProcedureName = "sp_EHSA_EmployeeSalary";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            EHSAModel salaryModel = new EHSAModel
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString(),
                                HourlySalary = Convert.ToDecimal(reader["HourlySalary"]),
                                TotalWorkHours = Convert.ToInt32(reader["TotalWorkHours"]),
                                TotalSalary = Convert.ToDecimal(reader["TotalSalary"])
                            };

                            employeeSalaries.Add(salaryModel);
                        }

                        connection.Close();

                        return Ok(employeeSalaries);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }
        }
    }
}