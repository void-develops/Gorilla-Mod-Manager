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

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            iconBox = new System.Windows.Forms.PictureBox();
            nameLabel = new System.Windows.Forms.Label();
            authorLabel = new System.Windows.Forms.Label();
            descLabel = new System.Windows.Forms.Label();
            downloadButton = new Guna.UI2.WinForms.Guna2Button();
            statusBadge = new System.Windows.Forms.Panel();
            upvoteBadge = new System.Windows.Forms.Panel();
            pinPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)iconBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pinPicture).BeginInit();
            SuspendLayout();

            iconBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            iconBox.Location = new System.Drawing.Point(0, 0);
            iconBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            iconBox.Name = "iconBox";
            iconBox.Size = new System.Drawing.Size(208, 115);
            iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            iconBox.TabIndex = 0;
            iconBox.TabStop = false;

            nameLabel.AutoEllipsis = true;
            nameLabel.BackColor = System.Drawing.Color.Transparent;
            nameLabel.Font = new System.Drawing.Font("Poppins", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            nameLabel.ForeColor = System.Drawing.Color.White;
            nameLabel.Location = new System.Drawing.Point(4, 120);
            nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(200, 22);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Mod Name";
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            nameLabel.Click += nameLabel_Click;

            authorLabel.AutoEllipsis = true;
            authorLabel.BackColor = System.Drawing.Color.Transparent;
            authorLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            authorLabel.ForeColor = System.Drawing.Color.FromArgb(110, 110, 110);
            authorLabel.Location = new System.Drawing.Point(4, 144);
            authorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            authorLabel.Name = "authorLabel";
            authorLabel.Size = new System.Drawing.Size(200, 16);
            authorLabel.TabIndex = 2;
            authorLabel.Text = "@author";
            authorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            descLabel.AutoEllipsis = true;
            descLabel.BackColor = System.Drawing.Color.Transparent;
            descLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            descLabel.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            descLabel.Location = new System.Drawing.Point(4, 162);
            descLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            descLabel.Name = "descLabel";
            descLabel.Size = new System.Drawing.Size(200, 34);
            descLabel.TabIndex = 3;
            descLabel.Text = "Mod description.";
            descLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            downloadButton.Animated = true;
            downloadButton.BorderRadius = 4;
            downloadButton.CustomizableEdges = customizableEdges1;
            downloadButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            downloadButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            downloadButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            downloadButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            downloadButton.FillColor = System.Drawing.Color.FromArgb(60, 30, 140);
            downloadButton.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            downloadButton.ForeColor = System.Drawing.Color.White;
            downloadButton.HoverState.FillColor = System.Drawing.Color.FromArgb(111, 69, 240);
            downloadButton.Location = new System.Drawing.Point(7, 228);
            downloadButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            downloadButton.Name = "downloadButton";
            downloadButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
            downloadButton.Size = new System.Drawing.Size(194, 30);
            downloadButton.TabIndex = 4;
            downloadButton.Text = "Install";
            downloadButton.Click += downloadButton_Click;

            statusBadge.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            statusBadge.Location = new System.Drawing.Point(8, 202);
            statusBadge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            statusBadge.Name = "statusBadge";
            statusBadge.Size = new System.Drawing.Size(78, 18);
            statusBadge.TabIndex = 5;
            statusBadge.Visible = false;

            upvoteBadge.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            upvoteBadge.Location = new System.Drawing.Point(152, 202);
            upvoteBadge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            upvoteBadge.Name = "upvoteBadge";
            upvoteBadge.Size = new System.Drawing.Size(52, 18);
            upvoteBadge.TabIndex = 6;
            upvoteBadge.Visible = false;

            pinPicture.BackColor = System.Drawing.Color.Transparent;
            pinPicture.Location = new System.Drawing.Point(8, 202);
            pinPicture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pinPicture.Name = "pinPicture";
            pinPicture.Size = new System.Drawing.Size(23, 23);
            pinPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pinPicture.TabIndex = 7;
            pinPicture.TabStop = false;
            pinPicture.Visible = false;

            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            Controls.Add(pinPicture);
            Controls.Add(upvoteBadge);
            Controls.Add(statusBadge);
            Controls.Add(downloadButton);
            Controls.Add(descLabel);
            Controls.Add(authorLabel);
            Controls.Add(nameLabel);
            Controls.Add(iconBox);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ModCard";
            Size = new System.Drawing.Size(208, 268);
            ((System.ComponentModel.ISupportInitialize)iconBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pinPicture).EndInit();
            ResumeLayout(false);
        }

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