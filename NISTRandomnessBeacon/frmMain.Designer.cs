namespace NISTRNGBeaconThingy
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStartOfChain = new System.Windows.Forms.Button();
            this.txtLastRecordOutput = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastRecordTimeStamp = new System.Windows.Forms.TextBox();
            this.btnHighestTimeStamp = new System.Windows.Forms.Button();
            this.btnMostRecent = new System.Windows.Forms.Button();
            this.btnVerifyLast = new System.Windows.Forms.Button();
            this.btnVerifyAndContinue = new System.Windows.Forms.Button();
            this.chkAutoScrollLog = new System.Windows.Forms.CheckBox();
            this.btnLastFromNist = new System.Windows.Forms.Button();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(13, 99);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.MaxLength = 268435456;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(979, 397);
            this.txtLog.TabIndex = 0;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // btnStartOfChain
            // 
            this.btnStartOfChain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnStartOfChain.Location = new System.Drawing.Point(13, 13);
            this.btnStartOfChain.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartOfChain.Name = "btnStartOfChain";
            this.btnStartOfChain.Size = new System.Drawing.Size(150, 58);
            this.btnStartOfChain.TabIndex = 1;
            this.btnStartOfChain.Text = "START OF CURRENT CHAIN";
            this.btnStartOfChain.UseVisualStyleBackColor = false;
            this.btnStartOfChain.Click += new System.EventHandler(this.btnStartOfChain_Click);
            // 
            // txtLastRecordOutput
            // 
            this.txtLastRecordOutput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastRecordOutput.Location = new System.Drawing.Point(13, 529);
            this.txtLastRecordOutput.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastRecordOutput.Name = "txtLastRecordOutput";
            this.txtLastRecordOutput.Size = new System.Drawing.Size(979, 23);
            this.txtLastRecordOutput.TabIndex = 2;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(13, 75);
            this.lblLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(117, 19);
            this.lblLog.TabIndex = 3;
            this.lblLog.Text = "RUNNING LOG:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 506);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(450, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "LAST PROCESSED RECORD\'S OUTPUT VALUE (HEX BYTES):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(565, 506);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "LAST RECORD TIMESTAMP:";
            // 
            // txtLastRecordTimeStamp
            // 
            this.txtLastRecordTimeStamp.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastRecordTimeStamp.Location = new System.Drawing.Point(778, 503);
            this.txtLastRecordTimeStamp.Name = "txtLastRecordTimeStamp";
            this.txtLastRecordTimeStamp.Size = new System.Drawing.Size(214, 23);
            this.txtLastRecordTimeStamp.TabIndex = 6;
            this.txtLastRecordTimeStamp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnHighestTimeStamp
            // 
            this.btnHighestTimeStamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnHighestTimeStamp.Location = new System.Drawing.Point(171, 13);
            this.btnHighestTimeStamp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHighestTimeStamp.Name = "btnHighestTimeStamp";
            this.btnHighestTimeStamp.Size = new System.Drawing.Size(159, 58);
            this.btnHighestTimeStamp.TabIndex = 7;
            this.btnHighestTimeStamp.Text = "HIGHEST CACHED TIMESTAMP";
            this.btnHighestTimeStamp.UseVisualStyleBackColor = false;
            this.btnHighestTimeStamp.Click += new System.EventHandler(this.btnHighestTimeStamp_Click);
            // 
            // btnMostRecent
            // 
            this.btnMostRecent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnMostRecent.Location = new System.Drawing.Point(338, 13);
            this.btnMostRecent.Margin = new System.Windows.Forms.Padding(4);
            this.btnMostRecent.Name = "btnMostRecent";
            this.btnMostRecent.Size = new System.Drawing.Size(158, 58);
            this.btnMostRecent.TabIndex = 8;
            this.btnMostRecent.Text = "MOST RECENTLY MODIFIED CACHE";
            this.btnMostRecent.UseVisualStyleBackColor = false;
            this.btnMostRecent.Click += new System.EventHandler(this.btnMostRecent_Click);
            // 
            // btnVerifyLast
            // 
            this.btnVerifyLast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnVerifyLast.Location = new System.Drawing.Point(639, 13);
            this.btnVerifyLast.Margin = new System.Windows.Forms.Padding(4);
            this.btnVerifyLast.Name = "btnVerifyLast";
            this.btnVerifyLast.Size = new System.Drawing.Size(126, 58);
            this.btnVerifyLast.TabIndex = 9;
            this.btnVerifyLast.Text = "VERIFY LAST RETRIEVED";
            this.btnVerifyLast.UseVisualStyleBackColor = false;
            this.btnVerifyLast.Click += new System.EventHandler(this.btnVerifyLast_Click);
            // 
            // btnVerifyAndContinue
            // 
            this.btnVerifyAndContinue.BackColor = System.Drawing.Color.Yellow;
            this.btnVerifyAndContinue.Location = new System.Drawing.Point(773, 13);
            this.btnVerifyAndContinue.Margin = new System.Windows.Forms.Padding(4);
            this.btnVerifyAndContinue.Name = "btnVerifyAndContinue";
            this.btnVerifyAndContinue.Size = new System.Drawing.Size(219, 58);
            this.btnVerifyAndContinue.TabIndex = 10;
            this.btnVerifyAndContinue.Text = "VERIFY CHAIN FROM LAST RETRIEVED TO THE END";
            this.btnVerifyAndContinue.UseVisualStyleBackColor = false;
            this.btnVerifyAndContinue.Click += new System.EventHandler(this.btnVerifyAndContinue_Click);
            // 
            // chkAutoScrollLog
            // 
            this.chkAutoScrollLog.AutoSize = true;
            this.chkAutoScrollLog.Checked = true;
            this.chkAutoScrollLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoScrollLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoScrollLog.Location = new System.Drawing.Point(137, 77);
            this.chkAutoScrollLog.Name = "chkAutoScrollLog";
            this.chkAutoScrollLog.Size = new System.Drawing.Size(103, 19);
            this.chkAutoScrollLog.TabIndex = 11;
            this.chkAutoScrollLog.Text = "AUTO SCROLL";
            this.chkAutoScrollLog.UseVisualStyleBackColor = true;
            // 
            // btnLastFromNist
            // 
            this.btnLastFromNist.BackColor = System.Drawing.Color.Yellow;
            this.btnLastFromNist.Location = new System.Drawing.Point(504, 13);
            this.btnLastFromNist.Margin = new System.Windows.Forms.Padding(4);
            this.btnLastFromNist.Name = "btnLastFromNist";
            this.btnLastFromNist.Size = new System.Drawing.Size(127, 58);
            this.btnLastFromNist.TabIndex = 12;
            this.btnLastFromNist.Text = "MOST RECENT FROM NIST";
            this.btnLastFromNist.UseVisualStyleBackColor = false;
            this.btnLastFromNist.Click += new System.EventHandler(this.btnLastFromNist_Click);
            // 
            // btnClearCache
            // 
            this.btnClearCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnClearCache.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearCache.Location = new System.Drawing.Point(246, 77);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(129, 19);
            this.btnClearCache.TabIndex = 13;
            this.btnClearCache.Text = "CLEAR CACHE";
            this.btnClearCache.UseVisualStyleBackColor = false;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnClearLogs.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearLogs.Location = new System.Drawing.Point(381, 77);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(129, 19);
            this.btnClearLogs.TabIndex = 14;
            this.btnClearLogs.Text = "CLEAR LOGS";
            this.btnClearLogs.UseVisualStyleBackColor = false;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 576);
            this.Controls.Add(this.btnClearLogs);
            this.Controls.Add(this.btnClearCache);
            this.Controls.Add(this.btnLastFromNist);
            this.Controls.Add(this.chkAutoScrollLog);
            this.Controls.Add(this.btnVerifyAndContinue);
            this.Controls.Add(this.btnVerifyLast);
            this.Controls.Add(this.btnMostRecent);
            this.Controls.Add(this.btnHighestTimeStamp);
            this.Controls.Add(this.txtLastRecordTimeStamp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtLastRecordOutput);
            this.Controls.Add(this.btnStartOfChain);
            this.Controls.Add(this.txtLog);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NIST RNG BEACON THINGY";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnStartOfChain;
        private System.Windows.Forms.TextBox txtLastRecordOutput;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastRecordTimeStamp;
        private System.Windows.Forms.Button btnHighestTimeStamp;
        private System.Windows.Forms.Button btnMostRecent;
        private System.Windows.Forms.Button btnVerifyLast;
        private System.Windows.Forms.Button btnVerifyAndContinue;
        private System.Windows.Forms.CheckBox chkAutoScrollLog;
        private System.Windows.Forms.Button btnLastFromNist;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.Button btnClearLogs;
    }
}

