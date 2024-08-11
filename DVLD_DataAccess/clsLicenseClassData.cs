using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {

            DataTable dt = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {

                string query = @"SELECT  LicenseClassID, ClassName, ClassDescription FROM LicenseClasses";

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

        public static bool GetLicenseClassByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref byte MinimumAllowAge, ref byte ValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM LicenseClasses WHERE ClassName = @ClassName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", ClassName);


                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                LicenseClassID = (int) reader["LicenseClassID"];
                                ClassDescription = (string) reader["ClassDescription"];
                                MinimumAllowAge = (byte) reader["MinimumAllowAge"];
                                ValidityLength = (byte) reader["ValidityLength"];
                                ClassFees = float.Parse(reader["ClassFees"].ToString());
                                
                                isFound = true;
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                    catch
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
    
        public static bool GetLicenseClassByLicenseClassID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowAge, ref byte ValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ClassName = (string) reader["ClassName"];
                                ClassDescription = (string) reader["ClassDescription"];
                                MinimumAllowAge = (byte) reader["MinimumAllowAge"];
                                ValidityLength = (byte) reader["ValidityLength"];
                                ClassFees = float.Parse(reader["ClassFees"].ToString());
                                
                                isFound = true;
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                    catch
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

    
    }
}
