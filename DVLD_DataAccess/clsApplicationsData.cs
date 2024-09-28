using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationsData
    {
        public static int AddNewApplication (int ApplicationTypeID, int PersonID, DateTime ApplicationDate, DateTime LastStatuDate, float PaidFees, int CreatedByUserID)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int ApplicaitonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications 
                            (ApplicationTypeID,PersonID,
                            ApplicationDate, LastStatuDate, PaidFees,
                            CreatedByUserID)
                            VALUES (@ApplicationTypeID,
                                    @PersonID,@ApplicationDate,
                                    @LastStatuDate, @PaidFees, @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@LastStatuDate", LastStatuDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicaitonID = insertedID;
                }
            }

            catch (Exception ex)
            {
                clsGlobal.LogError(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return ApplicaitonID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT ActiveApplicationID=Applications.ApplicationID
                             FROM   Applications 
                             		LEFT OUTER JOIN
                                    LocalDrivingLicenseApplication ON Applications.ApplicationID = LocalDrivingLicenseApplication.ApplicationID
                             		LEFT OUTER JOIN
                                    ApplicationTypes ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID
                             WHERE
                             		LocalDrivingLicenseApplication.LicenseClassID = @LicenseClassID AND
                             		Applications.PersonID = @PersonID AND
                             		ApplicationTypes.ApplicationTypeID = @ApplicationTypeID AND
                             		Applications.ApplicationStatu = 1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }
            finally
            {
                connection.Close();
            }

            return ActiveApplicationID;
        }

        public static bool DeleteApplication(int ApplicationID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete Applications 
                             where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        //public static DataTable GetAllABaseApplicaits()
        //{
        //    DataTable dt = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //    {
        //        string query = @"select * from LocalDrivingLicenseApplications_View;";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            try
        //            {
        //                connection.Open();

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        dt.Load(reader);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Console.WriteLine("Error: " + ex.Message);
        //            }
        //        }
        //    }

        //    return dt;
        //}

        public static bool GetApplicationInfoByID(int ApplicationID,
        ref int PersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID,
        ref byte ApplicationStatu, ref DateTime LastStatusDate,
        ref float PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatu = (byte)reader["ApplicationStatu"];
                    LastStatusDate = (DateTime)reader["LastStatuDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool UpdateApplication(int ApplicationID, byte ApplicationTypeID,
        byte ApplicationStatu, DateTime LastStatuDate, float PaidFees, int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Applications  
                            set ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatu = @ApplicationStatu,
                                LastStatuDate = @LastStatuDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID
                                where ApplicationID = @ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatu", ApplicationStatu);
            command.Parameters.AddWithValue("@LastStatuDate", LastStatuDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsGlobal.LogError(ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);

        }


        public static bool CancelApplicationByID(int ApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  Applications  
                            set ApplicationStatu = 2
                                where ApplicationID = @ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsGlobal.LogError(ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);

        }

    }
}
