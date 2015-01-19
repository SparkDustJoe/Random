using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileToBase64PasteBinWithHash
{
    public partial class frmFileDescription : Form
    {
        public frmFileDescription(string existingDescription)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(existingDescription))
                txtDescription.Text = existingDescription;
            else
                txtDescription.Text = "";
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            txtDescription.Text = txtDescription.Text.Replace(">", "").Replace("<", "").Replace("\"", "");
        }

        private void frmFileDescription_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Tag = txtDescription.Text;
            this.Close();
        }

    }
}
