namespace MESSENGER
{
    partial class MessengerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessengerForm));
            this.lbOnline = new System.Windows.Forms.ListBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.lblChat = new System.Windows.Forms.Label();
            this.lbChat = new System.Windows.Forms.ListBox();
            this.cmsChatImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProfile = new System.Windows.Forms.Label();
            this.cmsProfile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiEditName = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.cmsImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRemoveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.pbToSend = new System.Windows.Forms.PictureBox();
            this.pbProfile = new System.Windows.Forms.PictureBox();
            this.cmsChatImage.SuspendLayout();
            this.cmsProfile.SuspendLayout();
            this.cmsImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbToSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // lbOnline
            // 
            this.lbOnline.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbOnline.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOnline.IntegralHeight = false;
            this.lbOnline.ItemHeight = 50;
            this.lbOnline.Location = new System.Drawing.Point(12, 12);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.Size = new System.Drawing.Size(171, 437);
            this.lbOnline.TabIndex = 3;
            this.lbOnline.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbOnline_DrawItem);
            this.lbOnline.SelectedIndexChanged += new System.EventHandler(this.lbOnline_SelectedIndexChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.AllowDrop = true;
            this.tbMessage.Enabled = false;
            this.tbMessage.Location = new System.Drawing.Point(190, 369);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(354, 80);
            this.tbMessage.TabIndex = 6;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            this.tbMessage.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragDrop);
            this.tbMessage.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragEnter);
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            // 
            // lblChat
            // 
            this.lblChat.AutoSize = true;
            this.lblChat.Location = new System.Drawing.Point(186, 103);
            this.lblChat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChat.Name = "lblChat";
            this.lblChat.Size = new System.Drawing.Size(44, 19);
            this.lblChat.TabIndex = 8;
            this.lblChat.Text = "Chat:";
            // 
            // lbChat
            // 
            this.lbChat.ContextMenuStrip = this.cmsChatImage;
            this.lbChat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbChat.IntegralHeight = false;
            this.lbChat.ItemHeight = 10;
            this.lbChat.Location = new System.Drawing.Point(190, 125);
            this.lbChat.Name = "lbChat";
            this.lbChat.Size = new System.Drawing.Size(354, 219);
            this.lbChat.TabIndex = 9;
            this.lbChat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbChat_DrawItem);
            this.lbChat.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbChat_MeasureItem);
            this.lbChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbChat_KeyDown);
            this.lbChat.Leave += new System.EventHandler(this.lbChat_Leave);
            // 
            // cmsChatImage
            // 
            this.cmsChatImage.AllowDrop = true;
            this.cmsChatImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyImage,
            this.tsmiSaveImage,
            this.tsmiCopyMessage});
            this.cmsChatImage.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.cmsChatImage.Name = "cmsImage";
            this.cmsChatImage.ShowImageMargin = false;
            this.cmsChatImage.Size = new System.Drawing.Size(127, 70);
            this.cmsChatImage.Opening += new System.ComponentModel.CancelEventHandler(this.cmsChatImage_Opening);
            // 
            // tsmiCopyImage
            // 
            this.tsmiCopyImage.Name = "tsmiCopyImage";
            this.tsmiCopyImage.Size = new System.Drawing.Size(126, 22);
            this.tsmiCopyImage.Text = "Copy image";
            this.tsmiCopyImage.Click += new System.EventHandler(this.tsmiCopyImage_Click);
            // 
            // tsmiSaveImage
            // 
            this.tsmiSaveImage.Name = "tsmiSaveImage";
            this.tsmiSaveImage.Size = new System.Drawing.Size(126, 22);
            this.tsmiSaveImage.Text = "Save image";
            this.tsmiSaveImage.Click += new System.EventHandler(this.tsmiSaveImage_Click);
            // 
            // tsmiCopyMessage
            // 
            this.tsmiCopyMessage.Name = "tsmiCopyMessage";
            this.tsmiCopyMessage.Size = new System.Drawing.Size(126, 22);
            this.tsmiCopyMessage.Text = "Copy message";
            this.tsmiCopyMessage.Click += new System.EventHandler(this.tsmiCopyMessage_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label5.Location = new System.Drawing.Point(186, 347);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "Type here:";
            // 
            // lblProfile
            // 
            this.lblProfile.ContextMenuStrip = this.cmsProfile;
            this.lblProfile.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.Location = new System.Drawing.Point(250, 34);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(294, 50);
            this.lblProfile.TabIndex = 13;
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmsProfile
            // 
            this.cmsProfile.AllowDrop = true;
            this.cmsProfile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditName});
            this.cmsProfile.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.cmsProfile.Name = "cmsImage";
            this.cmsProfile.ShowImageMargin = false;
            this.cmsProfile.Size = new System.Drawing.Size(103, 26);
            // 
            // tsmiEditName
            // 
            this.tsmiEditName.Name = "tsmiEditName";
            this.tsmiEditName.Size = new System.Drawing.Size(102, 22);
            this.tsmiEditName.Text = "Edit name";
            this.tsmiEditName.Click += new System.EventHandler(this.tsmiEditName_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 19);
            this.label6.TabIndex = 4;
            this.label6.Text = "Profile";
            // 
            // cmsImage
            // 
            this.cmsImage.AllowDrop = true;
            this.cmsImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRemoveImage});
            this.cmsImage.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.cmsImage.Name = "cmsImage";
            this.cmsImage.ShowImageMargin = false;
            this.cmsImage.Size = new System.Drawing.Size(93, 26);
            // 
            // tsmiRemoveImage
            // 
            this.tsmiRemoveImage.Name = "tsmiRemoveImage";
            this.tsmiRemoveImage.Size = new System.Drawing.Size(92, 22);
            this.tsmiRemoveImage.Text = "Remove";
            this.tsmiRemoveImage.Click += new System.EventHandler(this.tsmiRemoveImage_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(190, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(354, 10);
            this.button1.TabIndex = 15;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pbToSend
            // 
            this.pbToSend.ContextMenuStrip = this.cmsImage;
            this.pbToSend.Location = new System.Drawing.Point(464, 369);
            this.pbToSend.Name = "pbToSend";
            this.pbToSend.Size = new System.Drawing.Size(80, 80);
            this.pbToSend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbToSend.TabIndex = 14;
            this.pbToSend.TabStop = false;
            this.pbToSend.Visible = false;
            this.pbToSend.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragDrop);
            this.pbToSend.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbMessage_DragEnter);
            // 
            // pbProfile
            // 
            this.pbProfile.Location = new System.Drawing.Point(194, 34);
            this.pbProfile.Name = "pbProfile";
            this.pbProfile.Size = new System.Drawing.Size(50, 50);
            this.pbProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbProfile.TabIndex = 12;
            this.pbProfile.TabStop = false;
            this.pbProfile.DragDrop += new System.Windows.Forms.DragEventHandler(this.pbProfile_DragDrop);
            this.pbProfile.DragEnter += new System.Windows.Forms.DragEventHandler(this.pbProfile_DragEnter);
            // 
            // MessengerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 461);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbToSend);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.pbProfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbChat);
            this.Controls.Add(this.lblChat);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbOnline);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(572, 500);
            this.MinimumSize = new System.Drawing.Size(572, 500);
            this.Name = "MessengerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messenger 2.2.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.cmsChatImage.ResumeLayout(false);
            this.cmsProfile.ResumeLayout(false);
            this.cmsImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbToSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbOnline;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label lblChat;
        private System.Windows.Forms.ListBox lbChat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pbProfile;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbToSend;
        private System.Windows.Forms.ContextMenuStrip cmsImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemoveImage;
        private System.Windows.Forms.ContextMenuStrip cmsChatImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip cmsProfile;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditName;
    }
}