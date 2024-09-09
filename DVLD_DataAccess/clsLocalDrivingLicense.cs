﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingLicenseData
    {
        public static DataTable GetAllLocalDrivingLicenses()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"select * from LocalDrivingLicenseApplications_View;";

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


        public static int AddNewDrvingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int DrivingApplicationID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO LocalDrivingLicenseApplication (ApplicationID, LicenseClassID)
                                    VALUES (@ApplicationID, @LicenseClassID);
                                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DrivingApplicationID = insertedID;
                        }
                    }

                    catch (Exception ex)
                    {
                        clsGlobal.LogError(ex.Message);
                    }
                }
            }

            return DrivingApplicationID;
        }


        public static bool GetInfoByDrivingLicenseApplicationID
            (int DrivingLicenseApplicationID, ref int LicenseClassID, ref int ApplicaitonID, ref byte ApplicationTypeID,
            ref int PersonID, ref byte ApplicationStatu, ref DateTime ApplicationDate,
            ref DateTime LastStausDate, ref float PaidFees, ref int CreateByUserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT LocalDrivingLicenseApplication.LicenseClassID, 
			                    Applications.ApplicationID, Applications.ApplicationTypeID,
			                    Applications.PersonID, Applications.ApplicationStatu,
			                    Applications.ApplicationDate, Applications.LastStatuDate,
			                    Applications.PaidFees, Applications.CreatedByUserID

                                FROM LocalDrivingLicenseApplication INNER JOIN
                                Applications ON LocalDrivingLicenseApplication.ApplicationID = Applications.ApplicationID
                                where LocalDrivingLicenseApplication.DrivingLicenseApplicationID = @DrivingLicenseApplicationID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrivingLicenseApplicationID", DrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                LicenseClassID = (int) reader["LicenseClassID"];
                                ApplicaitonID = (int) reader["ApplicationID"];
                                ApplicationTypeID = byte.Parse(reader["ApplicationTypeID"].ToString());
                                PersonID = (int) reader["PersonID"];
                                ApplicationStatu = byte.Parse(reader["ApplicationStatus"].ToString());
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                LastStausDate = (DateTime)reader["LastStausDate"];
                                PaidFees = float.Parse(reader["PaidFees"].ToString());
                                CreateByUserID = (int)reader["CreatedByUserID"];
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
            }
            return isFound;
        }

        //public static bool isRequestExistPrevious(int LicenseClassID, int PersonID, int StatusIApplicationD)
        //{
        //    int UserID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        //    string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
        //                     VALUES (@PersonID, @UserName,@Password,@IsActive);
        //                     SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@PersonID", PersonID);
        //    command.Parameters.AddWithValue("@UserName", UserName);
        //    command.Parameters.AddWithValue("@Password", Password);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);

        //    try
        //    {
        //        connection.Open();

        //        object result = command.ExecuteScalar();

        //        if (result != null && int.TryParse(result.ToString(), out int insertedID))
        //        {
        //            UserID = insertedID;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        clsGlobal.LogError(ex.Message);

        //    }

        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return UserID;

        //}


        public static bool Delete(int DrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @" DECLARE @ApplicationID INT;
                              SELECT @ApplicationID = ApplicationID                 
                              FROM LocalDrivingLicenseApplication
                              WHERE DrivingLicenseApplicationID = @DrivingLicenseApplicationID;
                              DELETE FROM LocalDrivingLicenseApplication
                              DELETE FROM Applications
                              WHERE ApplicationID = @ApplicationID;
                              ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DrivingLicenseApplicationID", DrivingLicenseApplicationID);

                try
                {
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
            }

            // Because rowsAffected with two tables (Should delete two record)
            return (rowsAffected > 1);
        }
    }
}
