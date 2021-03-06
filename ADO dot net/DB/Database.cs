﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ADO_dot_net.Models;
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
      
        private string TestProcedure = " CREATE PROCEDURE TestProcedure AS BEGIN SELECT * FROM Inventory";
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
        public int DeleteData(string query)
        {
            int row = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        row = cmd.ExecuteNonQuery(); 
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return row;
        }
        public bool CreateTable(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteReader();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool AddColumn (string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }


        // Stored Procedures
        public async Task<List<Inventory>> getAll()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    using (SqlCommand cmd = new SqlCommand("getAllData", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var response = new List<Inventory>();
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToValue(reader));
                            }
                        }
                        return response;
                    }

                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        private Inventory MapToValue(SqlDataReader reader)
        {
            return new Inventory()
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Price = (decimal)reader["Price"],
                Quantity = (int)reader["Quantity"],
                AddedOn = (DateTime)reader["AddedOn"]
            };
        }
    }
}
