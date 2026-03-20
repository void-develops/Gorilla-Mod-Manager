using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public class ModCard : Guna2Panel
    {
        private PictureBox iconBox;
        private Label nameLabel;
        private Label descLabel;
        private Label authorLabel;
        private Guna2Button downloadButton;
        private PictureBox pinPicture;
        private Label statusBadge;
        private Label upvoteBadge;

        private SbModData mod;
        private string gtagDir;

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
            set { pinned = value; pinPicture.Visible = pinned; }
        }

        public event EventHandler DownloadClicked;

        public ModCard()
        {
            Size = new Size(200, 270);
            BackColor = Color.FromArgb(30, 30, 30);
            BorderRadius = 8;
            Build();
        }

        public ModCard(SbModData modData, string gorillaTagDirectory) : this()
        {
            mod = modData;
            gtagDir = gorillaTagDirectory;

            ModName = modData.Name;
            ModDescription = modData.Description;
            authorLabel.Text = "@" + modData.Author;

            string badgeText = modData.IsFeatured ? "Featured" : modData.IsVerified ? "Verified" : "Unverified";
            Color badgeFg = modData.IsFeatured ? Color.FromArgb(255, 220, 60)
                             : modData.IsVerified ? Color.FromArgb(185, 157, 255)
                                                    : Color.FromArgb(248, 113, 113);
            Color badgeBg = modData.IsFeatured ? Color.FromArgb(80, 60, 0)
                             : modData.IsVerified ? Color.FromArgb(40, 20, 90)
                                                    : Color.FromArgb(70, 20, 30);

            statusBadge.Text = badgeText;
            statusBadge.ForeColor = badgeFg;
            statusBadge.BackColor = badgeBg;
            statusBadge.Visible = true;

            upvoteBadge.Text = "▲ " + modData.Upvotes;
            upvoteBadge.Visible = true;

            downloadButton.Click -= LegacyDownload;
            downloadButton.Click += async (s, e) => await InstallFromSb();

            if (!string.IsNullOrEmpty(modData.ImageUrl))
                _ = LoadImageAsync(modData.ImageUrl);
        }

        private void Build()
        {
            iconBox = new PictureBox
            {
                Size = new Size(180, 110),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(20, 20, 20)
            };
            Controls.Add(iconBox);

            statusBadge = new Label
            {
                AutoSize = false,
                Size = new Size(68, 16),
                Location = new Point(Width - 72, 4),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 6.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 40, 120),
                Visible = false
            };
            Controls.Add(statusBadge);
            statusBadge.Paint += BadgePaint;

            upvoteBadge = new Label
            {
                AutoSize = false,
                Size = new Size(48, 16),
                Location = new Point(Width - 52, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 6.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 180, 255),
                BackColor = Color.FromArgb(50, 25, 100),
                Visible = false
            };
            Controls.Add(upvoteBadge);
            upvoteBadge.Paint += BadgePaint;

            nameLabel = new Label
            {
                Font = new Font("Poppins", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 128),
                Size = new Size(180, 22),
                AutoEllipsis = true
            };
            Controls.Add(nameLabel);

            authorLabel = new Label
            {
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(140, 120, 180),
                Location = new Point(10, 150),
                Size = new Size(180, 16),
                AutoEllipsis = true
            };
            Controls.Add(authorLabel);

            descLabel = new Label
            {
                Font = new Font("Poppins", 7.5F),
                ForeColor = Color.LightGray,
                Location = new Point(10, 168),
                Size = new Size(180, 34),
                AutoEllipsis = true
            };
            Controls.Add(descLabel);

            downloadButton = new Guna2Button
            {
                Text = "Install",
                Size = new Size(180, 28),
                Location = new Point(10, 206),
                BorderRadius = 6,
                FillColor = Color.FromArgb(111, 69, 240),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                Animated = true,
                HoverState = { FillColor = Color.FromArgb(90, 50, 210) }
            };
            downloadButton.Click += LegacyDownload;
            Controls.Add(downloadButton);

            pinPicture = new PictureBox
            {
                Size = new Size(24, 24),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Location = new Point(Width - 26, 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Visible = false
            };
            Controls.Add(pinPicture);
            pinPicture.BringToFront();

            SizeChanged += (s, e) => RepositionBadges();
            RepositionBadges();

            statusBadge.BringToFront();
            upvoteBadge.BringToFront();
        }

        private void RepositionBadges()
        {
            statusBadge.Location = new Point(Width - statusBadge.Width - 4, 4);
            upvoteBadge.Location = new Point(Width - upvoteBadge.Width - 4, statusBadge.Bottom + 3);
        }

        private void BadgePaint(object sender, PaintEventArgs e)
        {
            var lbl = (Label)sender;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rc = lbl.ClientRectangle;
            rc.Width--; rc.Height--;
            int r = rc.Height / 2;
            using (var path = RoundedRect(rc, r))
            using (var fill = new SolidBrush(lbl.BackColor))
                g.FillPath(fill, path);
            using (var brush = new SolidBrush(lbl.ForeColor))
                g.DrawString(lbl.Text, lbl.Font, brush,
                    new RectangleF(0, 0, lbl.Width, lbl.Height),
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        private void LegacyDownload(object sender, EventArgs e) => DownloadClicked?.Invoke(this, EventArgs.Empty);

        public void UpdatePin(string pinUrl)
        {
            if (!Pinned) { pinPicture.Visible = false; return; }
            pinPicture.ImageLocation = pinUrl;
            pinPicture.Visible = true;
        }

        private async Task LoadImageAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    var bytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(bytes))
                    {
                        var img = Image.FromStream(ms);
                        if (iconBox.IsHandleCreated)
                            iconBox.Invoke((Action)(() => { iconBox.Image = img; iconBox.ImageLocation = null; }));
                        else
                            iconBox.Image = img;
                    }
                }
            }
            catch { }
        }

        private async Task InstallFromSb()
        {
            if (mod == null) return;

            if (string.IsNullOrEmpty(gtagDir) || !Directory.Exists(gtagDir))
            {
                SetBtn("No GT folder!", Color.FromArgb(248, 113, 113));
                return;
            }

            string dlUrl = mod.RepoUrl.TrimEnd('/') + "/releases/latest/download/" + mod.DllName;
            string dest = Path.Combine(gtagDir, "BepInEx", "plugins", mod.DllName);
            Directory.CreateDirectory(Path.GetDirectoryName(dest));

            SetBtn("0%", Color.FromArgb(60, 40, 120), false);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    var response = await client.GetAsync(dlUrl, HttpCompletionOption.ResponseHeadersRead);
                    if (!response.IsSuccessStatusCode)
                    {
                        SetBtn("Failed " + (int)response.StatusCode, Color.FromArgb(248, 113, 113));
                        return;
                    }

                    long total = response.Content.Headers.ContentLength ?? -1;
                    long received = 0;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var file = File.Create(dest))
                    {
                        var buf = new byte[8192];
                        int read;
                        while ((read = await stream.ReadAsync(buf, 0, buf.Length)) > 0)
                        {
                            await file.WriteAsync(buf, 0, read);
                            received += read;
                            if (total > 0)
                            {
                                int pct = (int)(received * 100 / total);
                                SetBtn(pct + "%", Color.FromArgb(60, 40, 120), false);
                            }
                        }
                    }
                }

                SetBtn("Installed ✓", Color.FromArgb(35, 110, 55));
            }
            catch (Exception ex)
            {
                SetBtn("Error: " + ex.Message, Color.FromArgb(248, 113, 113));
                await Task.Delay(3000);
                ResetBtn();
            }
        }

        private void SetBtn(string text, Color fill, bool enabled = true)
        {
            if (downloadButton.IsHandleCreated)
                downloadButton.Invoke((Action)(() =>
                {
                    downloadButton.Text = text;
                    downloadButton.FillColor = fill;
                    downloadButton.Enabled = enabled;
                }));
            else
            {
                downloadButton.Text = text;
                downloadButton.FillColor = fill;
                downloadButton.Enabled = enabled;
            }
        }

        private void ResetBtn()
        {
            SetBtn("Install", Color.FromArgb(111, 69, 240));
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}