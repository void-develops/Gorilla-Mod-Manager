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
    public partial class Form1 : Form
    {
        private const string SbApi = "https://sevvy-wevvy.com/mods/sb/api.php";
        private const string GameSlug = "gorillatag";
        private List<SbModData> allMods = new List<SbModData>();
        private string GtagDirectory;
        private FlowLayoutPanel flowPanel;

        public Form1(string path = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(path))
                GtagDirectory = path;
            DetectGorillaTag();
            FilePath.Text = GtagDirectory ?? "";
            InitializeModView();
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
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(6)
            };
            ModView.Controls.Add(flowPanel);
        }

        private void Form1_Load_1(object sender, EventArgs e) { }
        private void gorillamodmanager_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void guna2Button1_Click(object sender, EventArgs e) { }
        private void Github_Click(object sender, EventArgs e) =>
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            { FileName = "https://github.com/void-develops/GorillaModManagerModsRepo", UseShellExecute = true });

        private void Minimize_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        private void Close_Click(object sender, EventArgs e) => Application.Exit();

        private void DashboardBtn_Click(object sender, EventArgs e) =>
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            { FileName = "https://sevvy-wevvy.com/mods/sb/dashboard/", UseShellExecute = true });

        private void SearchBox_TextChanged(object sender, EventArgs e) => ApplyFilter();
        private void FilterDropdown_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();

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
            flowPanel.Controls.Add(new Label
            {
                Text = "Loading mods...",
                ForeColor = Color.FromArgb(160, 140, 200),
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(12)
            });

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager");

                    string regJson = await client.GetStringAsync($"{SbApi}?action=register_game&slug={GameSlug}");
                    int gameId = JObject.Parse(regJson)["data"]?["id"]?.ToObject<int>() ?? 0;

                    string listUrl = $"{SbApi}?action=list_mods&tab=all&page=1" + (gameId > 0 ? $"&game_id={gameId}" : "");
                    string listJson = await client.GetStringAsync(listUrl);
                    var modsArray = JObject.Parse(listJson)["data"]?["mods"] as JArray ?? new JArray();

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
                flowPanel.Controls.Add(new ModCard(mod, GtagDirectory));

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