using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NISTRNGBeaconThingy
{
    public partial class frmMain : Form
    {
        volatile bool IsRunning = false;
        volatile bool StopRunningRequested = false;
        BeaconV1.Record LastProcessedRecord = BeaconV1.Record.RetrieveLastFromDiskCache();
        BeaconV1.RecordVerifier TheVerifier = new BeaconV1.RecordVerifier();
        private string LogName = ".\\Cache\\Log-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log.txt";
        private static object LOCK_THIS = new object();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (LastProcessedRecord != null)
            {
                LogThis("[Record # " + LastProcessedRecord.TimeStamp.ToString() + " retrieved from disk cache (highest number cache timestamp)]");
                if (LastProcessedRecord.TimeStamp == BeaconV1.Record.StartOfCurrentChainTimeStamp)
                    LogThis("[START OF CURRENT CHAIN]");
                else if (LastProcessedRecord.StatusCode == 1)
                    LogThis("[START OF NON-CURRENT CHAIN]");
            }
            else
            {
                LogThis("[No cached records found]");
            }
        }
    
        private void LogThis(string message)
        {
            lock (LOCK_THIS)
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    string thingToLog = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f") + " > " + message;
                    txtLog.AppendText(thingToLog + "\r\n");
                    try
                    {
                        if (!System.IO.Directory.Exists(".\\Cache\\"))
                            System.IO.Directory.CreateDirectory(".\\Cache\\");
                        System.IO.StreamWriter sw = System.IO.File.AppendText(LogName);
                        sw.WriteLine(thingToLog);
                        sw.Flush();
                        sw.Close();
                    }
                    catch { }
                }
            }
            if (LastProcessedRecord == null)
            {
                txtLastRecordOutput.Text = "<null>";
                txtLastRecordTimeStamp.Text = "<null>";
                btnVerifyLast.Enabled = false;
            }
            else
            {
                txtLastRecordOutput.Text = LastProcessedRecord.OutputValue;
                txtLastRecordTimeStamp.Text = LastProcessedRecord.TimeStamp.ToString();
                btnVerifyLast.Enabled = true;
            }
            if (chkAutoScrollLog.Checked)
            {
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.SelectionLength = 0;
            }
        }

        private void btnStartOfChain_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            LogThis("[Retrieving start of current chain from NIST...]");
            LastProcessedRecord = BeaconV1.Record.RetrieveStartOfCurrentChain();
            if (LastProcessedRecord == null)
                LogThis("[!Start of current chain returned NULL!]");
            else
                LogThis("[Retrieved record # " + LastProcessedRecord.TimeStamp.ToString() + " - START OF CURRENT CHAIN]");
            this.Enabled = true;
        }

        private void btnVerifyLast_Click(object sender, EventArgs e)
        {
            if (LastProcessedRecord == null)
            {
                btnVerifyLast.Enabled = false;
                return;
            }
            this.Enabled = false;
            VerifyARecord(LastProcessedRecord);
            this.Enabled = true;
        }

        private bool VerifyARecord(BeaconV1.Record theRecord)
        {
            if (theRecord == null)
            {
                LogThis("[*** VERIFICATION: FAILED ***] <null record>");
                return false;
            }
            bool result = TheVerifier.VerifySignature(theRecord);
            if (!result)
            {
                LogThis("[*** VERIFICATION: FAILED ***] <#" + theRecord.TimeStamp.ToString() + ">");
            }
            else
            {
                LogThis("[verification: PASS] <#" + theRecord.TimeStamp.ToString() + ">");
            }
            return result;
        }


        private void btnHighestTimeStamp_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            LogThis("[Retrieving highest numbered cached timestamp record...]");
            LastProcessedRecord = BeaconV1.Record.RetrieveLastFromDiskCache();
            if (LastProcessedRecord != null)
            {
                LogThis("[Record # " + LastProcessedRecord.TimeStamp.ToString() + " retrieved from disk cache.]");
            }
            else
            {
                LogThis("[!No cached records found!]");
            }
            this.Enabled = true;
        }

        private void btnMostRecent_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            LogThis("[Retrieving last cached timestamp record (by modified date)...]");
            LastProcessedRecord = BeaconV1.Record.RetrieveLastModifiedFromDiskCache();
            if (LastProcessedRecord != null)
            {
                LogThis("[Record # " + LastProcessedRecord.TimeStamp.ToString() + " retrieved from disk cache.]");
            }
            else
            {
                LogThis("[!No cached records found!]");
            }
            this.Enabled = true;
        }

        private void btnLastFromNist_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            LogThis("[Retrieving last timestamp record from NIST...]");
            LastProcessedRecord = BeaconV1.Record.RetrieveLast();
            if (LastProcessedRecord != null)
            {
                LogThis("[Record # " + LastProcessedRecord.TimeStamp.ToString() + " retrieved from NIST.]");
            }
            else
            {
                LogThis("[!* RETURN VALUE WAS NULL *!]");
            }
            this.Enabled = true;
        }

        private void btnVerifyAndContinue_Click(object sender, EventArgs e)
        {
            if (IsRunning)
            {
                StopRunningRequested = true;
                return;
            }
            if (LastProcessedRecord == null)
            {
                btnVerifyAndContinue.Enabled = false;
                return;
            }
            this.Enabled = false;
            BeaconV1.Record StartOfVerificationChain = LastProcessedRecord;
            LogThis("[COMMENCING VERIFICATION PROCESS...]");
            if (!VerifyARecord(StartOfVerificationChain))
            {
                LogThis("[!* PROCESS STOPPED *!] <start of verification chain - bad signature>");
                this.Enabled = true;
                return;
            }
            BeaconV1.Record previousRecord = StartOfVerificationChain;
            BeaconV1.Record endOfTheLine = BeaconV1.Record.RetrieveLast();
            if (endOfTheLine == null)
            {
                LogThis("[!* PROCESS STOPPED *!] <'rest/record/last' request from NIST returned null>");
                this.Enabled = true;
                return;
            }
            IsRunning = true;
            string oldButtonText = btnVerifyAndContinue.Text;
            btnVerifyAndContinue.Text = "STOP";
            int count = 0;
            do
            {
                BeaconV1.Record nextRecord = BeaconV1.Record.RetrieveNext(previousRecord.TimeStamp);
                if (nextRecord == null && previousRecord.TimeStamp == endOfTheLine.TimeStamp)
                {
                    IsRunning = false;
                    LogThis("[end of the line encountered, re-run to continue from this point]");
                    break;
                }
                else
                {
                    if (nextRecord == null)
                    {
                        LogThis("[!* PROCESS STOPPED *!] <nextRecord returned null>");
                        break;
                    }
                    if (nextRecord.PreviousOutputValue.CompareTo(previousRecord.OutputValue) != 0)
                    {
                        LogThis("[!* PROCESS STOPPED *!] <chain broken, # " + nextRecord.TimeStamp.ToString() +
                            " 'previousOutputValue' doesn't match # " + previousRecord.TimeStamp.ToString() + " 'outputValue'>");
                        break;
                    }
                    else
                        LogThis("[chain intact] <#" + previousRecord.TimeStamp.ToString() + " outputValue = " + 
                            "#" + nextRecord.TimeStamp.ToString() + " previousOutputValue>");
                    if (nextRecord.StatusCode != 0)
                        LogThis("[!unusual status code: " + nextRecord.StatusCode.ToString() + "!]");
                    if (!VerifyARecord(nextRecord))
                    {
                        LogThis("[!* PROCESS STOPPED *!] <failed signature validation>");
                        break;
                    }
                    previousRecord = nextRecord;
                    LastProcessedRecord = previousRecord;
                    if (++count % 200 == 0)
                    {
                        LogThis("[...pause for cool-down, 3 sec...]");
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            } while (LastProcessedRecord != null && IsRunning == true && !StopRunningRequested);

            if (StopRunningRequested)
            {
                LogThis("[!* PROCESS ABORTED AS REQUESTED BY USER *!]");
            }

            IsRunning = false;
            StopRunningRequested = false;
            btnVerifyAndContinue.Text = oldButtonText;
            this.Enabled = true;
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show(this, "Are you sure you want to clear the cache files?", 
                "CLEAR CACHE FILES?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                this.Enabled = false;
                if (System.IO.Directory.Exists(".\\Cache\\"))
                {
                    foreach(string f in System.IO.Directory.EnumerateFiles(".\\Cache\\", "*.xml"))
                    {   
                        System.IO.File.Delete(f); 
                    }
                    foreach(string f in System.IO.Directory.EnumerateFiles(".\\Cache\\", "*.cer"))
                    {
                        System.IO.File.Delete(f);
                    }
                    LogThis("[!CACHE CLEARED!]");
                }
                this.Enabled = true;
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            if (txtLog.Text.Length > txtLog.MaxLength - 1000)
            {
                txtLog.Text = txtLog.Text.Substring(txtLog.MaxLength / 2);
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show(this, "Are you sure you want to clear the log files?",
                "CLEAR LOG FILES?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ret == DialogResult.Yes)
            {
                this.Enabled = false;
                if (System.IO.Directory.Exists(".\\Cache\\"))
                {
                    foreach (string f in System.IO.Directory.EnumerateFiles(".\\Cache\\", "*.log.txt"))
                    {
                        System.IO.File.Delete(f);
                    }
                    LogThis("[!LOGS CLEARED!]");
                }
                this.Enabled = true;
            }
        }
    
    }
}
