namespace Gorilla_Mod_Manager
{
    partial class ModCard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.authorLabel = new System.Windows.Forms.Label();
            this.descLabel = new System.Windows.Forms.Label();
            this.downloadButton = new Guna.UI2.WinForms.Guna2Button();
            this.statusBadge = new System.Windows.Forms.Panel();
            this.upvoteBadge = new System.Windows.Forms.Panel();
            this.pinPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // iconBox
            // 
            this.iconBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iconBox.Location = new System.Drawing.Point(0, 0);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(178, 100);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoEllipsis = true;
            this.nameLabel.BackColor = System.Drawing.Color.Transparent;
            this.nameLabel.Font = new System.Drawing.Font("Poppins", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.White;
            this.nameLabel.Location = new System.Drawing.Point(6, 106);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(166, 18);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Mod Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authorLabel
            // 
            this.authorLabel.AutoEllipsis = true;
            this.authorLabel.BackColor = System.Drawing.Color.Transparent;
            this.authorLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.authorLabel.Location = new System.Drawing.Point(6, 124);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(166, 14);
            this.authorLabel.TabIndex = 2;
            this.authorLabel.Text = "@author";
            this.authorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // descLabel
            // 
            this.descLabel.AutoEllipsis = true;
            this.descLabel.BackColor = System.Drawing.Color.Transparent;
            this.descLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.descLabel.Location = new System.Drawing.Point(6, 140);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(166, 30);
            this.descLabel.TabIndex = 3;
            this.descLabel.Text = "Mod description.";
            this.descLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // downloadButton
            // 
            this.downloadButton.Animated = true;
            this.downloadButton.BorderRadius = 4;
            this.downloadButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.downloadButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.downloadButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.downloadButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.downloadButton.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(30)))), ((int)(((byte)(140)))));
            this.downloadButton.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadButton.ForeColor = System.Drawing.Color.White;
            this.downloadButton.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(69)))), ((int)(((byte)(240)))));
            this.downloadButton.Location = new System.Drawing.Point(6, 194);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(166, 26);
            this.downloadButton.TabIndex = 4;
            this.downloadButton.Text = "Install";
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // statusBadge
            // 
            this.statusBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.statusBadge.Location = new System.Drawing.Point(10, 173);
            this.statusBadge.Name = "statusBadge";
            this.statusBadge.Size = new System.Drawing.Size(62, 15);
            this.statusBadge.TabIndex = 5;
            this.statusBadge.Visible = false;
            // 
            // upvoteBadge
            // 
            this.upvoteBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.upvoteBadge.Location = new System.Drawing.Point(132, 173);
            this.upvoteBadge.Name = "upvoteBadge";
            this.upvoteBadge.Size = new System.Drawing.Size(42, 15);
            this.upvoteBadge.TabIndex = 6;
            this.upvoteBadge.Visible = false;
            // 
            // pinPicture
            // 
            this.pinPicture.BackColor = System.Drawing.Color.Transparent;
            this.pinPicture.Location = new System.Drawing.Point(10, 173);
            this.pinPicture.Name = "pinPicture";
            this.pinPicture.Size = new System.Drawing.Size(20, 20);
            this.pinPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pinPicture.TabIndex = 7;
            this.pinPicture.TabStop = false;
            this.pinPicture.Visible = false;
            // 
            // ModCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.pinPicture);
            this.Controls.Add(this.upvoteBadge);
            this.Controls.Add(this.statusBadge);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.iconBox);
            this.DoubleBuffered = true;
            this.Name = "ModCard";
            this.Size = new System.Drawing.Size(178, 227);
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label descLabel;
        private Guna.UI2.WinForms.Guna2Button downloadButton;
        private System.Windows.Forms.Panel statusBadge;
        private System.Windows.Forms.Panel upvoteBadge;
        private System.Windows.Forms.PictureBox pinPicture;
    }
}