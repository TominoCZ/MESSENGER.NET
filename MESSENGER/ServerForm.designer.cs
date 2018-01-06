namespace MESSENGER
{
    partial class ServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.lbOnline = new System.Windows.Forms.ListBox();
            this.btnKick = new System.Windows.Forms.Button();
            this.btnKickCM = new System.Windows.Forms.Button();
            this.tbKickMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnWriteLog = new System.Windows.Forms.Button();
            this.btnKickAllCM = new System.Windows.Forms.Button();
            this.btnKickAll = new System.Windows.Forms.Button();
            this.btnRemoveAcc = new System.Windows.Forms.Button();
            this.btnBanAcc = new System.Windows.Forms.Button();
            this.btnUnbanAcc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbOnline
            // 
            this.lbOnline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbOnline.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbOnline.FormattingEnabled = true;
            this.lbOnline.IntegralHeight = false;
            this.lbOnline.ItemHeight = 50;
            this.lbOnline.Location = new System.Drawing.Point(13, 13);
            this.lbOnline.Margin = new System.Windows.Forms.Padding(4);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.Size = new System.Drawing.Size(189, 355);
            this.lbOnline.TabIndex = 0;
            this.lbOnline.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbOnline_DrawItem);
            this.lbOnline.SelectedIndexChanged += new System.EventHandler(this.lbOnline_SelectedIndexChanged);
            // 
            // btnKick
            // 
            this.btnKick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKick.Enabled = false;
            this.btnKick.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKick.Location = new System.Drawing.Point(210, 13);
            this.btnKick.Margin = new System.Windows.Forms.Padding(4);
            this.btnKick.Name = "btnKick";
            this.btnKick.Size = new System.Drawing.Size(156, 26);
            this.btnKick.TabIndex = 1;
            this.btnKick.Text = "KICK";
            this.btnKick.UseVisualStyleBackColor = true;
            this.btnKick.Click += new System.EventHandler(this.btnKick_Click);
            // 
            // btnKickCM
            // 
            this.btnKickCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKickCM.Enabled = false;
            this.btnKickCM.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKickCM.Location = new System.Drawing.Point(210, 81);
            this.btnKickCM.Margin = new System.Windows.Forms.Padding(4);
            this.btnKickCM.Name = "btnKickCM";
            this.btnKickCM.Size = new System.Drawing.Size(156, 26);
            this.btnKickCM.TabIndex = 6;
            this.btnKickCM.Text = "KICK/MSG";
            this.btnKickCM.UseVisualStyleBackColor = true;
            this.btnKickCM.Click += new System.EventHandler(this.btnKickCM_Click);
            // 
            // tbKickMessage
            // 
            this.tbKickMessage.Location = new System.Drawing.Point(210, 167);
            this.tbKickMessage.Name = "tbKickMessage";
            this.tbKickMessage.Size = new System.Drawing.Size(156, 27);
            this.tbKickMessage.TabIndex = 7;
            this.tbKickMessage.TextChanged += new System.EventHandler(this.tbKickMessage_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 145);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Kick message:";
            // 
            // btnWriteLog
            // 
            this.btnWriteLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWriteLog.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWriteLog.Location = new System.Drawing.Point(210, 331);
            this.btnWriteLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteLog.Name = "btnWriteLog";
            this.btnWriteLog.Size = new System.Drawing.Size(156, 37);
            this.btnWriteLog.TabIndex = 8;
            this.btnWriteLog.Text = "WRITE LOG";
            this.btnWriteLog.UseVisualStyleBackColor = true;
            this.btnWriteLog.Click += new System.EventHandler(this.btnWriteLog_Click);
            // 
            // btnKickAllCM
            // 
            this.btnKickAllCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKickAllCM.Enabled = false;
            this.btnKickAllCM.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKickAllCM.Location = new System.Drawing.Point(210, 115);
            this.btnKickAllCM.Margin = new System.Windows.Forms.Padding(4);
            this.btnKickAllCM.Name = "btnKickAllCM";
            this.btnKickAllCM.Size = new System.Drawing.Size(156, 26);
            this.btnKickAllCM.TabIndex = 9;
            this.btnKickAllCM.Text = "KICK ALL/MSG";
            this.btnKickAllCM.UseVisualStyleBackColor = true;
            this.btnKickAllCM.Click += new System.EventHandler(this.btnKickAllCM_Click);
            // 
            // btnKickAll
            // 
            this.btnKickAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKickAll.Enabled = false;
            this.btnKickAll.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKickAll.Location = new System.Drawing.Point(210, 47);
            this.btnKickAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnKickAll.Name = "btnKickAll";
            this.btnKickAll.Size = new System.Drawing.Size(156, 26);
            this.btnKickAll.TabIndex = 1;
            this.btnKickAll.Text = "KICK ALL";
            this.btnKickAll.UseVisualStyleBackColor = true;
            this.btnKickAll.Click += new System.EventHandler(this.btnKickAll_Click);
            // 
            // btnRemoveAcc
            // 
            this.btnRemoveAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveAcc.Enabled = false;
            this.btnRemoveAcc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveAcc.Location = new System.Drawing.Point(210, 285);
            this.btnRemoveAcc.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveAcc.Name = "btnRemoveAcc";
            this.btnRemoveAcc.Size = new System.Drawing.Size(156, 26);
            this.btnRemoveAcc.TabIndex = 10;
            this.btnRemoveAcc.Text = "UNREGISTER";
            this.btnRemoveAcc.UseVisualStyleBackColor = true;
            this.btnRemoveAcc.Click += new System.EventHandler(this.btnRemoveAcc_Click);
            // 
            // btnBanAcc
            // 
            this.btnBanAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBanAcc.Enabled = false;
            this.btnBanAcc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBanAcc.Location = new System.Drawing.Point(210, 217);
            this.btnBanAcc.Margin = new System.Windows.Forms.Padding(4);
            this.btnBanAcc.Name = "btnBanAcc";
            this.btnBanAcc.Size = new System.Drawing.Size(156, 26);
            this.btnBanAcc.TabIndex = 11;
            this.btnBanAcc.Text = "BAN";
            this.btnBanAcc.UseVisualStyleBackColor = true;
            this.btnBanAcc.Click += new System.EventHandler(this.btnBanAcc_Click);
            // 
            // btnUnbanAcc
            // 
            this.btnUnbanAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnbanAcc.Enabled = false;
            this.btnUnbanAcc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnbanAcc.Location = new System.Drawing.Point(210, 251);
            this.btnUnbanAcc.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnbanAcc.Name = "btnUnbanAcc";
            this.btnUnbanAcc.Size = new System.Drawing.Size(156, 26);
            this.btnUnbanAcc.TabIndex = 12;
            this.btnUnbanAcc.Text = "UNBAN";
            this.btnUnbanAcc.UseVisualStyleBackColor = true;
            this.btnUnbanAcc.Click += new System.EventHandler(this.btnUnbanAcc_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 381);
            this.Controls.Add(this.btnUnbanAcc);
            this.Controls.Add(this.btnBanAcc);
            this.Controls.Add(this.btnRemoveAcc);
            this.Controls.Add(this.btnKickAllCM);
            this.Controls.Add(this.btnWriteLog);
            this.Controls.Add(this.tbKickMessage);
            this.Controls.Add(this.btnKickCM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnKickAll);
            this.Controls.Add(this.btnKick);
            this.Controls.Add(this.lbOnline);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(395, 420);
            this.MinimumSize = new System.Drawing.Size(395, 420);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainServer_FormClosing);
            this.Shown += new System.EventHandler(this.ServerForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbOnline;
        private System.Windows.Forms.Button btnKick;
        private System.Windows.Forms.Button btnKickCM;
        private System.Windows.Forms.TextBox tbKickMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnWriteLog;
        private System.Windows.Forms.Button btnKickAllCM;
        private System.Windows.Forms.Button btnKickAll;
        private System.Windows.Forms.Button btnRemoveAcc;
        private System.Windows.Forms.Button btnBanAcc;
        private System.Windows.Forms.Button btnUnbanAcc;
    }
}