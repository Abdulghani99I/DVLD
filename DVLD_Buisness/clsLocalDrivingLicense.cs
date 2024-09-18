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
    public class clsLocalDrivingLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public static DataTable GetAllLocalDrivingLicenses()
        {
            return clsLocalDrivingLicenseData.GetAllLocalDrivingLicenses();
        }

        public int LocalDrivingLicenseApplicationID { set; get; }

        public int LicensesClassID { set; get; }

        public clsLicenseClass InfoLicenseClass { set; get; }

        public clsLocalDrivingLicense()
        {
            this.LocalDrivingLicenseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicense
            (int DrivingLicenseApplicationID, int LicenseClassID, int ApplicaitonID,
             enApplicationTypes ApplicationType, int PersonID, enApplicationStatus ApplicationStatus,
             DateTime ApplicationDate, DateTime LastStausDate, float PaidFees, int CreateByUserID)
        {

            this.LocalDrivingLicenseApplicationID = DrivingLicenseApplicationID;
            this.LicensesClassID = LicenseClassID;
            this.InfoLicenseClass = clsLicenseClass.Find(LicenseClassID);

            this.ApplicationID = ApplicaitonID;
            this.ApplicationType = ApplicationType;
            this.PersonID = PersonID;
            this.ApplicationStatus = ApplicationStatus;
            this.ApplicationDate = ApplicationDate;
            this.LastStausDate = LastStausDate;
            this.PaidFees = PaidFees;
            this.CreateByUserID = CreateByUserID;

            Mode = enMode.Update;
        }

        public static clsLocalDrivingLicense Find(int DrivingLicenseApplicationID)
        {
            int _LicenseClassID = -1,  _ApplicaitonID = -1, _PersonID = -1, _CreateByUserID = -1;

            byte _ApplicationStatus = 0, _ApplicationType = 0;

            DateTime _ApplicationDate = DateTime.MinValue, _LastStausDate = DateTime.MinValue;

            float _PaidFees = 0;

            bool IsFound = clsLocalDrivingLicenseData.GetInfoByDrivingLicenseApplicationID
                                (DrivingLicenseApplicationID, ref _LicenseClassID, ref _ApplicaitonID, ref _ApplicationType,
                                  ref _PersonID, ref _ApplicationStatus, ref _ApplicationDate,
                                  ref _LastStausDate, ref _PaidFees, ref _CreateByUserID );

            //we return new object of that LocalDrivingLicense with the right data
            if (IsFound)
                return new clsLocalDrivingLicense(DrivingLicenseApplicationID, _LicenseClassID, _ApplicaitonID, (enApplicationTypes)_ApplicationType,
                                  _PersonID, (enApplicationStatus)_ApplicationStatus, _ApplicationDate,
                                  _LastStausDate, _PaidFees, _CreateByUserID);
            else
                return null;
        }

        public bool Save()
        {
            // save info Application
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDrvingLicenseApplication())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                //case enMode.Update:

                    //return _UpdateUser();
            }

            return false;
        }

        private bool _AddNewDrvingLicenseApplication()
        {
            //call DataAccess Layer 
           

            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseData.AddNewDrvingLicenseApplication(this.ApplicationID, LicensesClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
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
    }
}
