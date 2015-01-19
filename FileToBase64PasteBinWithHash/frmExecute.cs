using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace FileToBase64PasteBinWithHash
{
    public partial class frmExecute : Form
    {
        private Direction _direction = Direction.Encode;
        private bool _silent = false;
        private string _description = null;

        public enum Direction: byte
        {
            Unspecified = 0,
            Encode,
            Decode
        }

        public frmExecute(Direction dir)
        {
            InitializeComponent();
            _direction = dir;
            if (_direction == Direction.Unspecified)
                throw new InvalidOperationException("A direction must be specified when creating a new frmExecute object");
        }

        private void frmExecute_Load(object sender, EventArgs e)
        {
            string cmdLine = Environment.CommandLine + " ";
            if (cmdLine.Contains(" -c ") || cmdLine.Contains(" -compress "))
            {
                chkCompress.Checked = true;
                chkCompress.Enabled = false;
            }
            if (cmdLine.Contains(" -s ") || cmdLine.Contains(" -silent "))
            {
                _silent = true;
                chkOverWrite.Checked = true;
                chkOverWrite.Enabled = false;
            }
            if (cmdLine.Contains(" -o ") || cmdLine.Contains(" -overwrite "))
            {
                chkOverWrite.Checked = true;
                chkOverWrite.Enabled = false;
            }
            if (_direction == Direction.Encode)
            {
                this.Text = "ENCODE A FILE";
                chkCompress.Visible = true;
                chkDescription.Visible = true;
            }
            else if (_direction == Direction.Decode)
            {
                this.Text = "DECODE A FILE";
                chkCompress.Visible = false;
                chkDescription.Visible = false;
            }
            
        }

        private void frmExecute_Shown(object sender, EventArgs e)
        {
            Exception ex = null;
            string cmdLine = Environment.CommandLine + " ";
            if (cmdLine.Contains(" -source "))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -source \"") + 10);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    if (File.Exists(sb.ToString()))
                    {
                        txtSource.Text = sb.ToString();
                        if (_direction == Direction.Encode)
                            txtDestination.Text = Path.GetFullPath(txtSource.Text) + ".b64.txt";
                        else
                            txtDestination.Text = Path.GetFullPath(txtSource.Text).Replace(Path.GetFileName(txtSource.Text), "")
                                + Common.ExtractOriginalFileName(txtSource.Text);
                    }
                    else
                        System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: source"), out ex);
                }
            }
            if (cmdLine.Contains(" -destination "))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -destination \"") + 15);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    txtDestination.Text = sb.ToString();
                    chkOverWrite_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: destination"), out ex);
                }
            }
            if (cmdLine.Contains(" -description \""))
            {
                string stuff = cmdLine.Substring(cmdLine.IndexOf(" -description \"") + 15);
                StringBuilder sb = new StringBuilder();
                int infinite = 0;
                do
                {
                    if (stuff[infinite] != '\"')
                    {
                        sb.Append(stuff[infinite]);
                        //stuff.Remove(0, 1);
                        infinite++;
                    }
                    else
                        break;
                } while (stuff.Length > 0 && infinite < 1024);
                if (infinite < 1024)
                {
                    _description = sb.ToString();
                    _description = _description.Replace("<", "").Replace(">", "");
                    _description = _description.Substring(0, 255);
                    chkDescription.Checked = true;
                    chkDescription_CheckedChanged(this, new EventArgs());
                }
                else
                {
                    Common.LogException(new InvalidOperationException("Infinite loop: description"), out ex);
                }
            }
            if (cmdLine.Contains(" -auto ") || cmdLine.Contains(" -autoexit "))
            {
                btnExecute_Click(this, new EventArgs());
            }
            if (cmdLine.Contains(" -autoexit "))
            {
                Application.Exit();
            }
        }

        void frmExecute_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSource.Text) || txtSource.Text.Contains("::INVALID::"))
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            if (File.Exists(txtDestination.Text) && !chkOverWrite.Checked && !_silent)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }
            this.Enabled = false;
            Exception exceptionArg = null;
            bool success = false;
            if (_direction == Direction.Encode)
            {
                string tempDesc = (chkDescription.Checked == true) ? _description : null;
                success = Common.WriteEncodedFile(txtSource.Text, txtDestination.Text, chkCompress.Checked, tempDesc, out exceptionArg);
            }
            else
            {
                success = Common.WriteDecodedFile(txtSource.Text, txtDestination.Text, out exceptionArg);
            }
            if (!success)
            {
                if (!_silent)
                    MessageBox.Show(this, "Oops!  Something went wrong: " + exceptionArg.Message,
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = Common.LogException(exceptionArg, out exceptionArg);
                if (!success)
                    MessageBox.Show(this, "Double oops!  Something went wrong trying to even log the error: " + exceptionArg.Message,
                        "ERROR - SILENT FLAG IGNORED", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                if (!_silent)
                    MessageBox.Show("Done.");
            this.Enabled = true;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            lblSource.Text = "SOURCE:";
            if (!string.IsNullOrWhiteSpace(txtSource.Text))
            {
                ofdLoad.FileName = Path.GetFileName(txtSource.Text);
                ofdLoad.InitialDirectory = Path.GetFullPath(txtSource.Text);
            }
            if (_direction == Direction.Encode)
            {
                ofdLoad.Filter = "All Files *.*|*.*";
                sfdSave.Filter = "Base64 Encoded Files *.b64.txt|*.b64.txt|XML Files *.xml|*.xml|All Files *.*|*.*";
            }
            else
            {
                ofdLoad.Filter = "Base64 Encoded Files *.b64.txt|*.b64.txt|XML Files *.xml|*.xml|All Files *.*|*.*";
                sfdSave.Filter = "All Files *.*|*.*";
            }
            DialogResult ret = ofdLoad.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                txtSource.Text = ofdLoad.FileName;
                if (_direction == Direction.Encode)
                {//ENCODING==================================================================
                    if (string.IsNullOrWhiteSpace(txtDestination.Text))
                    {
                        txtDestination.Text = txtSource.Text + ".b64.txt";
                        if (File.Exists(txtDestination.Text))
                        {
                            if (!_silent && !chkOverWrite.Checked)
                                MessageBox.Show(this, "Caution:  The destination filename that was auto-populated already exists.\r\n\r\n" +
                                    "If this file should be overwritten, please check the box to overwrite.",
                                    "CAUTION: FILE ALREADY EXISTS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            if (!_silent && !chkOverWrite.Checked)
                                lblDestination.Text = "DESTINATION:  CAUTION - FILE ALREADY EXISTS";
                        }
                    }
                }
                else
                {//DECODING==================================================================
                    string origFilename = Common.ExtractOriginalFileName(txtSource.Text);
                    if (string.IsNullOrWhiteSpace(origFilename))
                        txtDestination.Text = Path.GetFullPath(txtSource.Text) +
                            Path.GetFileName(txtSource.Text).ToLower().Replace(".b64.txt", "");
                    else
                        txtDestination.Text = Path.GetFullPath(txtSource.Text).Replace(Path.GetFileName(txtSource.Text), "")
                            + Common.ExtractOriginalFileName(txtSource.Text);
                }
            }
        }

        private void LabelSourceAsInvalid()
        {
            lblSource.Text = "SOURCE IS NOT A VALID DOCUMENT FOR THIS PROGRAM TO DECODE!";
            txtSource.Text = "::INVALID::" + txtSource.Text;
            txtDestination.Clear();
        }

        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                sfdSave.FileName = Path.GetFileName(txtDestination.Text);
                sfdSave.InitialDirectory = Path.GetFullPath(txtDestination.Text);
            }
            lblDestination.Text = "DESTINATION:";
            if (_direction == Direction.Encode)
            {
                ofdLoad.Filter = "All Files *.*|*.*";
                sfdSave.Filter = "Base64 Encoded Files *.b64.txt|*.b64.txt|XML Files *.xml|*.xml|All Files *.*|*.*";
            }
            else
            {
                ofdLoad.Filter = "Base64 Encoded Files *.b64.txt|*.b64.txt|XML Files *.xml|*.xml|All Files *.*|*.*";
                sfdSave.Filter = "All Files *.*|*.*";
            }
            DialogResult ret = sfdSave.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                txtDestination.Text = sfdSave.FileName;
                if (!chkOverWrite.Checked && !_silent && File.Exists(txtDestination.Text))
                {
                    lblDestination.Text = "DESTINATION:  CAUTION - FILE ALREADY EXISTS!";
                }
            }
        }

        private void chkOverWrite_CheckedChanged(object sender, EventArgs e)
        {
            lblDestination.Text = "DESTINATION:";
            if (!chkOverWrite.Checked && File.Exists(txtDestination.Text))
            {
                MessageBox.Show(this, "Caution:  The destination filename already exists.\r\n\r\n" +
                    "If this file should be overwritten, please check the box to overwrite.",
                    "CAUTION: FILE ALREADY EXISTS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lblDestination.Text = "DESTINATION:  CAUTION - FILE ALREADY EXISTS!";
            }
        }

        private void chkDescription_CheckedChanged(object sender, EventArgs e)
        {
            btnEditDescription.Visible = chkDescription.Checked;
        }

        private void btnEditDescription_Click(object sender, EventArgs e)
        {
            frmFileDescription fileDesc = new frmFileDescription(_description);
            DialogResult ret = fileDesc.ShowDialog(this);
            if (ret == DialogResult.OK)
            {
                _description = fileDesc.Tag as string;
            }
            fileDesc.Dispose();
        }
    }
}
