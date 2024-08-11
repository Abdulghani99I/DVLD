using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classes;
using DVLD_Buisness;

namespace DVLD.ApplicationTypes
{
    public partial class frmListApplicationTypes : Form
    {
        public frmListApplicationTypes()
        {
            InitializeComponent();

            clsFormat.StartPositionBottomCenterMainScreen(this);
        }
        
        private void ApplicationTypes_Load(object sender, EventArgs e)
        {
            DataTable dtApplicationTypes = clsApplicationType.GetAllApplicationTypes();

            if (dtApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.DataSource = dtApplicationTypes;

                dgvApplicationTypes.Columns[0].Width = 130;
                dgvApplicationTypes.Columns[1].Width = 400;
                dgvApplicationTypes.Columns[2].Width = 130;

                lblRecordsCount.Text = dtApplicationTypes.Rows.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
