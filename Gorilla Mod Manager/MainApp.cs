using Guna.UI2.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public partial class MainApp : Form
    {
        private Guna2BorderlessForm guna2BorderlessForm1;
        private System.ComponentModel.IContainer components;
        private Guna2Separator guna2Separator1;
        private Guna2Button opengamepath;
        private Label undertext;
        private Guna2TextBox FilePath;
        private Label gorillamodmanager;
        private Guna2Panel ModView;
        private Guna2VScrollBar modScrollBar;
        private Guna2CircleButton Minimize;
        private Guna2CircleButton Close;
        private string GtagDirectory;
        private FlowLayoutPanel flowPanel;

        public MainApp(string path = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(path))
                GtagDirectory = path;
            DetectGorillaTag();
            FilePath.Text = GtagDirectory;
            InitializeModView();
            _ = LoadModsFromGitHub();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.opengamepath = new Guna.UI2.WinForms.Guna2Button();
            this.undertext = new System.Windows.Forms.Label();
            this.FilePath = new Guna.UI2.WinForms.Guna2TextBox();
            this.gorillamodmanager = new System.Windows.Forms.Label();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.Minimize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.Close = new Guna.UI2.WinForms.Guna2CircleButton();
            this.SuspendLayout();

            this.guna2BorderlessForm1.AnimateWindow = true;
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;

            this.opengamepath.Animated = true;
            this.opengamepath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.opengamepath.BorderRadius = 4;
            this.opengamepath.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.opengamepath.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold);
            this.opengamepath.ForeColor = System.Drawing.Color.White;
            this.opengamepath.Location = new System.Drawing.Point(424, 38);
            this.opengamepath.Name = "opengamepath";
            this.opengamepath.Size = new System.Drawing.Size(37, 26);
            this.opengamepath.TabIndex = 3;
            this.opengamepath.Text = "...";
            this.opengamepath.Click += new System.EventHandler(this.opengamepath_Click);

            this.undertext.AutoSize = true;
            this.undertext.Font = new System.Drawing.Font("Poppins", 6F);
            this.undertext.ForeColor = System.Drawing.Color.White;
            this.undertext.Location = new System.Drawing.Point(21, 40);
            this.undertext.Name = "undertext";
            this.undertext.Size = new System.Drawing.Size(144, 14);
            this.undertext.TabIndex = 4;
            this.undertext.Text = "inspired by monkey mod manager.";

            this.FilePath.Animated = true;
            this.FilePath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.FilePath.BorderRadius = 4;
            this.FilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilePath.DefaultText = "";
            this.FilePath.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.FilePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FilePath.Location = new System.Drawing.Point(196, 38);
            this.FilePath.Name = "FilePath";
            this.FilePath.PlaceholderText = "";
            this.FilePath.SelectedText = "";
            this.FilePath.Size = new System.Drawing.Size(222, 26);
            this.FilePath.TabIndex = 5;

            this.gorillamodmanager.AutoSize = true;
            this.gorillamodmanager.Font = new System.Drawing.Font("Poppins", 12F);
            this.gorillamodmanager.ForeColor = System.Drawing.Color.White;
            this.gorillamodmanager.Location = new System.Drawing.Point(7, 12);
            this.gorillamodmanager.Name = "gorillamodmanager";
            this.gorillamodmanager.Size = new System.Drawing.Size(182, 28);
            this.gorillamodmanager.TabIndex = 6;
            this.gorillamodmanager.Text = "gorilla mod manager";

            this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.guna2Separator1.Location = new System.Drawing.Point(-12, 67);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(486, 10);
            this.guna2Separator1.TabIndex = 2;

            this.Minimize.Animated = true;
            this.Minimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(80)))));
            this.Minimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.Location = new System.Drawing.Point(415, 12);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(20, 20);
            this.Minimize.TabIndex = 0;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);

            this.Close.Animated = true;
            this.Close.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.Close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Close.ForeColor = System.Drawing.Color.White;
            this.Close.Location = new System.Drawing.Point(441, 12);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(20, 20);
            this.Close.TabIndex = 1;
            this.Close.Click += new System.EventHandler(this.Close_Click);

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(472, 353);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.guna2Separator1);
            this.Controls.Add(this.opengamepath);
            this.Controls.Add(this.undertext);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.gorillamodmanager);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainApp";
            this.Text = "Gorilla Mod Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializeModView()
        {
            ModView = new Guna2Panel
            {
                Location = new Point(12, 83),
                Size = new Size(448, 258),
                BorderRadius = 5,
                BackColor = Color.FromArgb(33, 33, 33)
            };
            this.Controls.Add(ModView);

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(33, 33, 33)
            };
            ModView.Controls.Add(flowPanel);
        }

        private void Minimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private void Close_Click(object sender, EventArgs e) => Application.Exit();

        private void DetectGorillaTag()
        {
            string[] possiblePaths =
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe",
                @"C:\Program Files\Steam\steamapps\common\Gorilla Tag\Gorilla Tag.exe"
            };

            string foundPath = null;
            foreach (var p in possiblePaths)
            {
                if (File.Exists(p))
                {
                    foundPath = p;
                    break;
                }
            }

            if (foundPath != null)
                GtagDirectory = Path.GetDirectoryName(foundPath);
            else
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    fileDialog.Title = "Select Gorilla Tag Executable";
                    fileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string path = fileDialog.FileName;
                        string fileName = Path.GetFileName(path);
                        if (fileName.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) ||
                            fileName.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                            GtagDirectory = Path.GetDirectoryName(path);
                        else
                            MessageBox.Show("Invalid executable!", "Error");
                    }
                }
            }
            FilePath.Text = GtagDirectory;
        }

        private void opengamepath_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Title = "Select Gorilla Tag Executable";
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

        public async Task DownloadMod(string downloadUrl, string modName)
        {
            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory))
            {
                MessageBox.Show("Please select a valid Gorilla Tag folder first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pluginsDir = Path.Combine(GtagDirectory, "BepInEx", "plugins");
            Directory.CreateDirectory(pluginsDir);

            string fileName = GetModName(downloadUrl);
            if (string.IsNullOrEmpty(fileName)) fileName = modName;
            if (!fileName.EndsWith(".dll")) fileName += ".dll";

            string fullPath = Path.Combine(pluginsDir, fileName);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
                    var response = await client.GetAsync(downloadUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Download URL is invalid or unreachable: {downloadUrl}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    await Task.Run(() => File.WriteAllBytes(fullPath, bytes));
                }
                MessageBox.Show($"{fileName} downloaded successfully to {fullPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download {modName}.\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task LoadModsFromGitHub()
        {
            if (ModView.Controls.Count == 0) InitializeModView();
            FlowLayoutPanel panel = (FlowLayoutPanel)ModView.Controls[0];
            panel.Controls.Clear();

            string repoOwner = "void-develops";
            string repoName = "GorillaModManagerModsRepo";
            string pinUrl = $"https://raw.githubusercontent.com/{repoOwner}/{repoName}/main/pin.png";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
                string apiUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/contents/";
                string content = await client.GetStringAsync(apiUrl);
                JArray folders = JArray.Parse(content);

                foreach (var phase in new[] { true, false })
                {
                    foreach (var item in folders)
                    {
                        if (item["type"].ToString() != "dir") continue;
                        string folderName = item["name"].ToString();
                        string jsonUrl = $"https://raw.githubusercontent.com/{repoOwner}/{repoName}/main/{folderName}/settings.json";
                        string iconUrl = $"https://raw.githubusercontent.com/{repoOwner}/{repoName}/main/{folderName}/icon.png";

                        try
                        {
                            string jsonText = await client.GetStringAsync(jsonUrl);
                            JObject data = JObject.Parse(jsonText);
                            bool pinned = data["pinned"]?.ToObject<bool>() ?? false;
                            if (pinned != phase) continue;

                            ModCard card = new ModCard
                            {
                                ModName = data["modName"]?.ToString() ?? folderName,
                                ModDescription = data["modDescription"]?.ToString() ?? "No description",
                                IconUrl = iconUrl,
                                DownloadUrl = data["downloadUrl"]?.ToString() ?? "",
                                Pinned = pinned
                            };
                            card.UpdatePin(pinUrl);
                            card.DownloadClicked += async (s, e) => await DownloadMod(card.DownloadUrl, card.ModName);
                            panel.Controls.Add(card);
                        }
                        catch
                        {
                            ModCard card = new ModCard
                            {
                                ModName = folderName,
                                ModDescription = "No description",
                                IconUrl = "https://via.placeholder.com/128",
                                DownloadUrl = "",
                                Pinned = false
                            };
                            card.UpdatePin(pinUrl);
                            panel.Controls.Add(card);
                        }
                    }
                }

                var sorted = new System.Collections.Generic.List<Control>();
                foreach (Control c in panel.Controls)
                    if (c is ModCard mc && mc.Pinned) sorted.Add(c);
                foreach (Control c in panel.Controls)
                    if (c is ModCard mc && !mc.Pinned) sorted.Add(c);
                panel.Controls.Clear();
                panel.Controls.AddRange(sorted.ToArray());
            }
        }

        private void guna2VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}