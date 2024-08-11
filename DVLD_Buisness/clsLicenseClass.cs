using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsLicenseClass
    {
        public int LicenseClassID { set; get; }

        public string ClassName { set; get; }

        public string ClassDescription { set; get; }

        public byte MinimumAllowAge { set; get; }

        public byte ValidityLength {  set; get; }

        public float ClassFees { set; get; }


        public clsLicenseClass ()
        {
            this.LicenseClassID = -1;
            this.ClassName = string.Empty;
            this.ClassDescription = string.Empty;
            this.MinimumAllowAge = byte.MinValue;
            this.ValidityLength = byte.MinValue;
            this.ClassFees = float.MinValue;
        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowAge, byte ValidityLength, float ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowAge = MinimumAllowAge;
            this.ValidityLength = ValidityLength;
            this.ClassFees = ClassFees;
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1;
            string Description = string.Empty;
            byte MinimumAllowAge = byte.MinValue;
            byte ValidityLength = byte.MinValue;
            float ClassFees = float.MinValue;

            bool isFound = clsLicenseClassData.GetLicenseClassByClassName(ClassName, ref LicenseClassID, ref Description, ref MinimumAllowAge, ref ValidityLength, ref ClassFees);

            if (isFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName, Description, MinimumAllowAge, ValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = string.Empty;
            string Description = string.Empty;
            byte MinimumAllowAge = byte.MinValue;
            byte ValidityLength = byte.MinValue;
            float ClassFees = float.MinValue;

            bool isFound = clsLicenseClassData.GetLicenseClassByLicenseClassID(LicenseClassID, ref ClassName, ref Description, ref MinimumAllowAge, ref ValidityLength, ref ClassFees);

            if (isFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName, Description, MinimumAllowAge, ValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }

    }
}
