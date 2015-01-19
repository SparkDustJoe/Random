using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileToBase64PasteBinWithHash
{
    public partial class frmDecide : Form
    {
        public frmDecide()
        {
            InitializeComponent();
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            frmExecute ex = new frmExecute(frmExecute.Direction.Encode);
            ex.Show();
            this.Hide();
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            frmExecute ex = new frmExecute(frmExecute.Direction.Decode);
            ex.Show();
            this.Hide();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
