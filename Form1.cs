using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public partial class Form1 : Form
    {
        private const string CurrentVersion = "1.0.3";
        private const string GithubApiUrl = "https://api.github.com/repos/void-develops/Gorilla-Mod-Manager/releases/latest";
        private const string ExeName = "Gorilla Mod Manager.exe";

        private string GtagDirectory;
        private string _latestVersion;
        private string _downloadUrl;

        private static readonly Keys[] KonamiCode =
        {
            Keys.Up, Keys.Up, Keys.Down, Keys.Down,
            Keys.Left, Keys.Right, Keys.Left, Keys.Right,
            Keys.B, Keys.A
        };
        private int _konamiIndex = 0;

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            this.Load += Form1_Load;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & Keys.KeyCode;
            if (key == KonamiCode[_konamiIndex])
            {
                _konamiIndex++;
                if (_konamiIndex == KonamiCode.Length)
                {
                    _konamiIndex = 0;
                    LaunchDoom();
                }
                return true;
            }
            else
            {
                _konamiIndex = key == KonamiCode[0] ? 1 : 0;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LaunchDoom()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "steam://rungameid/2280",
                UseShellExecute = true
            });
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterControls();
        }

        private void CenterControls()
        {
            int cx = this.ClientSize.Width / 2;

            gorillamodmanager.Left = cx - gorillamodmanager.Width / 2;
            undertext.Left = cx - undertext.Width / 2;

            FilePath.Left = cx - (FilePath.Width + 6 + opengamepath.Width) / 2;
            opengamepath.Left = FilePath.Left + FilePath.Width + 6;

            Open.Left = cx - Open.Width / 2;
            Github.Left = cx - Github.Width / 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DetectGorillaTag();
            _ = CheckForUpdates();
        }

        private async Task CheckForUpdates()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    string json = await client.GetStringAsync(GithubApiUrl);
                    var release = JObject.Parse(json);

                    _latestVersion = (release["v1.0.3"]?.ToString() ?? "").TrimStart('v'); // yo void dont forget to change ts again you dummy head

                    var assets = release["assets"] as JArray;
                    if (assets != null)
                    {
                        foreach (var asset in assets)
                        {
                            string name = asset["name"]?.ToString() ?? "";
                            if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                            {
                                _downloadUrl = asset["browser_download_url"]?.ToString();
                                break;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(_downloadUrl))
                        _downloadUrl = $"https://github.com/void-develops/Gorilla-Mod-Manager/releases/latest/download/{ExeName}";

                    if (IsNewerVersion(_latestVersion, CurrentVersion))
                        ShowUpdateBanner(_latestVersion);
                }
            }
            catch { }
        }

        private bool IsNewerVersion(string latest, string current)
        {
            if (string.IsNullOrEmpty(latest)) return false;
            try { return new Version(latest) > new Version(current); }
            catch { return string.Compare(latest, current, StringComparison.Ordinal) > 0; }
        }

        private void ShowUpdateBanner(string newVersion)
        {
            if (InvokeRequired) { Invoke((Action)(() => ShowUpdateBanner(newVersion))); return; }

            var banner = new Panel
            {
                Size = new Size(this.ClientSize.Width, 36),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(111, 69, 240),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            var bannerLabel = new Label
            {
                Text = $"Update available: v{newVersion} — click Update to install",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(10, 0),
                Size = new Size(banner.Width - 100, 36),
                BackColor = Color.Transparent
            };

            var updateBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Update",
                Size = new Size(70, 24),
                Location = new Point(banner.Width - 82, 6),
                FillColor = Color.White,
                ForeColor = Color.FromArgb(111, 69, 240),
                BorderRadius = 4,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Animated = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            updateBtn.HoverState.FillColor = Color.FromArgb(240, 240, 255);
            updateBtn.Click += async (s, e) => await DownloadAndUpdate(updateBtn, bannerLabel);

            banner.Controls.Add(bannerLabel);
            banner.Controls.Add(updateBtn);

            this.Controls.Add(banner);
            banner.BringToFront();

            foreach (Control c in this.Controls)
            {
                if (c != banner)
                    c.Top += 36;
            }
        }

        private async Task DownloadAndUpdate(Guna.UI2.WinForms.Guna2Button btn, Label statusLabel)
        {
            try
            {
                btn.Enabled = false;
                statusLabel.Text = "Downloading update...";

                string tempPath = Path.Combine(Path.GetTempPath(), ExeName + ".update");
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    var response = await client.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    long total = response.Content.Headers.ContentLength ?? -1;
                    long received = 0;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var file = File.Create(tempPath))
                    {
                        var buf = new byte[8192];
                        int read;
                        while ((read = await stream.ReadAsync(buf, 0, buf.Length)) > 0)
                        {
                            await file.WriteAsync(buf, 0, read);
                            received += read;
                            if (total > 0)
                                statusLabel.Text = $"Downloading... {received * 100 / total}%";
                        }
                    }
                }

                statusLabel.Text = "Installing...";

                string batchPath = Path.Combine(Path.GetTempPath(), "gmm_update.bat");
                File.WriteAllText(batchPath,
                    "@echo off\r\n" +
                    "timeout /t 2 /nobreak >nul\r\n" +
                    $"move /y \"{tempPath}\" \"{currentExe}\"\r\n" +
                    $"start \"\" \"{currentExe}\"\r\n" +
                    "del \"%~f0\"\r\n");

                Process.Start(new ProcessStartInfo
                {
                    FileName = batchPath,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                Application.Exit();
            }
            catch (Exception ex)
            {
                btn.Enabled = true;
                statusLabel.Text = $"Update available: v{_latestVersion} — click Update to install";
                MessageBox.Show("Update failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            this.Hide();
            await Task.Delay(200);
            MainApp app = new MainApp(GtagDirectory);
            app.StartPosition = FormStartPosition.Manual;
            app.Location = this.Location;
            app.Size = this.Size;
            app.FormBorderStyle = FormBorderStyle.None;
            app.Show();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void gorillamodmanager_Click(object sender, EventArgs e) { }
        private void Close_Click(object sender, EventArgs e) => Application.Exit();
        private void Minimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void Github_Click(object sender, EventArgs e) =>
            OpenUrl("https://github.com/void-develops?tab=repositories");

        public void OpenUrl(string url)
        {
            try { Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true }); }
            catch (Exception ex) { MessageBox.Show("Link couldn't be opened: " + ex.Message); }
        }

        private void DetectGorillaTag()
        {
            string[] possiblePaths =
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe",
                @"C:\Program Files\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe"
            };

            string foundPath = null;
            foreach (var p in possiblePaths)
                if (File.Exists(p)) { foundPath = p; break; }

            if (foundPath != null)
            {
                GtagDirectory = Path.GetDirectoryName(foundPath);
                FilePath.Text = GtagDirectory;
            }
            else
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    fileDialog.Title = "Select Gorilla Tag executable";
                    fileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = fileDialog.FileName;
                        string fileName = Path.GetFileName(path);

                        if (fileName.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                            fileName.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                        {
                            GtagDirectory = Path.GetDirectoryName(path);
                            FilePath.Text = GtagDirectory;
                        }
                        else
                            MessageBox.Show("Invalid executable!", "Error");
                    }
                }
            }
        }

        private void opengamepath_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Select Gorilla Tag executable";
                fileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                fileDialog.FilterIndex = 1;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = fileDialog.FileName;
                    string fileName = Path.GetFileName(path);

                    if (fileName.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                        fileName.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        GtagDirectory = Path.GetDirectoryName(path);
                        FilePath.Text = GtagDirectory;
                    }
                    else
                        MessageBox.Show("Sorry! That isn't the correct file for Gorilla Tag", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e) { }
        private void Form1_Resize(object sender, EventArgs e) { }
    }
}