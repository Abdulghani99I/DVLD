using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Windows.Forms;
using DVLD.Classes;
using DVLD_Buisness;

namespace DVLD.Applications
{
    public partial class frmUpdateTestType : Form
    {
        private int _ID = -1;
        private clsTestType _InfoTest;
        public frmUpdateTestType(int ID)
        {
            InitializeComponent();

            clsFormat.StartPositionBottomCenterMainScreen(this);

            _ID = ID;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _InfoTest = clsTestType.Find(_ID);

            lbID.Text = _ID.ToString();
            txtTitle.Text = _InfoTest.Title;
            txtDescription.Text = _InfoTest.Description;
            txtFees.Text = _InfoTest.Fees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _InfoTest.Title = txtTitle.Text.Trim();
            _InfoTest.Description = txtDescription.Text.Trim();
            _InfoTest.Fees = Convert.ToDecimal(txtFees.Text.Trim());

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Complate All Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_InfoTest.Save())
            {
                MessageBox.Show("Info Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

                HandleFormAfterSave();
            }
            else
            {
                MessageBox.Show("Failed to update information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleFormAfterSave()
        {
            this.Text = "Info Updated Successfully";
            lblTitle.Text = "Info Updated Successfully";
            txtTitle.Enabled = false;
            txtDescription.Enabled = false;
            txtFees.Enabled = false;
        }


        private void frmUpdateTestType_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);

            if (string.IsNullOrEmpty(textBox.Text))
            {
                errorProvider1.SetError(textBox, "This Field Required");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }
    }
}
