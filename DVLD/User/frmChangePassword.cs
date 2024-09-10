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

namespace DVLD.User
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID )
        {
            InitializeComponent();
            clsFormat.StartPositionBottomCenterMainScreen(this);

            _UserID=UserID;
        }

        private void _ResetDefualtValues()
        {
            //txtCurrentPassword.Text = "";
            //txtNewPassword.Text = "";
            //txtConfirmPassword.Text = "";
            //txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            //_ResetDefualtValues();

            txtCurrentPassword.Focus();

            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 this.Close();

                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };

            // Here we compare bettwen HashPassword and Current password (Entered User) convert to Hash Password
            string CurrentHashPassword = clsGlobal.HashPassword(txtCurrentPassword.Text.Trim());

            if (_User.HashPassword != CurrentHashPassword)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            };
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //_User.Password = txtNewPassword.Text;

            string NewHashPassword = clsGlobal.HashPassword(txtNewPassword.Text.Trim());

            if (clsUser.ChangePassword(_UserID, NewHashPassword))
            {
                MessageBox.Show("Password Changed Successfully.",
                   "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information );

                HandleTheFormAfterSaveNewPassword();

                //_ResetDefualtValues();
            }
            else
            {
                MessageBox.Show("An Erro Occured, Password did not change.",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleTheFormAfterSaveNewPassword()
        {
            // we desable all inputs and button save, after change password.

            txtCurrentPassword.Enabled = false;
            txtNewPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            btnSave.Enabled = false;
            this.Text = "The password changed successfully";
            lblTitle.Text = "The password changed successfully";
            lblTitle.ForeColor = Color.Green;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
