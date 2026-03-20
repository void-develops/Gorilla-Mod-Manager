using Guna.UI2.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public class MainApp : Form
    {
        private Guna2BorderlessForm guna2BorderlessForm1;
        private System.ComponentModel.IContainer components;
        private Guna2Separator guna2Separator1;
        private Guna2Button opengamepath;
        private Guna2TextBox FilePath;
        private Label gorillamodmanager;
        private Guna2Panel ModView;
        private Guna2CircleButton Minimize;
        private Guna2CircleButton Close;
        private Guna2TextBox SearchBox;
        private Guna2ComboBox FilterDropdown;
        private Guna2Button DashboardBtn;
        private string GtagDirectory;
        private FlowLayoutPanel flowPanel;

        private const string SbApi = "https://sevvy-wevvy.com/mods/sb/api.php";
        private const string GameSlug = "gorillatag";
        private List<SbModData> allMods = new List<SbModData>();

        public MainApp(string path = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(path))
                GtagDirectory = path;
            DetectGorillaTag();
            FilePath.Text = GtagDirectory;
            InitializeModView();
            _ = LoadModsFromApi();
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));

            guna2BorderlessForm1 = new Guna2BorderlessForm(components);
            opengamepath = new Guna2Button();
            FilePath = new Guna2TextBox();
            gorillamodmanager = new Label();
            guna2Separator1 = new Guna2Separator();
            Minimize = new Guna2CircleButton();
            Close = new Guna2CircleButton();
            SearchBox = new Guna2TextBox();
            FilterDropdown = new Guna2ComboBox();
            DashboardBtn = new Guna2Button();

            SuspendLayout();

            guna2BorderlessForm1.AnimateWindow = true;
            guna2BorderlessForm1.BorderRadius = 5;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;

            gorillamodmanager.AutoSize = true;
            gorillamodmanager.Font = new Font("Poppins", 12F);
            gorillamodmanager.ForeColor = Color.White;
            gorillamodmanager.Location = new Point(7, 12);
            gorillamodmanager.Text = "gorilla mod manager";

            Minimize.Animated = true;
            Minimize.FillColor = Color.FromArgb(255, 162, 80);
            Minimize.Font = new Font("Segoe UI", 9F);
            Minimize.ForeColor = Color.White;
            Minimize.Location = new Point(479, 12);
            Minimize.Size = new Size(20, 20);
            Minimize.TabIndex = 0;
            Minimize.Click += Minimize_Click;

            Close.Animated = true;
            Close.FillColor = Color.FromArgb(255, 80, 80);
            Close.Font = new Font("Segoe UI", 9F);
            Close.ForeColor = Color.White;
            Close.Location = new Point(505, 12);
            Close.Size = new Size(20, 20);
            Close.TabIndex = 1;
            Close.Click += Close_Click;

            FilePath.Animated = true;
            FilePath.BorderColor = Color.FromArgb(33, 33, 33);
            FilePath.BorderRadius = 4;
            FilePath.FillColor = Color.FromArgb(21, 21, 21);
            FilePath.Font = new Font("Segoe UI", 9F);
            FilePath.Location = new Point(196, 38);
            FilePath.Size = new Size(222, 26);
            FilePath.TabIndex = 5;

            opengamepath.Animated = true;
            opengamepath.BorderColor = Color.FromArgb(33, 33, 33);
            opengamepath.BorderRadius = 4;
            opengamepath.FillColor = Color.FromArgb(21, 21, 21);
            opengamepath.Font = new Font("Poppins SemiBold", 7.5F, FontStyle.Bold);
            opengamepath.ForeColor = Color.White;
            opengamepath.Location = new Point(424, 38);
            opengamepath.Size = new Size(37, 26);
            opengamepath.Text = "...";
            opengamepath.Click += opengamepath_Click;

            DashboardBtn.Animated = true;
            DashboardBtn.BorderColor = Color.FromArgb(111, 69, 240);
            DashboardBtn.BorderRadius = 4;
            DashboardBtn.FillColor = Color.FromArgb(60, 30, 140);
            DashboardBtn.Font = new Font("Poppins SemiBold", 7F, FontStyle.Bold);
            DashboardBtn.ForeColor = Color.White;
            DashboardBtn.Location = new Point(467, 38);
            DashboardBtn.Size = new Size(68, 26);
            DashboardBtn.Text = "Dashboard";
            DashboardBtn.Click += (s, e) =>
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                { FileName = "https://sevvy-wevvy.com/mods/sb/dashboard/", UseShellExecute = true });

            SearchBox.Animated = true;
            SearchBox.BorderColor = Color.FromArgb(33, 33, 33);
            SearchBox.BorderRadius = 4;
            SearchBox.FillColor = Color.FromArgb(21, 21, 21);
            SearchBox.Font = new Font("Segoe UI", 9F);
            SearchBox.PlaceholderText = "Search mods...";
            SearchBox.ForeColor = Color.White;
            SearchBox.Location = new Point(12, 72);
            SearchBox.Size = new Size(280, 26);
            SearchBox.TabIndex = 7;
            SearchBox.TextChanged += (s, e) => ApplyFilter();

            FilterDropdown.BackColor = Color.FromArgb(21, 21, 21);
            FilterDropdown.DrawMode = DrawMode.OwnerDrawFixed;
            FilterDropdown.FillColor = Color.FromArgb(21, 21, 21);
            FilterDropdown.FocusedColor = Color.FromArgb(111, 69, 240);
            FilterDropdown.Font = new Font("Segoe UI", 9F);
            FilterDropdown.ForeColor = Color.White;
            FilterDropdown.Location = new Point(300, 72);
            FilterDropdown.Size = new Size(130, 26);
            FilterDropdown.TabIndex = 8;
            FilterDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            FilterDropdown.Items.AddRange(new object[] { "All", "Featured", "Verified", "Unverified" });
            FilterDropdown.SelectedIndex = 0;
            FilterDropdown.SelectedIndexChanged += (s, e) => ApplyFilter();

            guna2Separator1.FillColor = Color.FromArgb(33, 33, 33);
            guna2Separator1.Location = new Point(-12, 101);
            guna2Separator1.Size = new Size(560, 6);
            guna2Separator1.TabIndex = 2;

            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(536, 420);
            FormBorderStyle = FormBorderStyle.None;
            Text = "Gorilla Mod Manager";

            Controls.Add(gorillamodmanager);
            Controls.Add(Minimize);
            Controls.Add(Close);
            Controls.Add(FilePath);
            Controls.Add(opengamepath);
            Controls.Add(DashboardBtn);
            Controls.Add(SearchBox);
            Controls.Add(FilterDropdown);
            Controls.Add(guna2Separator1);

            try { Icon = (Icon)resources.GetObject("$this.Icon"); } catch { }

            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeModView()
        {
            ModView = new Guna2Panel
            {
                Location = new Point(12, 110),
                Size = new Size(512, 298),
                BorderRadius = 5,
                BackColor = Color.FromArgb(33, 33, 33)
            };
            Controls.Add(ModView);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(6)
            };
            ModView.Controls.Add(flowPanel);
        }

        private void Minimize_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        private void Close_Click(object sender, EventArgs e) => Application.Exit();

        private void DetectGorillaTag()
        {
            string[] possiblePaths =
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe",
                @"C:\Program Files\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe"
            };

            foreach (var p in possiblePaths)
            {
                if (File.Exists(p)) { GtagDirectory = Path.GetDirectoryName(p); return; }
            }

            using (var dlg = new OpenFileDialog
            {
                Title = "Select Gorilla Tag Executable",
                Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*"
            })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string fn = Path.GetFileName(dlg.FileName);
                    if (fn.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                        fn.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                        GtagDirectory = Path.GetDirectoryName(dlg.FileName);
                    else
                        MessageBox.Show("Invalid executable!", "Error");
                }
            }
        }

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
                string fn = Path.GetFileName(dlg.FileName);
                if (fn.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                    fn.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                {
                    GtagDirectory = Path.GetDirectoryName(dlg.FileName);
                    FilePath.Text = GtagDirectory;
                }
                else
                    MessageBox.Show("Sorry! That isn't Gorilla Tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadModsFromApi()
        {
            flowPanel.Controls.Clear();

            var loading = new Label
            {
                Text = "Loading mods...",
                ForeColor = Color.FromArgb(160, 140, 200),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(12)
            };
            flowPanel.Controls.Add(loading);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");

                    string regUrl = $"{SbApi}?action=register_game&slug={GameSlug}";
                    string regJson = await client.GetStringAsync(regUrl);
                    var regData = JObject.Parse(regJson);
                    int gameId = regData["data"]?["id"]?.ToObject<int>() ?? 0;

                    string listUrl = $"{SbApi}?action=list_mods&tab=all&page=1" + (gameId > 0 ? $"&game_id={gameId}" : "");
                    string listJson = await client.GetStringAsync(listUrl);
                    var listData = JObject.Parse(listJson);
                    var modsArray = listData["data"]?["mods"] as JArray ?? new JArray();

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
                    Text = "Failed to load mods: " + ex.Message,
                    ForeColor = Color.FromArgb(248, 113, 113),
                    Font = new Font("Segoe UI", 9F),
                    AutoSize = true,
                    Margin = new Padding(8)
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
                    Margin = new Padding(12)
                });
                flowPanel.ResumeLayout();
                return;
            }

            foreach (var mod in sorted)
            {
                var card = new ModCard(mod, GtagDirectory);
                flowPanel.Controls.Add((Control)(object)card);
            }

            flowPanel.ResumeLayout();
        }

        internal string GetModName(string link)
        {
            var trimmed = link.Trim();
            if (trimmed.Length == 0) return string.Empty;
            var lastSlash = trimmed.LastIndexOf('/');
            if (lastSlash < 0) return string.Empty;
            var modName = trimmed.Substring(lastSlash + 1);
            var dllIndex = modName.IndexOf(".dll", StringComparison.OrdinalIgnoreCase);
            if (dllIndex > 0) modName = modName.Substring(0, dllIndex);
            return modName;
        }
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