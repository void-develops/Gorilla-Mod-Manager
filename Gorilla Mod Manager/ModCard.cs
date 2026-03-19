using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public class ModCard : Guna2Panel
    {
        private PictureBox iconBox;
        private Label nameLabel;
        private Label descLabel;
        private Guna2Button downloadButton;
        private PictureBox pinPicture;

        public string ModName
        {
            get => nameLabel.Text;
            set => nameLabel.Text = value;
        }

        public string ModDescription
        {
            get => descLabel.Text;
            set => descLabel.Text = value;
        }

        public string IconUrl
        {
            get => iconBox.ImageLocation;
            set => iconBox.ImageLocation = value;
        }

        public string DownloadUrl { get; set; }

        private bool pinned;
        public bool Pinned
        {
            get => pinned;
            set
            {
                pinned = value;
                pinPicture.Visible = pinned;
            }
        }

        public event EventHandler DownloadClicked;

        public ModCard()
        {
            this.Size = new Size(200, 250);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.BorderRadius = 8;

            iconBox = new PictureBox
            {
                Size = new Size(180, 120),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(iconBox);

            nameLabel = new Label
            {
                Font = new Font("Poppins", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 140),
                Size = new Size(180, 25)
            };
            this.Controls.Add(nameLabel);

            descLabel = new Label
            {
                Font = new Font("Poppins", 8),
                ForeColor = Color.LightGray,
                Location = new Point(10, 170),
                Size = new Size(180, 40),
                AutoEllipsis = true
            };
            this.Controls.Add(descLabel);

            downloadButton = new Guna2Button
            {
                Text = "Download",
                Size = new Size(180, 30),
                Location = new Point(10, 215),
                BorderRadius = 15,
                FillColor = Color.FromArgb(34, 49, 34),
                ForeColor = Color.White,
                HoverState = { FillColor = Color.FromArgb(46, 65, 46) }
            };
            downloadButton.Click += (s, e) => DownloadClicked?.Invoke(this, EventArgs.Empty);
            this.Controls.Add(downloadButton);

            pinPicture = new PictureBox
            {
                Size = new Size(24, 24),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Location = new Point(this.Width - 26, 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Visible = false
            };
            this.Controls.Add(pinPicture);
            pinPicture.BringToFront();
        }

        public void UpdatePin(string pinUrl)
        {
            if (!Pinned)
            {
                pinPicture.Visible = false;
                return;
            }
            pinPicture.ImageLocation = pinUrl;
            pinPicture.Visible = true;
        }
    }
}