using DVLD.Applications;
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

namespace DVLD.frmApplication
{
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();

            clsFormat.StartPositionBottomCenterMainScreen(this);
        }

        private void ListTestTypes_Load(object sender, EventArgs e)
        {
            DataTable dt = clsTestType.GetAllTestTypes();

            if (dt.Rows.Count > 0) 
            {
                dgvTestTypes.DataSource = dt;

                dgvTestTypes.Columns[0].Width = 120;
                dgvTestTypes.Columns[1].Width = 250;
                dgvTestTypes.Columns[2].Width = 500;
                dgvTestTypes.Columns[3].Width = 120;
            }

            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void edetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType frm = new frmUpdateTestType((int) dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            ListTestTypes_Load(null, null);
        }
    }
}
