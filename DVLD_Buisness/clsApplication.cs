using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplication
    {
        
        enum enMode { AddNew, Update}
        
        enMode Mode = enMode.AddNew;

        public enum enApplicationStatus { New=1, Cansel, Complated};
        enApplicationStatus applicationStatus = enApplicationStatus.New;

        public int ApplicaitonID {  get; set; }
        public clsApplicationType.enApplicationTypes ApplicationType { get; set; }
        public int PersonID { get; set; }
        public enApplicationStatus ApplicationStatus {  get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStausDate { get; set; }
        public float PaidFees { get; set; }
        public int CreateByUserID { get; set; }

        public clsPerson PersonInfo { get; set; }



        public clsApplication()
        
        {
            this.ApplicaitonID = -1;
            this.ApplicationType = 0;
            this.PersonID = -1;
            this.ApplicationStatus = 0;
            this.ApplicationDate = DateTime.MinValue;
            this.LastStausDate = DateTime.MinValue;
            this.PaidFees = 0;
            this.CreateByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsApplication(int ApplicaitonID, clsApplicationType.enApplicationTypes ApplicationType, int PersonID, byte ApplicationStatus,
            DateTime ApplicationDate, float PaidFees, int CreateByUserID)

        {
            this.ApplicaitonID = ApplicaitonID;
            this.ApplicationType = ApplicationType;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.ApplicationStatus = 0;
            this.ApplicationDate = ApplicationDate;
            this.PaidFees = PaidFees;
            this.CreateByUserID = CreateByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewApplicatino()
        {
            //call DataAccess Layer 

            this.ApplicaitonID = clsApplicationsData.AddNewApplication((int)ApplicationType, PersonID, ApplicationDate, LastStausDate, PaidFees, CreateByUserID);

            return (this.ApplicaitonID != -1);
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

                //case enMode.Update:
                //    return _UpdateUser();

            }

            return false;
        }

        //private bool _UpdateUser()
        //{
        //    //call DataAccess Layer 

        //    int ApplicationTypeID = -1, PersonID = -1, CreateByUserID = -1;
        //    byte ApplicationStatus = 0;
        //    DateTime ApplicationDate = DateTime.MinValue;
        //    decimal PaidFees = 0;


        //    return clsApplicationsData.UpdateApplication(ApplicationTypeID, PersonID, CreateByUserID,
        //        ApplicationStatus, ApplicationDate, PaidFees);
        //}

        //public static clsUser FindByUserID(int UserID)
        //{
        //    int PersonID = -1;
        //    string UserName = "", Password = "";
        //    bool IsActive = false;

        //    bool IsFound = clsUserData.GetUserInfoByUserID
        //                        (UserID, ref PersonID, ref UserName, ref Password, ref IsActive);

        //    if (IsFound)
        //        //we return new object of that User with the right data
        //        return new clsUser(UserID, PersonID, UserName, Password, IsActive);
        //    else
        //        return null;
        //}

        //public static clsUser FindByPersonID(int PersonID)
        //{
        //    int UserID = -1;
        //    string UserName = "", Password = "";
        //    bool IsActive = false;

        //    bool IsFound = clsUserData.GetUserInfoByPersonID
        //                        (PersonID, ref UserID, ref UserName, ref Password, ref IsActive);

        //    if (IsFound)
        //        //we return new object of that User with the right data
        //        return new clsUser(UserID, UserID, UserName, Password, IsActive);
        //    else
        //        return null;
        //}

    }
}
