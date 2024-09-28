using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Test
{
    public partial class frmListTestAppointments : Form
    {
        private int _LocalDrivingAppID = -1;

        public frmListTestAppointments(int LocalDrivingLicenseApplication)
        {
            _LocalDrivingAppID = LocalDrivingLicenseApplication;
            InitializeComponent();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingAppID);

            _LoadTableTestAppointments();
        }


        private void _LoadTableTestAppointments()
        {
            dgvLicenseTestAppointments.DataSource = clsTest.GetAllTestAppointmentsByLocalDrivingAppID(_LocalDrivingAppID);

            lblRecordsCount.Text = dgvLicenseTestAppointments.RowCount.ToString();
        }
    }
}
