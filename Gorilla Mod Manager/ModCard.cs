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
    public partial class ModCard : UserControl
    {
        private SbModData mod;
        private string gtagDir;

        private string statusText = "";
        private Color statusFg = Color.FromArgb(160, 160, 160);
        private Color statusBg = Color.FromArgb(40, 40, 40);
        private string upvoteText = "▲ 0";
        private Image pinImage = null;

        public string ModName { get => nameLabel.Text; set => nameLabel.Text = value; }
        public string ModDescription { get => descLabel.Text; set => descLabel.Text = value; }
        public string IconUrl { get => iconBox.ImageLocation; set => iconBox.ImageLocation = value; }
        public string DownloadUrl { get; set; }

        private bool _pinned;
        public bool Pinned
        {
            get => _pinned;
            set { _pinned = value; Invalidate(); }
        }

        public event EventHandler DownloadClicked;

        public ModCard()
        {
            InitializeComponent();
            DoubleBuffered = true;
            statusBadge.Paint += StatusBadgePaint;
            upvoteBadge.Paint += UpvoteBadgePaint;
            downloadButton.Click += LegacyDownload;
            Margin = new Padding(4);
        }

        public ModCard(SbModData modData, string gorillaTagDirectory) : this()
        {
            mod = modData;
            gtagDir = gorillaTagDirectory;

            ModName = modData.Name;
            ModDescription = modData.Description;
            authorLabel.Text = "@" + modData.Author;
            upvoteText = "▲ " + modData.Upvotes;

            if (modData.IsFeatured)
            {
                statusText = "Featured";
                statusFg = Color.FromArgb(255, 208, 56);
                statusBg = Color.FromArgb(50, 38, 0);
            }
            else if (modData.IsVerified)
            {
                statusText = "Verified";
                statusFg = Color.FromArgb(140, 100, 255);
                statusBg = Color.FromArgb(35, 18, 70);
            }
            else
            {
                statusText = "Unverified";
                statusFg = Color.FromArgb(130, 130, 130);
                statusBg = Color.FromArgb(38, 38, 38);
            }

            statusBadge.Visible = true;
            upvoteBadge.Visible = true;
            statusBadge.Invalidate();
            upvoteBadge.Invalidate();

            downloadButton.Click -= LegacyDownload;
            downloadButton.Click += async (s, e) => await InstallFromSb();

            if (!string.IsNullOrEmpty(modData.ImageUrl))
                _ = LoadImageAsync(modData.ImageUrl);
        }

        private void StatusBadgePaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rc = new Rectangle(0, 0, statusBadge.Width - 1, statusBadge.Height - 1);
            int r = rc.Height / 2;
            using (var path = Pill(rc, r))
            {
                using (var fill = new SolidBrush(statusBg))
                    g.FillPath(fill, path);
                using (var pen = new Pen(statusFg, 1f))
                    g.DrawPath(pen, path);
            }
            using (var brush = new SolidBrush(statusFg))
            using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(statusText, new Font("Segoe UI", 6.5F, FontStyle.Bold), brush,
                    new RectangleF(0, 0, statusBadge.Width, statusBadge.Height), fmt);
        }

        private void UpvoteBadgePaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rc = new Rectangle(0, 0, upvoteBadge.Width - 1, upvoteBadge.Height - 1);
            int r = rc.Height / 2;
            using (var path = Pill(rc, r))
            {
                using (var fill = new SolidBrush(Color.FromArgb(35, 18, 70)))
                    g.FillPath(fill, path);
                using (var pen = new Pen(Color.FromArgb(111, 69, 240), 1f))
                    g.DrawPath(pen, path);
            }
            using (var brush = new SolidBrush(Color.FromArgb(185, 157, 255)))
            using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(upvoteText, new Font("Segoe UI", 6.5F, FontStyle.Bold), brush,
                    new RectangleF(0, 0, upvoteBadge.Width, upvoteBadge.Height), fmt);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Pill(rc, 8))
            {
                using (var bg = new SolidBrush(Color.FromArgb(30, 30, 30)))
                    g.FillPath(bg, path);
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Pill(rc, 8))
            using (var pen = new Pen(Color.FromArgb(55, 55, 55), 1f))
                g.DrawPath(pen, path);

            if (_pinned && pinImage != null)
            {
                var s = g.Save();
                int cx = pinPicture.Left + pinPicture.Width / 2;
                int cy = pinPicture.Top + pinPicture.Height / 2;
                g.TranslateTransform(cx, cy);
                g.RotateTransform(-90);
                g.DrawImage(pinImage, -pinPicture.Width / 2, -pinPicture.Height / 2,
                    pinPicture.Width, pinPicture.Height);
                g.Restore(s);
            }
        }

        private void LegacyDownload(object sender, EventArgs e) =>
            DownloadClicked?.Invoke(this, EventArgs.Empty);

        public void UpdatePin(string pinUrl)
        {
            if (!_pinned) { pinImage = null; Invalidate(); return; }
            _ = LoadPinAsync(pinUrl);
        }

        private async Task LoadPinAsync(string url)
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
                        if (IsHandleCreated) Invoke((Action)(() => { pinImage = img; Invalidate(); }));
                        else pinImage = img;
                    }
                }
            }
            catch { }
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
            { SetBtn("No GT folder!", Color.FromArgb(80, 20, 20)); return; }

            string dlUrl = mod.RepoUrl.TrimEnd('/') + "/releases/latest/download/" + mod.DllName;
            string dest = Path.Combine(gtagDir, "BepInEx", "plugins", mod.DllName);
            Directory.CreateDirectory(Path.GetDirectoryName(dest));
            SetBtn("0%", Color.FromArgb(33, 33, 33), false);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    var response = await client.GetAsync(dlUrl, HttpCompletionOption.ResponseHeadersRead);
                    if (!response.IsSuccessStatusCode)
                    { SetBtn("Failed " + (int)response.StatusCode, Color.FromArgb(80, 20, 20)); return; }

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
                            if (total > 0) SetBtn((int)(received * 100 / total) + "%", Color.FromArgb(33, 33, 33), false);
                        }
                    }
                }
                SetBtn("Installed ✓", Color.FromArgb(20, 80, 20));
            }
            catch (Exception ex)
            {
                SetBtn("Error!", Color.FromArgb(80, 20, 20));
                await Task.Delay(3000);
                _ = ex;
                ResetBtn();
            }
        }

        private void SetBtn(string text, Color fill, bool enabled = true)
        {
            void Do() { downloadButton.Text = text; downloadButton.FillColor = fill; downloadButton.Enabled = enabled; }
            if (downloadButton.IsHandleCreated) downloadButton.Invoke((Action)Do); else Do();
        }

        private void ResetBtn() => SetBtn("Install", Color.FromArgb(60, 30, 140));

        private static GraphicsPath Pill(Rectangle r, int radius)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            p.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            p.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {

        }
    }
}