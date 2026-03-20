using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public partial class MainApp : Form
    {
        private const string SbApi = "https://sevvy-wevvy.com/mods/sb/api.php";
        private const string GameSlug = "gorillatag";
        private const string GorillaTagSteamAppId = "1533390";

        private static readonly Color ActiveTab = Color.FromArgb(111, 69, 240);
        private static readonly Color InactiveTab = Color.FromArgb(21, 21, 21);

        private List<SbModData> allMods = new List<SbModData>();
        private string GtagDirectory;
        private FlowLayoutPanel flowPanel;
        private FlowLayoutPanel installedFlowPanel;

        public MainApp(string path = "")
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Font;
            MinimumSize = new Size(600, 400);
            GtagDirectory = path;
            FilePath.Text = GtagDirectory ?? "";
            FilePath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SearchBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FilterDropdown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ModView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            InstalledView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            BrowseTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            InstalledTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            LaunchGameBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            BrowseTabBtn.FillColor = ActiveTab;
            BrowseTabBtn.HoverState.FillColor = ActiveTab;
            InstalledTabBtn.FillColor = InactiveTab;
            InstalledTabBtn.HoverState.FillColor = InactiveTab;

            InitializeModView();
            InitializeInstalledView();
            _ = LoadModsFromApi();
        }

        private void InitializeModView()
        {
            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(24, 24, 24),
                Padding = new Padding(4),
                Margin = new Padding(0)
            };
            ModView.Controls.Add(flowPanel);
        }

        private void InitializeInstalledView()
        {
            installedFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(4),
                Margin = new Padding(0)
            };
            InstalledView.Controls.Add(installedFlowPanel);
        }

        private void BrowseTabBtn_Click(object sender, EventArgs e)
        {
            ModView.Visible = true;
            InstalledView.Visible = false;
            BrowseTabBtn.FillColor = ActiveTab;
            BrowseTabBtn.HoverState.FillColor = ActiveTab;
            InstalledTabBtn.FillColor = InactiveTab;
            InstalledTabBtn.HoverState.FillColor = InactiveTab;
            SearchBox.Visible = true;
            FilterDropdown.Visible = true;
        }

        private void InstalledTabBtn_Click(object sender, EventArgs e)
        {
            ModView.Visible = false;
            InstalledView.Visible = true;
            InstalledTabBtn.FillColor = ActiveTab;
            InstalledTabBtn.HoverState.FillColor = ActiveTab;
            BrowseTabBtn.FillColor = InactiveTab;
            BrowseTabBtn.HoverState.FillColor = InactiveTab;
            SearchBox.Visible = false;
            FilterDropdown.Visible = false;
            LoadInstalledMods();
        }

        private void LoadInstalledMods()
        {
            installedFlowPanel.Controls.Clear();

            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory))
            {
                installedFlowPanel.Controls.Add(new Label
                {
                    Text = "No Gorilla Tag folder set.",
                    ForeColor = Color.FromArgb(120, 100, 160),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = true,
                    Margin = new Padding(14)
                });
                return;
            }

            string pluginsPath = Path.Combine(GtagDirectory, "BepInEx", "plugins");
            string disabledPath = Path.Combine(GtagDirectory, "BepInEx", "plugins", "disabled");

            if (!Directory.Exists(pluginsPath))
            {
                installedFlowPanel.Controls.Add(new Label
                {
                    Text = "No mods installed. Install BepInEx first.",
                    ForeColor = Color.FromArgb(120, 100, 160),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = true,
                    Margin = new Padding(14)
                });
                return;
            }

            var enabledDlls = Directory.GetFiles(pluginsPath, "*.dll");
            var disabledDlls = Directory.Exists(disabledPath)
                ? Directory.GetFiles(disabledPath, "*.disabled")
                : new string[0];

            if (enabledDlls.Length == 0 && disabledDlls.Length == 0)
            {
                installedFlowPanel.Controls.Add(new Label
                {
                    Text = "No mods installed.",
                    ForeColor = Color.FromArgb(120, 100, 160),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = true,
                    Margin = new Padding(14)
                });
                return;
            }

            foreach (var dll in enabledDlls)
                installedFlowPanel.Controls.Add(BuildInstalledCard(dll, false));

            foreach (var dll in disabledDlls)
                installedFlowPanel.Controls.Add(BuildInstalledCard(dll, true));
        }

        private Panel BuildInstalledCard(string filePath, bool isDisabled)
        {
            string fileName = Path.GetFileName(filePath);
            string modName = isDisabled
                ? Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath))
                : Path.GetFileNameWithoutExtension(filePath);

            long fileSize = new FileInfo(filePath).Length;
            string sizeText = fileSize >= 1024 * 1024
                ? $"{fileSize / (1024f * 1024f):0.0} MB"
                : $"{fileSize / 1024f:0.0} KB";

            Color cardBg = isDisabled ? Color.FromArgb(22, 22, 22) : Color.FromArgb(30, 30, 30);
            Color nameFg = isDisabled ? Color.FromArgb(80, 80, 80) : Color.White;
            Color borderFg = isDisabled ? Color.FromArgb(40, 40, 40) : Color.FromArgb(55, 55, 55);

            var card = new Panel
            {
                Size = new Size(178, 110),
                BackColor = cardBg,
                Margin = new Padding(4)
            };

            var nameLabel = new Label
            {
                Text = modName,
                ForeColor = nameFg,
                Font = new Font("Poppins", 8F, FontStyle.Bold),
                Location = new Point(6, 10),
                Size = new Size(166, 18),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };

            var sizeLabel = new Label
            {
                Text = isDisabled ? sizeText + " · disabled" : sizeText,
                ForeColor = isDisabled ? Color.FromArgb(70, 70, 70) : Color.FromArgb(110, 110, 110),
                Font = new Font("Segoe UI", 7F),
                Location = new Point(6, 30),
                Size = new Size(166, 14),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            var toggleBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = isDisabled ? "Enable" : "X",
                FillColor = isDisabled ? Color.FromArgb(50, 50, 50) : Color.FromArgb(80, 40, 0),
                ForeColor = isDisabled ? Color.FromArgb(160, 160, 160) : Color.FromArgb(255, 140, 40),
                Font = new Font("Poppins SemiBold", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(isDisabled ? 166 : 78, 26),
                Location = new Point(6, 50),
                Animated = true
            };
            toggleBtn.HoverState.FillColor = isDisabled
                ? Color.FromArgb(70, 70, 70)
                : Color.FromArgb(110, 55, 0);

            var uninstallBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Uninstall",
                FillColor = Color.FromArgb(120, 20, 20),
                ForeColor = Color.White,
                Font = new Font("Poppins SemiBold", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(isDisabled ? 0 : 82, 26),
                Location = new Point(90, 50),
                Animated = true,
                Visible = !isDisabled
            };
            uninstallBtn.HoverState.FillColor = Color.FromArgb(180, 30, 30);

            string capturedPath = filePath;
            bool capturedDisabled = isDisabled;

            toggleBtn.Click += (s, e) =>
            {
                try
                {
                    string disabledFolder = Path.Combine(GtagDirectory, "BepInEx", "plugins", "disabled");
                    Directory.CreateDirectory(disabledFolder);

                    if (capturedDisabled)
                    {
                        string dllName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(capturedPath)) + ".dll";
                        string dest = Path.Combine(GtagDirectory, "BepInEx", "plugins", dllName);
                        File.Move(capturedPath, dest);
                    }
                    else
                    {
                        string disabledName = Path.GetFileName(capturedPath) + ".disabled";
                        string dest = Path.Combine(disabledFolder, disabledName);
                        File.Move(capturedPath, dest);
                    }

                    LoadInstalledMods();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to toggle mod: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            uninstallBtn.Click += (s, e) =>
            {
                var result = MessageBox.Show(
                    $"Uninstall {modName}?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                try
                {
                    File.Delete(capturedPath);
                    LoadInstalledMods();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to uninstall: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            card.Controls.Add(nameLabel);
            card.Controls.Add(sizeLabel);
            card.Controls.Add(toggleBtn);
            if (!isDisabled)
                card.Controls.Add(uninstallBtn);

            Color capturedBorder = borderFg;
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(capturedBorder, 1f))
                    g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }

        private void LaunchGameBtn_Click(object sender, EventArgs e)
        {
            string steamExe = FindSteamExe();

            if (steamExe != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = steamExe,
                    Arguments = $"-applaunch {GorillaTagSteamAppId}",
                    UseShellExecute = true
                });
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = $"steam://rungameid/{GorillaTagSteamAppId}",
                UseShellExecute = true
            });
        }

        private string FindSteamExe()
        {
            string[] candidates = new[]
            {
                @"C:\Program Files (x86)\Steam\steam.exe",
                @"C:\Program Files\Steam\steam.exe",
            };

            foreach (var path in candidates)
            {
                if (File.Exists(path))
                    return path;
            }

            try
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        string steamPath = key.GetValue("SteamExe") as string;
                        if (!string.IsNullOrEmpty(steamPath) && File.Exists(steamPath))
                            return steamPath;

                        string steamDir = key.GetValue("SteamPath") as string;
                        if (!string.IsNullOrEmpty(steamDir))
                        {
                            string exe = Path.Combine(steamDir.Replace("/", "\\"), "steam.exe");
                            if (File.Exists(exe))
                                return exe;
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private void Minimize_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        private void Close_Click(object sender, EventArgs e) => Application.Exit();

        private void DashboardBtn_Click(object sender, EventArgs e) =>
            Process.Start(new ProcessStartInfo
            { FileName = "https://sevvy-wevvy.com/mods/sb/dashboard/", UseShellExecute = true });

        private void MainApp_Load(object sender, EventArgs e) { }

        private void Github_Click(object sender, EventArgs e) =>
            Process.Start(new ProcessStartInfo
            { FileName = "https://github.com/void-develops?tab=repositories", UseShellExecute = true });

        private void SearchBox_TextChanged(object sender, EventArgs e) => ApplyFilter();
        private void FilterDropdown_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();

        private void opengamepath_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = "Select Gorilla Tag Executable",
                Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                FilterIndex = 1
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                string fn = System.IO.Path.GetFileName(dlg.FileName);
                if (fn.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                    fn.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                {
                    GtagDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
                    FilePath.Text = GtagDirectory;
                }
                else
                    MessageBox.Show("Sorry! That isn't Gorilla Tag.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadModsFromApi()
        {
            flowPanel.Controls.Clear();
            flowPanel.Controls.Add(new Label
            {
                Text = "Loading mods...",
                ForeColor = Color.FromArgb(160, 140, 200),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(14)
            });

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");

                    string regJson = await client.GetStringAsync(
                        $"{SbApi}?action=register_game&slug={GameSlug}");
                    int gameId = JObject.Parse(regJson)["data"]?["id"]?.ToObject<int>() ?? 0;

                    string listUrl = $"{SbApi}?action=list_mods&tab=all&page=1"
                                    + (gameId > 0 ? $"&game_id={gameId}" : "");
                    string listJson = await client.GetStringAsync(listUrl);
                    var modsArray = JObject.Parse(listJson)["data"]?["mods"] as JArray
                                    ?? new JArray();

                    allMods.Clear();
                    foreach (var m in modsArray)
                    {
                        allMods.Add(new SbModData
                        {
                            Id = m["id"]?.ToObject<int>() ?? 0,
                            Name = m["name"]?.ToString() ?? "",
                            Description = m["description"]?.ToString() ?? "",
                            Author = m["author_username"]?.ToString() ?? "",
                            RepoUrl = (m["repo_url"]?.ToString() ?? "").Replace("\\/", "/"),
                            DllName = m["dll_name"]?.ToString() ?? "",
                            ImageUrl = (m["image_url"]?.ToString() ?? "").Replace("\\/", "/"),
                            Upvotes = m["upvotes"]?.ToObject<int>() ?? 0,
                            IsVerified = m["is_verified"]?.ToObject<int>() == 1,
                            IsFeatured = m["is_featured"]?.ToObject<int>() == 1,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                flowPanel.Controls.Clear();
                flowPanel.Controls.Add(new Label
                {
                    Text = "Failed to load: " + ex.Message,
                    ForeColor = Color.FromArgb(248, 113, 113),
                    Font = new Font("Segoe UI", 9F),
                    AutoSize = true,
                    Margin = new Padding(10)
                });
                return;
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (allMods == null || flowPanel == null) return;

            string search = SearchBox?.Text?.Trim().ToLowerInvariant() ?? "";
            string filter = FilterDropdown?.SelectedItem?.ToString() ?? "All";

            IEnumerable<SbModData> filtered = allMods;

            if (!string.IsNullOrEmpty(search))
                filtered = filtered.Where(m =>
                    m.Name.ToLowerInvariant().Contains(search) ||
                    m.Author.ToLowerInvariant().Contains(search) ||
                    m.Description.ToLowerInvariant().Contains(search));

            if (filter == "Featured")
                filtered = filtered.Where(m => m.IsFeatured);
            else if (filter == "Verified")
                filtered = filtered.Where(m => m.IsVerified && !m.IsFeatured);
            else if (filter == "Unverified")
                filtered = filtered.Where(m => !m.IsVerified && !m.IsFeatured);

            var sorted = filtered
                .OrderByDescending(m => m.IsFeatured ? 2 : m.IsVerified ? 1 : 0)
                .ThenByDescending(m => m.Upvotes)
                .ToList();

            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();

            if (!sorted.Any())
            {
                flowPanel.Controls.Add(new Label
                {
                    Text = "No mods found.",
                    ForeColor = Color.FromArgb(120, 100, 160),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = true,
                    Margin = new Padding(14)
                });
                flowPanel.ResumeLayout();
                return;
            }

            foreach (var mod in sorted)
                flowPanel.Controls.Add(new ModCard(mod, GtagDirectory));

            flowPanel.ResumeLayout();
        }

        private async void InstallBepinex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory))
            {
                MessageBox.Show("Please set your Gorilla Tag directory first.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string bepinexUrl = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip";
            string zipPath = Path.Combine(Path.GetTempPath(), "BepInEx.zip");

            var btn = (Guna.UI2.WinForms.Guna2Button)sender;
            btn.Enabled = false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");
                    var bytes = await client.GetByteArrayAsync(bepinexUrl);
                    File.WriteAllBytes(zipPath, bytes);
                }

                string destDir = GtagDirectory;
                await Task.Run(() =>
                {
                    using (var archive = new ZipArchive(File.OpenRead(zipPath), ZipArchiveMode.Read))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            string destPath = Path.Combine(destDir, entry.FullName);
                            if (string.IsNullOrEmpty(entry.Name))
                            {
                                Directory.CreateDirectory(destPath);
                            }
                            else
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                                entry.ExtractToFile(destPath, true);
                            }
                        }
                    }
                });

                File.Delete(zipPath);

                btn.FillColor = Color.FromArgb(39, 174, 96);
                btn.HoverState.FillColor = Color.FromArgb(39, 174, 96);
                btn.BorderColor = Color.FromArgb(39, 174, 96);
                btn.Enabled = true;

                MessageBox.Show("BepInEx 5.4.23.5 installed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                btn.Enabled = true;
                MessageBox.Show("Failed to install BepInEx: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModView_Paint(object sender, PaintEventArgs e) { }
        private void InstalledView_Paint(object sender, PaintEventArgs e) { }
    }

    public class SbModData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string RepoUrl { get; set; }
        public string DllName { get; set; }
        public string ImageUrl { get; set; }
        public int Upvotes { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFeatured { get; set; }
    }
}