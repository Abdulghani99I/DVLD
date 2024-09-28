using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static DataTable GetAllTestAppointmentsByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT  TestAppointments.AppointmentID, TestAppointments.AppointmentDate, TestAppointments.PaidFees, TestAppointments.IsLocked
                                FROM     Tests INNER JOIN
                                         TestAppointments ON Tests.AppointmentID = TestAppointments.AppointmentID
                                WHERE    (TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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
                        clsGlobal.LogError(ex.Message);
                    }
                }
            }
            return dt;
        }



    }
}
