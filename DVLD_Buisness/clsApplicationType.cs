using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsApplicationType
    {
        public enum enApplicationTypes 
        { NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService,
          ReplacementForALostDrivingLicense, ReplacementForADamagedDrivingLicense,
          ReleaseDetainedDrivingLicense, NewIntemationalDrivingLicsense, RetakeTest }

        public int ApplicationTypeID { get; set; }

        public string Title { get; set; }

        public decimal Fees { get; set; }



        public clsApplicationType()
        {
            this.ApplicationTypeID = 0;
            this.Title = "";
            this.Fees = 0;
        }

        private clsApplicationType(int applicationTypeID, string title, decimal fees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.Title = title;
            this.Fees = fees;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }

        public static clsApplicationType Find(int ApplicationsTypeID)
        {
            string Title = "";
            decimal Fees = 0;


            clsApplicationTypesData.FindByID(ApplicationsTypeID, ref Title , ref Fees);

            return new clsApplicationType(ApplicationsTypeID, Title, Fees);
        }
        
        //public static clsApplicationTypes Find(string Title)
        //{
        //    int ApplicationsTypeID = -1;
        //    decimal Fees = 0;

        //    clsApplicationTypesData.(ref ApplicationsTypeID, ref Title, ref Fees);

        //    return new clsApplicationTypes(ApplicationsTypeID, Title, Fees);
        //}



    }
}
