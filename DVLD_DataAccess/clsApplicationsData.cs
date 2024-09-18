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

        //public static bool UpdateApplication(int ApplicationID, int ApplicationTypeID,
        //        int PersonID, byte ApplicationStatus, DateTime ApplicationDate, float PaidFees, int CreateByUserID)
        //{
        //    int rowsAffected = 0;
        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        //    string query = @"Update  Applicatinos  
        //                    set ApplicationTypeID = @ApplicationTypeID,
        //                        PersonID = @PersonID,
        //                        ApplicationStatus = @ApplicationStatus
        //                        ApplicationDate = @ApplicationDate
        //                        PaidFees = @PaidFees
        //                        CreateByUserID = @CreateByUserID
        //                        where ApplicationID = @ApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
        //    command.Parameters.AddWithValue("@PersonID", PersonID);
        //    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
        //    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);
        //    command.Parameters.AddWithValue("@CreateByUserID", CreateByUserID);


        //    try
        //    {
        //        connection.Open();
        //        rowsAffected = command.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {
        //        clsGlobal.LogError(ex.Message);
        //        return false;
        //    }

        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return (rowsAffected > 0);

        //}

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
    }
}
