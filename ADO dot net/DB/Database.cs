using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace ADO_dot_net.DB
{
    public class Database
    {
        public static string sqlDataSource = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Company;Trusted_Connection=True;";
        //public static string sqlDataSource = "Data Source=DESKTOP-GV4424J;Initial Catalog=TestDB ; Integrated Security = True;";

        public DataTable GetData(string str)
        {
            DataTable objresult = new DataTable();

            try
            {
                SqlDataReader myReader;
                using (SqlConnection mycon = new SqlConnection(sqlDataSource))
                {
                    mycon.Open();

                    SqlCommand command = new SqlCommand(str, mycon);
                    myReader = command.ExecuteReader();
                        objresult.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objresult;
        }

        public int ExecuteData(string str, params IDataParameter[] sqlParams)
        {
            int rows = -1;
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        if (sqlParams != null)
                        {
                            foreach (IDataParameter para in sqlParams)
                            {
                                cmd.Parameters.Add(para);
                            }
                            rows = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return rows;
        }
    }
}
