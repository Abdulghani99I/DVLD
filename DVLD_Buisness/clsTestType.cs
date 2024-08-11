using DVLD_DataAccess;
using System;
using System.Data;
using System.Xml.Linq;

namespace DVLD_Buisness
{
    public class clsTestType
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { set; get; }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        private clsTestType(int ID, string Title, string Description, decimal Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description; 
            this.Fees = Fees;
        }

        public static clsTestType Find(int ID)
        {
            string Title = string.Empty;
            string Description = string.Empty;
            decimal Fees = 0;

            clsTestTypesData.Find(ID, ref Title, ref Description, ref Fees);

            return new clsTestType(ID, Title, Description, Fees);
        }

        public bool Save()
        {
            return clsTestTypesData.UpdateInfoTest(this.ID, this.Title, this.Description, this.Fees);
        }
    }
}
