using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Select * from ApplicationTypes";

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

        public static bool FindByID(int ApplicationTypeID, ref string Title, ref decimal Fees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypesID";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                Title = (string)reader["Title"];

                                Fees = decimal.Parse(reader["Fees"].ToString());
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
                        //Console.WriteLine("Error: " + ex.Message);
                        isFound = false;
                    }
                }
            }

            return isFound;
        }


        //public static bool FindByID(int ApplicationTypeID, ref string Title, ref decimal Fees)
        //{
        //    bool isFound = false;

        //    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //    {
        //        string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypesID";


        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
        //            try
        //            {
        //                connection.Open();
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        // The record was found
        //                        isFound = true;

        //                        Title = (string)reader["Title"];

        //                        Fees = decimal.Parse(reader["Fees"].ToString());
        //                    }
        //                    else
        //                    {
        //                        // The record was not found
        //                        isFound = false;
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //Console.WriteLine("Error: " + ex.Message);
        //                isFound = false;
        //            }
        //        }
        //    }

        //    return isFound;
        //}

    }


}
