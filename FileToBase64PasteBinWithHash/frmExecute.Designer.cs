namespace FileToBase64PasteBinWithHash
{
    partial class frmExecute
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
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.ofdLoad = new System.Windows.Forms.OpenFileDialog();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.chkOverWrite = new System.Windows.Forms.CheckBox();
            this.chkDescription = new System.Windows.Forms.CheckBox();
            this.btnEditDescription = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.BackColor = System.Drawing.Color.Cyan;
            this.btnBrowseSource.Location = new System.Drawing.Point(12, 41);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSource.TabIndex = 0;
            this.btnBrowseSource.Text = "Browse...";
            this.btnBrowseSource.UseVisualStyleBackColor = false;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // sfdSave
            // 
            this.sfdSave.Filter = "B64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
            // 
            // ofdLoad
            // 
            this.ofdLoad.AddExtension = false;
            this.ofdLoad.Filter = "All Files *.*|*.*";
            this.ofdLoad.ReadOnlyChecked = true;
            // 
            // chkCompress
            // 
            this.chkCompress.Location = new System.Drawing.Point(33, 69);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(345, 36);
            this.chkCompress.TabIndex = 2;
            this.chkCompress.Text = "Compress file before encoding (not recommended for some images, binaries, music f" +
    "iles (such as MP3/WMA/AAC), etc.)";
            this.chkCompress.UseVisualStyleBackColor = true;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 25);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(55, 13);
            this.lblSource.TabIndex = 3;
            this.lblSource.Text = "SOURCE:";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(12, 109);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(83, 13);
            this.lblDestination.TabIndex = 6;
            this.lblDestination.Text = "DESTINATION:";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.BackColor = System.Drawing.Color.Cyan;
            this.btnBrowseDestination.Location = new System.Drawing.Point(12, 125);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDestination.TabIndex = 4;
            this.btnBrowseDestination.Text = "Browse...";
            this.btnBrowseDestination.UseVisualStyleBackColor = false;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // txtSource
            // 
            this.txtSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtSource.Location = new System.Drawing.Point(93, 43);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(442, 20);
            this.txtSource.TabIndex = 7;
            // 
            // txtDestination
            // 
            this.txtDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDestination.Location = new System.Drawing.Point(93, 127);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(442, 20);
            this.txtDestination.TabIndex = 8;
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.Lime;
            this.btnExecute.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(280, 153);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(153, 38);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "EXECUTE";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Red;
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(439, 153);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(96, 38);
            this.btnQuit.TabIndex = 10;
            this.btnQuit.Text = "QUIT";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // chkOverWrite
            // 
            this.chkOverWrite.AutoSize = true;
            this.chkOverWrite.Location = new System.Drawing.Point(33, 166);
            this.chkOverWrite.Name = "chkOverWrite";
            this.chkOverWrite.Size = new System.Drawing.Size(220, 17);
            this.chkOverWrite.TabIndex = 11;
            this.chkOverWrite.Text = "Overwrite destination (silences warnings).";
            this.chkOverWrite.UseVisualStyleBackColor = true;
            this.chkOverWrite.CheckedChanged += new System.EventHandler(this.chkOverWrite_CheckedChanged);
            // 
            // chkDescription
            // 
            this.chkDescription.AutoSize = true;
            this.chkDescription.Location = new System.Drawing.Point(384, 79);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new System.Drawing.Size(117, 17);
            this.chkDescription.TabIndex = 12;
            this.chkDescription.Text = "Include Description";
            this.chkDescription.UseVisualStyleBackColor = true;
            this.chkDescription.CheckedChanged += new System.EventHandler(this.chkDescription_CheckedChanged);
            // 
            // btnEditDescription
            // 
            this.btnEditDescription.Location = new System.Drawing.Point(507, 75);
            this.btnEditDescription.Name = "btnEditDescription";
            this.btnEditDescription.Size = new System.Drawing.Size(28, 23);
            this.btnEditDescription.TabIndex = 13;
            this.btnEditDescription.Text = "...";
            this.btnEditDescription.UseVisualStyleBackColor = true;
            this.btnEditDescription.Visible = false;
            this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
            // 
            // frmExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(547, 203);
            this.Controls.Add(this.btnEditDescription);
            this.Controls.Add(this.chkDescription);
            this.Controls.Add(this.chkOverWrite);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.btnBrowseDestination);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.chkCompress);
            this.Controls.Add(this.btnBrowseSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ENCODE A FILE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExecute_FormClosed);
            this.Load += new System.EventHandler(this.frmExecute_Load);
            this.Shown += new System.EventHandler(this.frmExecute_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.SaveFileDialog sfdSave;
        private System.Windows.Forms.OpenFileDialog ofdLoad;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.CheckBox chkOverWrite;
        private System.Windows.Forms.CheckBox chkDescription;
        private System.Windows.Forms.Button btnEditDescription;
    }
}