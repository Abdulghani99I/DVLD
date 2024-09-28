using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using static DVLD_Buisness.clsApplicationType;


namespace DVLD_Buisness
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public static DataTable GetAllLocalDrivingLicenses()
        {
            return clsLocalDrivingLicenseData.GetAllLocalDrivingLicenses();
        }

        public int LocalDrivingLicenseApplicationID { set; get; }

        public byte LicensesClassID { set; get; }

        public clsLicenseClass LicenseClassInfo { set; get; }

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;

            // Make clsApplication And clsLocalDrivingLicenseApplication Mode Equel Add New.
            Mode = enMode.AddNew;
            base.Mode = clsApplication.enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int PersonID,
                                                    DateTime ApplicationDate, int ApplicationTypeID,
                                                     byte ApplicationStatus, DateTime LastStatusDate,
                                                     float PaidFees, int CreatedByUserID, int LicenseClassID)

        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID; ;
            this.ApplicationID = ApplicationID;
            this.PersonID = PersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (enApplicationTypes)ApplicationTypeID;
            this.ApplicationStatus = (enApplicationStatus)ApplicationStatus;
            this.LastStausDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreateByUserID = CreatedByUserID;
            this.LicensesClassID = (byte)LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find((byte)LicenseClassID);
            Mode = enMode.Update;
            base.Mode = clsApplication.enMode.Update;
        }

        public static clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);


            if (IsFound)
            {
                //now we find the base application
                clsApplication Application = FindBaseApplication(ApplicationID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplication(
                    LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.PersonID, Application.ApplicationDate, (byte)Application.ApplicationTypeID,
                    (byte)Application.ApplicationStatus, Application.LastStausDate,
                    Application.PaidFees, Application.CreateByUserID, LicenseClassID);
            }
            else
                return null;
        }

        public bool Save()
        {
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    // save info Application AND Info DrvingLicenseApplication.
                    if (_AddNewDrvingLicenseApplication())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                // in this case we just edit on Application.
                case enMode.Update:
                    if (_UpdateDrvingLicenseApplication())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }

            return false;
        }

        private bool _AddNewDrvingLicenseApplication()
        {
            //call DataAccess Layer 
           

            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseData.AddNewDrvingLicenseApplication(this.ApplicationID, LicensesClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateDrvingLicenseApplication()
        {
            return clsLocalDrivingLicenseData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.LicensesClassID);
        }

        public bool Delete()
        {
            bool isLocalDrivingLicenseApplicationDeleted = clsLocalDrivingLicenseData.Delete(LocalDrivingLicenseApplicationID);

            bool isbaseApplicaionDeleted = base.Delete();

            if (isLocalDrivingLicenseApplicationDeleted & isbaseApplicaionDeleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CancelApplicationByID(int ApplicationID)
        {
            return clsApplicationsData.CancelApplicationByID(ApplicationID);
        }
    }
}
