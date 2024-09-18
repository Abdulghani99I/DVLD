using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.DrivingLicensesServices
{
    public partial class frmListLocalDrivingLicense : Form
    {
        private DataTable _dtLicenseApplications;

        public frmListLocalDrivingLicense()
        {
            InitializeComponent();

            clsFormat.StartPositionBottomCenterMainScreen(this);
        }

        private void frmListLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            LoadTable();

            cbFilterBy.SelectedIndex = 0;

            if (_dtLicenseApplications.Rows.Count > 0 )
            {
                dgvLicenseApplications.Columns[0].HeaderText = "L.D.LAppID";
                dgvLicenseApplications.Columns[0].Width = 100;
                
                dgvLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLicenseApplications.Columns[1].Width = 280;
                
                dgvLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLicenseApplications.Columns[2].Width = 140;
                
                dgvLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLicenseApplications.Columns[3].Width = 300;
                
                dgvLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLicenseApplications.Columns[4].Width = 180;

                dgvLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLicenseApplications.Columns[5].Width = 130;

                dgvLicenseApplications.Columns[6].HeaderText = "Status";
                dgvLicenseApplications.Columns[6].Width = 100;
            }
        }

        private void LoadTable()
        {
            _dtLicenseApplications = clsLocalDrivingLicense.GetAllLocalDrivingLicenses();
            dgvLicenseApplications.DataSource = _dtLicenseApplications;
            lblRecordsCount.Text = _dtLicenseApplications.Rows.Count.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtLicenseApplications == null) return;

            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Enabled = false;
                _dtLicenseApplications.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                txtFilterValue.Enabled = true;
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "L.D.LAppID":
                    FilterColumn = "DrivingLicenseApplicationID";
                    break;

                case "Driving Class":
                    FilterColumn = "DrivingClass";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Passed Tests":
                    FilterColumn = "PassedTests";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == string.Empty || FilterColumn == "None")
            {
                _dtLicenseApplications.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = _dtLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "DrivingLicenseApplicationID" || FilterColumn == "PassedTests")
                //in this case we deal with numbers not string.
                _dtLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            
            else
                //in this case we deal with string not number.
                _dtLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
    
            lblRecordsCount.Text = dgvLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "L.D.LAppID" || cbFilterBy.Text == "Passed Tests")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicense frm = new frmAddUpdateLocalDrivingLicense();
            frm.ShowDialog();
            LoadTable();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicense LocalDrivingLicenseApplication =
                clsLocalDrivingLicense.Find(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTable();
                }
                else
                {
                    MessageBox.Show("Don't Deleted Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
