using DVLD.Classes;
using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmAddUpdateLocalDrivingLicense : Form
    {

        private clsLocalDrivingLicense _InfoDrivingLicenseApplication;
        private int _DrivingLicenseApplicationID = -1;

        enum enMode {AddNew, Update};
        enMode Mode = enMode.AddNew;

        enum enApplicationStatus { New=1, Cancel, Complated};
        enApplicationStatus ApplicationStutus = enApplicationStatus.New;

        public frmAddUpdateLocalDrivingLicense()
        {
            InitializeComponent();

            Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicense(int DrivingLicenseApplicationID)
        {
            InitializeComponent();

            Mode = enMode.Update;
            _DrivingLicenseApplicationID  = DrivingLicenseApplicationID;

        }

        private void frmLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            _FillLicenseClassesInComoboBox();

            if (Mode == enMode.AddNew)
            {
                // Select 'Ordinary License Class' by default
                cbLicenseClasses.SelectedIndex = 2;

                lblDrivingLicenseApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();

                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                _LoadInfoApplication();
            }
        }

        private void _LoadInfoApplication()
        {
            _InfoDrivingLicenseApplication = clsLocalDrivingLicense.Find(_DrivingLicenseApplicationID);

            ctrlPersonCardWithFilter1.LoadPersonInfo(_InfoDrivingLicenseApplication.PersonID);
            lblDrivingLicenseApplicationID.Text = _InfoDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _InfoDrivingLicenseApplication.ApplicationDate.ToString("yyyy/MM/dd");
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(_InfoDrivingLicenseApplication.InfoLicenseClass.ClassName);
            lblApplicationFees.Text = _InfoDrivingLicenseApplication.InfoLicenseClass.ClassFees.ToString();
            lblCreatedBy.Text = _InfoDrivingLicenseApplication.CreateByUserID.ToString();
        }

        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtCountries = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (Mode == enMode.AddNew)
                {
                    TabControl.SelectedTab = TabControl.TabPages["tpApplicationInfo"];
                    btnSave.Enabled = true;
                    panelApplicationInfo.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void cbLicenseClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            // here we get Info Application By License Class

            clsLicenseClass LicenseClass = clsLicenseClass.Find(cbLicenseClasses.Text);
            lblApplicationFees.Text = LicenseClass.ClassFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _InfoDrivingLicenseApplication = new clsLocalDrivingLicense();

            clsLicenseClass LicenseClass = clsLicenseClass.Find(cbLicenseClasses.Text);

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter1.PersonID, clsApplicationType.enApplicationTypes.NewLocalDrivingLicenseService, LicenseClass.LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClasses.Focus();
                return;
            }

            _InfoDrivingLicenseApplication.ApplicationType = clsApplicationType.enApplicationTypes.NewLocalDrivingLicenseService;
            _InfoDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _InfoDrivingLicenseApplication.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _InfoDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _InfoDrivingLicenseApplication.LastStausDate = DateTime.Now;
            _InfoDrivingLicenseApplication.PaidFees = LicenseClass.ClassFees;
            _InfoDrivingLicenseApplication.CreateByUserID = clsGlobal.CurrentUser.UserID;
            _InfoDrivingLicenseApplication.LicensesClassID = LicenseClass.LicenseClassID;


            if (_InfoDrivingLicenseApplication.Save())
            {
                lblDrivingLicenseApplicationID.Text = _InfoDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                MessageBox.Show("Data Saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (Mode == enMode.AddNew)
                {
                    Mode = enMode.Update;
                    ctrlPersonCardWithFilter1.FilterEnabled = false;
                    lblTitle.Text = "Update Local Driving License Application";
                }
            }
            else
            {
                MessageBox.Show("Data Don't Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
