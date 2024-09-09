using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypesData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Select * from TestTypes";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            return dt;
        }

        public static bool Find(int ID, ref string Title, ref string Description, ref decimal Fees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestTypes WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    try
                    {
                        connection.Open();
                        //connection.Close();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                Title = (string)reader["Title"];
                                Description = (string)reader["Description"];
                                Fees = (decimal)reader["Fees"];
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobal.LogError(ex.Message);
                        isFound = false;
                    }
                }
                return isFound;
            }
        }

        public static bool UpdateInfoTest(int ID, string Title, string Description, decimal Fees)
        {
            int RowAffected = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "Update TestTypes set " +  
                               "Title = @Title, " +
                               "Description = @Description, " +
                               "Fees = @Fees " +
                               "where ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Fees", Fees);
                    
                    try
                    {
                        connection.Open();

                        RowAffected = command.ExecuteNonQuery();
                    }
                    catch
                    {
                        return false;
                    }
                }

            }

            return (RowAffected > 0);

        }

    }
}
