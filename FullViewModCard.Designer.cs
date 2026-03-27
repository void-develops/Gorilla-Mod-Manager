namespace Gorilla_Mod_Manager
{
    partial class FullViewModCard
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            iconBox = new System.Windows.Forms.PictureBox();
            nameLabel = new System.Windows.Forms.Label();
            authorLabel = new System.Windows.Forms.Label();
            descLabel = new System.Windows.Forms.Label();
            downloadButton = new Guna.UI2.WinForms.Guna2Button();
            statusBadge = new System.Windows.Forms.Panel();
            upvoteBadge = new System.Windows.Forms.Panel();
            pinPicture = new System.Windows.Forms.PictureBox();
            ViewRepoBtn = new Guna.UI2.WinForms.Guna2Button();
            ExitBtn = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)iconBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pinPicture).BeginInit();
            SuspendLayout();
            // 
            // iconBox
            // 
            iconBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            iconBox.Location = new System.Drawing.Point(4, 3);
            iconBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            iconBox.Name = "iconBox";
            iconBox.Size = new System.Drawing.Size(303, 142);
            iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            iconBox.TabIndex = 0;
            iconBox.TabStop = false;
            iconBox.Click += iconBox_Click;
            // 
            // nameLabel
            // 
            nameLabel.AutoEllipsis = true;
            nameLabel.BackColor = System.Drawing.Color.Transparent;
            nameLabel.Font = new System.Drawing.Font("Poppins", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            nameLabel.ForeColor = System.Drawing.Color.White;
            nameLabel.Location = new System.Drawing.Point(312, 28);
            nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(224, 31);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Mod Name";
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            nameLabel.Click += nameLabel_Click;
            // 
            // authorLabel
            // 
            authorLabel.AutoEllipsis = true;
            authorLabel.BackColor = System.Drawing.Color.Transparent;
            authorLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            authorLabel.ForeColor = System.Drawing.Color.FromArgb(110, 110, 110);
            authorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            authorLabel.Location = new System.Drawing.Point(312, 59);
            authorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            authorLabel.Name = "authorLabel";
            authorLabel.Size = new System.Drawing.Size(224, 34);
            authorLabel.TabIndex = 2;
            authorLabel.Text = "@author";
            authorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            authorLabel.Click += authorLabel_Click;
            // 
            // descLabel
            // 
            descLabel.AutoEllipsis = true;
            descLabel.BackColor = System.Drawing.Color.Transparent;
            descLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            descLabel.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            descLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            descLabel.Location = new System.Drawing.Point(312, 93);
            descLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            descLabel.Name = "descLabel";
            descLabel.Size = new System.Drawing.Size(224, 52);
            descLabel.TabIndex = 3;
            descLabel.Text = "Mod description.";
            descLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            descLabel.Click += descLabel_Click;
            // 
            // downloadButton
            // 
            downloadButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
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
            downloadButton.Location = new System.Drawing.Point(493, 272);
            downloadButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            downloadButton.Name = "downloadButton";
            downloadButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
            downloadButton.Size = new System.Drawing.Size(98, 30);
            downloadButton.TabIndex = 4;
            downloadButton.Text = "Install";
            downloadButton.Click += downloadButton_Click;
            // 
            // statusBadge
            // 
            statusBadge.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            statusBadge.Location = new System.Drawing.Point(315, 7);
            statusBadge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            statusBadge.Name = "statusBadge";
            statusBadge.Size = new System.Drawing.Size(78, 18);
            statusBadge.TabIndex = 5;
            statusBadge.Visible = false;
            statusBadge.Paint += statusBadge_Paint;
            // 
            // upvoteBadge
            // 
            upvoteBadge.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            upvoteBadge.Location = new System.Drawing.Point(401, 7);
            upvoteBadge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            upvoteBadge.Name = "upvoteBadge";
            upvoteBadge.Size = new System.Drawing.Size(52, 18);
            upvoteBadge.TabIndex = 6;
            upvoteBadge.Visible = false;
            upvoteBadge.Paint += upvoteBadge_Paint;
            // 
            // pinPicture
            // 
            pinPicture.BackColor = System.Drawing.Color.Transparent;
            pinPicture.Location = new System.Drawing.Point(284, 2);
            pinPicture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pinPicture.Name = "pinPicture";
            pinPicture.Size = new System.Drawing.Size(23, 23);
            pinPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pinPicture.TabIndex = 7;
            pinPicture.TabStop = false;
            pinPicture.Visible = false;
            pinPicture.Click += pinPicture_Click;
            // 
            // ViewRepoBtn
            // 
            ViewRepoBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ViewRepoBtn.Animated = true;
            ViewRepoBtn.BorderRadius = 4;
            ViewRepoBtn.CustomizableEdges = customizableEdges3;
            ViewRepoBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            ViewRepoBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            ViewRepoBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            ViewRepoBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            ViewRepoBtn.FillColor = System.Drawing.Color.FromArgb(60, 30, 140);
            ViewRepoBtn.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ViewRepoBtn.ForeColor = System.Drawing.Color.White;
            ViewRepoBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(111, 69, 240);
            ViewRepoBtn.Location = new System.Drawing.Point(387, 272);
            ViewRepoBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ViewRepoBtn.Name = "ViewRepoBtn";
            ViewRepoBtn.ShadowDecoration.CustomizableEdges = customizableEdges4;
            ViewRepoBtn.Size = new System.Drawing.Size(98, 30);
            ViewRepoBtn.TabIndex = 8;
            ViewRepoBtn.Text = "View Repo";
            ViewRepoBtn.Click += ViewRepoBtn_Click;
            // 
            // ExitBtn
            // 
            ExitBtn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ExitBtn.Animated = true;
            ExitBtn.BorderRadius = 4;
            ExitBtn.CustomizableEdges = customizableEdges5;
            ExitBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            ExitBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            ExitBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            ExitBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            ExitBtn.FillColor = System.Drawing.Color.Transparent;
            ExitBtn.Font = new System.Drawing.Font("Poppins SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ExitBtn.ForeColor = System.Drawing.Color.White;
            ExitBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(111, 69, 240);
            ExitBtn.Location = new System.Drawing.Point(544, 3);
            ExitBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ExitBtn.Size = new System.Drawing.Size(49, 45);
            ExitBtn.TabIndex = 9;
            ExitBtn.Text = "X";
            ExitBtn.Click += ExitBtn_Click;
            // 
            // FullViewModCard
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            Controls.Add(ExitBtn);
            Controls.Add(ViewRepoBtn);
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
            Name = "FullViewModCard";
            Size = new System.Drawing.Size(595, 305);
            Load += FullViewModCard_Load;
            ((System.ComponentModel.ISupportInitialize)iconBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pinPicture).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label descLabel;
        internal Guna.UI2.WinForms.Guna2Button downloadButton;
        private System.Windows.Forms.Panel statusBadge;
        private System.Windows.Forms.Panel upvoteBadge;
        private System.Windows.Forms.PictureBox pinPicture;
        internal Guna.UI2.WinForms.Guna2Button ViewRepoBtn;
        internal Guna.UI2.WinForms.Guna2Button ExitBtn;
    }
}