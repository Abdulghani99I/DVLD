using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Buisness.clsApplicationType;

namespace DVLD_DataAccess
{
    public class clsApplication
    {
        public enum enMode { AddNew, Update}
        
        public enMode Mode = enMode.AddNew;

        public enum enApplicationStatus { New=1, Cansel, Complated};
        enApplicationStatus applicationStatus = enApplicationStatus.New;

        public int ApplicationID {  get; set; }
        public clsApplicationType.enApplicationTypes ApplicationTypeID { get; set; }
        public int PersonID { get; set; }
        public enApplicationStatus ApplicationStatus {  get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStausDate { get; set; }
        public float PaidFees { get; set; }
        public int CreateByUserID { get; set; }

        public clsPerson PersonInfo { get; set; }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicationTypeID = 0;
            this.PersonID = -1;
            this.ApplicationStatus = 0;
            this.ApplicationDate = DateTime.MinValue;
            this.LastStausDate = DateTime.MinValue;
            this.PaidFees = 0;
            this.CreateByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsApplication(int ApplicationID, int PersonID,
            DateTime ApplicationDate, enApplicationTypes ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)

        {
            this.ApplicationID = ApplicationID;
            this.ApplicationTypeID = ApplicationTypeID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(this.PersonID);
            this.ApplicationStatus = ApplicationStatus;
            this.ApplicationDate = ApplicationDate;
            this.LastStausDate = LastStatusDate;
            this.CreateByUserID = CreatedByUserID;
            this.PaidFees = PaidFees;

            Mode = enMode.Update;
        }

        private bool _AddNewApplicatino()
        {
            //call DataAccess Layer 

            this.ApplicationID = clsApplicationsData.AddNewApplication((int)ApplicationTypeID, PersonID, ApplicationDate, LastStausDate, PaidFees, CreateByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication() 
        {
            return clsApplicationsData.UpdateApplication(this.ApplicationID, (byte)this.ApplicationTypeID, (byte)this.ApplicationStatus, this.ApplicationDate, this.PaidFees, this.CreateByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicatino())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplicationType.enApplicationTypes ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationsData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public bool Delete()
        {
            return clsApplicationsData.DeleteApplication(this.ApplicationID);
        }


        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = DateTime.Now; int ApplicationTypeID = -1;
            byte ApplicationStatus = 1; DateTime LastStatusDate = DateTime.Now;
            float PaidFees = 0; int CreatedByUserID = -1;

            bool IsFound = clsApplicationsData.GetApplicationInfoByID(
                    ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
                    ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID);

            if (IsFound)
                //we return new object of that person with the right data
                return new clsApplication(ApplicationID, ApplicantPersonID,
                                     ApplicationDate, (enApplicationTypes)ApplicationTypeID,
                                    (enApplicationStatus)ApplicationStatus, LastStatusDate,
                                     PaidFees, CreatedByUserID);
            else
                return null;
        }

    }
}
