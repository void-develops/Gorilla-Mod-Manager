using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GorillaModManager", "settings.json");

        private Color _accentColor = Color.FromArgb(111, 69, 240);
        private string _themeName = "Dark";
        private Color ActiveTab => _accentColor;
        private Color InactiveTab => _currentTheme.ButtonInactive;
        private AppTheme _currentTheme = AppTheme.Dark;

        private List<SbModData> allMods = new List<SbModData>();
        private string GtagDirectory;
        private FlowLayoutPanel flowPanel;
        private FlowLayoutPanel installedFlowPanel;

        private Guna.UI2.WinForms.Guna2TextBox settingsPathBox;
        private Panel accentPreviewPanel;
        private ComboBox themeDropdown;

        private const int LoadoutCount = 5;
        private List<Loadout> _loadouts;
        private int _activeLoadout = 0;
        private Guna.UI2.WinForms.Guna2Button[] _loadoutBtns;
        private Guna.UI2.WinForms.Guna2TextBox _loadoutNameBox;
        private Panel _loadoutEditorPanel;
        private bool _loadoutEditorOpen = false;

        private class Loadout
        {
            public string Name { get; set; }
            public HashSet<string> EnabledMods { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        private class AppTheme
        {
            public Color FormBack { get; set; }
            public Color PanelBack { get; set; }
            public Color CardBack { get; set; }
            public Color FlowBack { get; set; }
            public Color LabelFore { get; set; }
            public Color SubLabelFore { get; set; }
            public Color InputFill { get; set; }
            public Color InputBorder { get; set; }
            public Color ButtonInactive { get; set; }

            public static readonly AppTheme Dark = new AppTheme
            {
                FormBack = Color.FromArgb(24, 24, 24),
                PanelBack = Color.FromArgb(33, 33, 33),
                CardBack = Color.FromArgb(30, 30, 30),
                FlowBack = Color.FromArgb(24, 24, 24),
                LabelFore = Color.White,
                SubLabelFore = Color.FromArgb(180, 180, 180),
                InputFill = Color.FromArgb(21, 21, 21),
                InputBorder = Color.FromArgb(33, 33, 33),
                ButtonInactive = Color.FromArgb(21, 21, 21),
            };
            public static readonly AppTheme Darker = new AppTheme
            {
                FormBack = Color.FromArgb(10, 10, 10),
                PanelBack = Color.FromArgb(18, 18, 18),
                CardBack = Color.FromArgb(15, 15, 15),
                FlowBack = Color.FromArgb(10, 10, 10),
                LabelFore = Color.White,
                SubLabelFore = Color.FromArgb(160, 160, 160),
                InputFill = Color.FromArgb(14, 14, 14),
                InputBorder = Color.FromArgb(25, 25, 25),
                ButtonInactive = Color.FromArgb(14, 14, 14),
            };
            public static readonly AppTheme Light = new AppTheme
            {
                FormBack = Color.FromArgb(235, 235, 240),
                PanelBack = Color.FromArgb(220, 220, 226),
                CardBack = Color.FromArgb(210, 210, 216),
                FlowBack = Color.FromArgb(228, 228, 234),
                LabelFore = Color.FromArgb(30, 30, 30),
                SubLabelFore = Color.FromArgb(80, 80, 90),
                InputFill = Color.FromArgb(245, 245, 250),
                InputBorder = Color.FromArgb(190, 190, 200),
                ButtonInactive = Color.FromArgb(200, 200, 208),
            };
            public static AppTheme FromName(string name)
            {
                switch (name) { case "Darker": return Darker; case "Light": return Light; default: return Dark; }
            }
        }

        public MainApp(string path = "")
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Font;
            GtagDirectory = path;

            _loadouts = Enumerable.Range(1, LoadoutCount)
                .Select(i => new Loadout { Name = "Unnamed" })
                .ToList();

            LoadConfig();

            FilePath.Text = GtagDirectory ?? "";
            BrowseTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            InstalledTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            SettingsTabBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            LaunchGameBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ModView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            InstalledView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SettingsView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            ApplyTheme(_currentTheme);
            ApplyAccent(_accentColor);
            InitializeModView();
            InitializeInstalledView();
            SetActiveTab("browse");
            _ = LoadModsFromApi();
        }

        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(ConfigPath)) return;
                var json = JObject.Parse(File.ReadAllText(ConfigPath));

                string savedPath = json["GtagDirectory"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(savedPath) && Directory.Exists(savedPath))
                    GtagDirectory = savedPath;

                string accent = json["AccentColor"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(accent))
                {
                    var parts = accent.Split(',');
                    if (parts.Length == 3 && int.TryParse(parts[0], out int r) && int.TryParse(parts[1], out int g) && int.TryParse(parts[2], out int b))
                        _accentColor = Color.FromArgb(r, g, b);
                }

                string theme = json["Theme"]?.ToString() ?? "Dark";
                _themeName = theme;
                _currentTheme = AppTheme.FromName(theme);

                _activeLoadout = json["ActiveLoadout"]?.ToObject<int>() ?? 0;
                if (_activeLoadout < 0 || _activeLoadout >= LoadoutCount) _activeLoadout = 0;

                var loadoutsArr = json["Loadouts"] as JArray;
                if (loadoutsArr != null)
                {
                    for (int i = 0; i < Math.Min(loadoutsArr.Count, LoadoutCount); i++)
                    {
                        var lo = loadoutsArr[i];
                        _loadouts[i].Name = lo["Name"]?.ToString() ?? "Unnamed";
                        var mods = lo["Mods"] as JArray;
                        if (mods != null)
                            foreach (var m in mods)
                                _loadouts[i].EnabledMods.Add(m.ToString());
                    }
                }
            }
            catch { }
        }

        private void SaveConfig()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath));
                var loadoutsArr = new JArray();
                foreach (var lo in _loadouts)
                    loadoutsArr.Add(new JObject { ["Name"] = lo.Name, ["Mods"] = new JArray(lo.EnabledMods.ToArray()) });

                var json = new JObject
                {
                    ["GtagDirectory"] = GtagDirectory ?? "",
                    ["AccentColor"] = $"{_accentColor.R},{_accentColor.G},{_accentColor.B}",
                    ["Theme"] = _themeName,
                    ["ActiveLoadout"] = _activeLoadout,
                    ["Loadouts"] = loadoutsArr
                };
                File.WriteAllText(ConfigPath, json.ToString());
            }
            catch { }
        }

        private void ApplyTheme(AppTheme theme)
        {
            _currentTheme = theme;
            this.BackColor = theme.FormBack;
            ModView.BackColor = theme.PanelBack;
            InstalledView.BackColor = theme.PanelBack;
            SettingsView.BackColor = theme.PanelBack;
            if (flowPanel != null) flowPanel.BackColor = theme.FlowBack;
            if (installedFlowPanel != null) installedFlowPanel.BackColor = theme.PanelBack;
            gorillamodmanager.ForeColor = theme.LabelFore;
            undertext.ForeColor = theme.LabelFore;
            FilePath.FillColor = theme.InputFill; FilePath.BorderColor = theme.InputBorder; FilePath.ForeColor = theme.LabelFore;
            SearchBox.FillColor = theme.InputFill; SearchBox.BorderColor = theme.InputBorder; SearchBox.ForeColor = theme.LabelFore;
            FilterDropdown.FillColor = theme.InputFill; FilterDropdown.BorderColor = theme.InputBorder; FilterDropdown.BackColor = theme.InputFill; FilterDropdown.ForeColor = theme.LabelFore;
            opengamepath.FillColor = theme.InputFill; opengamepath.ForeColor = theme.LabelFore;
            guna2Separator1.FillColor = theme.InputBorder;
            BrowseTabBtn.ForeColor = theme.LabelFore; InstalledTabBtn.ForeColor = theme.LabelFore; SettingsTabBtn.ForeColor = theme.LabelFore;
            LaunchGameBtn.ForeColor = Color.White;
            if (settingsPathBox != null) { settingsPathBox.FillColor = theme.InputFill; settingsPathBox.BorderColor = theme.InputBorder; settingsPathBox.ForeColor = theme.LabelFore; }
            foreach (Control c in SettingsView.Controls) ApplyThemeToControl(c, theme);
        }

        private void ApplyThemeToControl(Control c, AppTheme theme)
        {
            if (c is Label lbl && !(c is LinkLabel)) lbl.ForeColor = theme.SubLabelFore;
            foreach (Control child in c.Controls) ApplyThemeToControl(child, theme);
        }

        private void ApplyAccent(Color accent)
        {
            _accentColor = accent;
            if (accentPreviewPanel != null) accentPreviewPanel.BackColor = accent;

            BrowseTabBtn.HoverState.FillColor = accent;
            InstalledTabBtn.HoverState.FillColor = accent;
            SettingsTabBtn.HoverState.FillColor = accent;

            FilePath.FocusedState.BorderColor = accent;
            FilePath.HoverState.BorderColor = accent;
            SearchBox.FocusedState.BorderColor = accent;
            SearchBox.HoverState.BorderColor = accent;
            FilterDropdown.FocusedColor = accent;
            FilterDropdown.FocusedState.BorderColor = accent;

            if (settingsPathBox != null)
            {
                settingsPathBox.FocusedState.BorderColor = accent;
                settingsPathBox.HoverState.BorderColor = accent;
            }
            if (_loadoutNameBox != null)
            {
                _loadoutNameBox.FocusedState.BorderColor = accent;
                _loadoutNameBox.HoverState.BorderColor = accent;
            }

            SetActiveTab(ModView.Visible ? "browse" : InstalledView.Visible ? "installed" : "settings");
        }

        private void SetActiveTab(string tab)
        {
            ModView.Visible = tab == "browse";
            InstalledView.Visible = tab == "installed";
            SettingsView.Visible = tab == "settings";
            BrowseTabBtn.FillColor = tab == "browse" ? _accentColor : _currentTheme.ButtonInactive;
            InstalledTabBtn.FillColor = tab == "installed" ? _accentColor : _currentTheme.ButtonInactive;
            SettingsTabBtn.FillColor = tab == "settings" ? _accentColor : _currentTheme.ButtonInactive;
            SearchBox.Visible = tab == "browse";
            FilterDropdown.Visible = tab == "browse";
        }

        private void InitializeModView()
        {
            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.FlowBack,
                Padding = new Padding(4),
                Margin = new Padding(0)
            };
            ModView.Controls.Add(flowPanel);
        }

        private void InitializeInstalledView()
        {
            InstalledView.Controls.Clear();
            _loadoutEditorOpen = false;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Color.Transparent
            };

            _loadoutBtns = new Guna.UI2.WinForms.Guna2Button[LoadoutCount];
            int btnW = 100, btnH = 30, btnSpacing = 108, startX = 8;

            for (int i = 0; i < LoadoutCount; i++)
            {
                int captured = i;
                var btn = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = _loadouts[i].Name,
                    Size = new Size(btnW, btnH),
                    Location = new Point(startX + i * btnSpacing, 9),
                    FillColor = i == _activeLoadout ? _accentColor : _currentTheme.ButtonInactive,
                    ForeColor = Color.White,
                    BorderRadius = 4,
                    Font = new Font("Poppins", 8F, FontStyle.Bold),
                    Animated = true
                };
                btn.HoverState.FillColor = _accentColor;
                btn.Click += (s, e) =>
                {
                    _activeLoadout = captured;
                    RefreshLoadoutButtons();
                    SaveConfig();
                    OpenLoadoutEditor(captured);
                };
                header.Controls.Add(btn);
                _loadoutBtns[i] = btn;
            }

            installedFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.PanelBack,
                Padding = new Padding(4),
                Margin = new Padding(0)
            };

            _loadoutEditorPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _currentTheme.PanelBack,
                Visible = false
            };

            InstalledView.Controls.Add(_loadoutEditorPanel);
            InstalledView.Controls.Add(installedFlowPanel);
            InstalledView.Controls.Add(header);
        }

        private void OpenLoadoutEditor(int loadoutIndex)
        {
            _loadoutEditorOpen = true;
            _loadoutEditorPanel.Visible = true;
            installedFlowPanel.Visible = false;
            _loadoutEditorPanel.Controls.Clear();

            var loadout = _loadouts[loadoutIndex];

            var editorHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Color.Transparent
            };

            var backBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "← Back",
                Location = new Point(8, 9),
                Size = new Size(75, 30),
                FillColor = _currentTheme.ButtonInactive,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 4,
                Font = new Font("Poppins", 8F, FontStyle.Bold),
                Animated = true
            };
            backBtn.HoverState.FillColor = _accentColor;
            backBtn.Click += (s, e) =>
            {
                _loadoutEditorOpen = false;
                _loadoutEditorPanel.Visible = false;
                installedFlowPanel.Visible = true;
            };
            editorHeader.Controls.Add(backBtn);

            var nameLabel = new Label
            {
                Text = "Name:",
                ForeColor = _currentTheme.SubLabelFore,
                Font = new Font("Poppins", 8.5F),
                Location = new Point(94, 15),
                Size = new Size(54, 20),
                AutoSize = false,
                BackColor = Color.Transparent
            };
            editorHeader.Controls.Add(nameLabel);

            _loadoutNameBox = new Guna.UI2.WinForms.Guna2TextBox
            {
                Location = new Point(152, 10),
                Size = new Size(180, 28),
                FillColor = _currentTheme.InputFill,
                BorderColor = _currentTheme.InputBorder,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 4,
                Font = new Font("Poppins", 8.5F),
                Text = loadout.Name,
                PlaceholderText = "Loadout name..."
            };
            _loadoutNameBox.FocusedState.BorderColor = _accentColor;
            _loadoutNameBox.TextChanged += (s, e) =>
            {
                _loadouts[_activeLoadout].Name = _loadoutNameBox.Text;
                _loadoutBtns[_activeLoadout].Text = _loadoutNameBox.Text;
                SaveConfig();
            };
            editorHeader.Controls.Add(_loadoutNameBox);

            var applyBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Apply Loadout",
                Location = new Point(342, 10),
                Size = new Size(120, 28),
                FillColor = _accentColor,
                ForeColor = Color.White,
                BorderRadius = 4,
                Font = new Font("Poppins", 8F, FontStyle.Bold),
                Animated = true
            };
            applyBtn.HoverState.FillColor = ControlPaint.Light(_accentColor, 0.2f);
            applyBtn.Click += (s, e) => ApplyLoadout(_activeLoadout);
            editorHeader.Controls.Add(applyBtn);

            var modFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.PanelBack,
                Padding = new Padding(4)
            };

            if (!string.IsNullOrEmpty(GtagDirectory) && Directory.Exists(GtagDirectory))
            {
                string pluginsRoot = Path.Combine(GtagDirectory, "BepInEx", "plugins");
                string disabledRoot = Path.Combine(pluginsRoot, "disabled");

                if (Directory.Exists(pluginsRoot))
                {
                    var allDlls = Directory.GetFiles(pluginsRoot, "*.dll", SearchOption.AllDirectories)
                        .Where(f => !f.StartsWith(disabledRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        .Concat(Directory.Exists(disabledRoot)
                            ? Directory.GetFiles(disabledRoot, "*.disabled", SearchOption.AllDirectories)
                            : new string[0])
                        .ToArray();

                    foreach (var dll in allDlls)
                    {
                        bool isDisabled = dll.EndsWith(".disabled", StringComparison.OrdinalIgnoreCase);
                        string fileName = isDisabled
                            ? Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(dll)) + ".dll"
                            : Path.GetFileName(dll);
                        bool included = loadout.EnabledMods.Contains(fileName);

                        modFlow.Controls.Add(BuildLoadoutModCard(fileName, included, loadoutIndex));
                    }

                    if (!allDlls.Any())
                        modFlow.Controls.Add(new Label { Text = "No mods installed.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
                }
                else
                {
                    modFlow.Controls.Add(new Label { Text = "Install BepInEx first.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
                }
            }
            else
            {
                modFlow.Controls.Add(new Label { Text = "No Gorilla Tag folder set.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
            }

            _loadoutEditorPanel.Controls.Add(modFlow);
            _loadoutEditorPanel.Controls.Add(editorHeader);
        }

        private Panel BuildLoadoutModCard(string fileName, bool included, int loadoutIndex)
        {
            string modName = Path.GetFileNameWithoutExtension(fileName);
            Color cardBg = _currentTheme.CardBack;
            Color borderFg = Color.FromArgb(55, 55, 55);

            var card = new Panel { Size = new Size(178, 90), BackColor = cardBg, Margin = new Padding(4) };

            card.Controls.Add(new Label
            {
                Text = modName,
                ForeColor = _currentTheme.LabelFore,
                Font = new Font("Poppins", 8F, FontStyle.Bold),
                Location = new Point(6, 10),
                Size = new Size(166, 18),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                BackColor = Color.Transparent
            });

            var includeBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = included ? "✓ Included" : "Include",
                FillColor = included ? _accentColor : _currentTheme.ButtonInactive,
                ForeColor = Color.White,
                Font = new Font("Poppins", 8F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(166, 28),
                Location = new Point(6, 50),
                Animated = true
            };
            includeBtn.HoverState.FillColor = included ? ControlPaint.Light(_accentColor, 0.2f) : _accentColor;

            string capturedFileName = fileName;
            bool capturedIncluded = included;

            includeBtn.Click += (s, e) =>
            {
                capturedIncluded = !capturedIncluded;
                if (capturedIncluded)
                {
                    _loadouts[loadoutIndex].EnabledMods.Add(capturedFileName);
                    includeBtn.Text = "✓ Included";
                    includeBtn.FillColor = _accentColor;
                    includeBtn.HoverState.FillColor = ControlPaint.Light(_accentColor, 0.2f);
                }
                else
                {
                    _loadouts[loadoutIndex].EnabledMods.Remove(capturedFileName);
                    includeBtn.Text = "Include";
                    includeBtn.FillColor = _currentTheme.ButtonInactive;
                    includeBtn.HoverState.FillColor = _accentColor;
                }
                SaveConfig();
            };

            card.Controls.Add(includeBtn);

            Color capturedBorder = borderFg;
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(capturedBorder, 1f))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }

        private void RefreshLoadoutButtons()
        {
            if (_loadoutBtns == null) return;
            for (int i = 0; i < LoadoutCount; i++)
                _loadoutBtns[i].FillColor = i == _activeLoadout ? _accentColor : _currentTheme.ButtonInactive;
        }

        private void ApplyLoadout(int loadoutIndex)
        {
            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory)) return;

            string pluginsRoot = Path.Combine(GtagDirectory, "BepInEx", "plugins");
            string disabledRoot = Path.Combine(pluginsRoot, "disabled");
            if (!Directory.Exists(pluginsRoot)) return;
            Directory.CreateDirectory(disabledRoot);

            var loadout = _loadouts[loadoutIndex];

            var enabledDlls = Directory.GetFiles(pluginsRoot, "*.dll", SearchOption.AllDirectories)
                .Where(f => !f.StartsWith(disabledRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)).ToArray();
            var disabledDlls = Directory.GetFiles(disabledRoot, "*.disabled", SearchOption.AllDirectories);

            foreach (var dll in enabledDlls)
            {
                string dllName = Path.GetFileName(dll);
                if (!loadout.EnabledMods.Contains(dllName))
                {
                    string rel = dll.Substring(pluginsRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                    string subDir = Path.GetDirectoryName(rel);
                    string destDir = string.IsNullOrEmpty(subDir) ? disabledRoot : Path.Combine(disabledRoot, subDir);
                    Directory.CreateDirectory(destDir);
                    try { File.Move(dll, Path.Combine(destDir, dllName + ".disabled")); } catch { }
                }
            }

            foreach (var dis in disabledDlls)
            {
                string dllName = Path.GetFileNameWithoutExtension(dis);
                if (loadout.EnabledMods.Contains(dllName))
                {
                    string rel = dis.Substring(disabledRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                    string subDir = Path.GetDirectoryName(rel);
                    string destDir = string.IsNullOrEmpty(subDir) ? pluginsRoot : Path.Combine(pluginsRoot, subDir);
                    Directory.CreateDirectory(destDir);
                    try { File.Move(dis, Path.Combine(destDir, dllName)); } catch { }
                }
            }

            LoadInstalledMods();
        }

        private void LaunchGame()
        {
            string steamExe = FindSteamExe();
            if (steamExe != null) { Process.Start(new ProcessStartInfo { FileName = steamExe, Arguments = $"-applaunch {GorillaTagSteamAppId}", UseShellExecute = true }); return; }
            Process.Start(new ProcessStartInfo { FileName = $"steam://rungameid/{GorillaTagSteamAppId}", UseShellExecute = true });
        }

        private void LoadInstalledMods()
        {
            installedFlowPanel.Controls.Clear();

            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory))
            {
                installedFlowPanel.Controls.Add(new Label { Text = "No Gorilla Tag folder set.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
                return;
            }

            string pluginsPath = Path.Combine(GtagDirectory, "BepInEx", "plugins");
            string disabledPath = Path.Combine(GtagDirectory, "BepInEx", "plugins", "disabled");

            if (!Directory.Exists(pluginsPath))
            {
                installedFlowPanel.Controls.Add(new Label { Text = "No mods installed. Install BepInEx first.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
                return;
            }

            var enabledDlls = Directory.GetFiles(pluginsPath, "*.dll", SearchOption.AllDirectories)
                .Where(f => !f.StartsWith(disabledPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)).ToArray();
            var disabledDlls = Directory.Exists(disabledPath)
                ? Directory.GetFiles(disabledPath, "*.disabled", SearchOption.AllDirectories)
                : new string[0];

            if (enabledDlls.Length == 0 && disabledDlls.Length == 0)
            {
                installedFlowPanel.Controls.Add(new Label { Text = "No mods installed.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
                return;
            }

            foreach (var dll in enabledDlls) installedFlowPanel.Controls.Add(BuildInstalledCard(dll, false));
            foreach (var dll in disabledDlls) installedFlowPanel.Controls.Add(BuildInstalledCard(dll, true));
        }

        private Panel BuildInstalledCard(string filePath, bool isDisabled)
        {
            string modName = isDisabled
                ? Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath))
                : Path.GetFileNameWithoutExtension(filePath);

            long fileSize = new FileInfo(filePath).Length;
            string sizeText = fileSize >= 1024 * 1024
                ? $"{fileSize / (1024f * 1024f):0.0} MB"
                : $"{fileSize / 1024f:0.0} KB";

            Color cardBg = isDisabled ? Color.FromArgb(22, 22, 22) : _currentTheme.CardBack;
            Color nameFg = isDisabled ? Color.FromArgb(80, 80, 80) : _currentTheme.LabelFore;
            Color borderFg = isDisabled ? Color.FromArgb(40, 40, 40) : Color.FromArgb(55, 55, 55);

            var card = new Panel { Size = new Size(178, 110), BackColor = cardBg, Margin = new Padding(4) };

            card.Controls.Add(new Label { Text = modName, ForeColor = nameFg, Font = new Font("Poppins", 8F, FontStyle.Bold), Location = new Point(6, 10), Size = new Size(166, 18), TextAlign = ContentAlignment.MiddleCenter, AutoEllipsis = true, BackColor = Color.Transparent });
            card.Controls.Add(new Label { Text = isDisabled ? sizeText + " · disabled" : sizeText, ForeColor = isDisabled ? Color.FromArgb(70, 70, 70) : _currentTheme.SubLabelFore, Font = new Font("Poppins", 7F), Location = new Point(6, 30), Size = new Size(166, 14), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Transparent });

            var toggleBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = isDisabled ? "Enable" : "Disable",
                FillColor = isDisabled ? Color.FromArgb(50, 50, 50) : Color.FromArgb(80, 40, 0),
                ForeColor = isDisabled ? Color.FromArgb(160, 160, 160) : Color.FromArgb(255, 140, 40),
                Font = new Font("Poppins SemiBold", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(isDisabled ? 166 : 78, 26),
                Location = new Point(6, 52),
                Animated = true
            };
            toggleBtn.HoverState.FillColor = isDisabled ? Color.FromArgb(70, 70, 70) : Color.FromArgb(110, 55, 0);

            var uninstallBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Uninstall",
                FillColor = Color.FromArgb(120, 20, 20),
                ForeColor = Color.White,
                Font = new Font("Poppins SemiBold", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(82, 26),
                Location = new Point(90, 52),
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
                    string pluginsRoot = Path.Combine(GtagDirectory, "BepInEx", "plugins");
                    string disabledRoot = Path.Combine(pluginsRoot, "disabled");
                    Directory.CreateDirectory(disabledRoot);
                    if (capturedDisabled)
                    {
                        string relativePath = capturedPath.Substring(disabledRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                        string dllName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(relativePath));
                        string subFolder = Path.GetDirectoryName(relativePath);
                        string destDir = string.IsNullOrEmpty(subFolder) ? pluginsRoot : Path.Combine(pluginsRoot, subFolder);
                        Directory.CreateDirectory(destDir);
                        File.Move(capturedPath, Path.Combine(destDir, dllName + ".dll"));
                    }
                    else
                    {
                        string relativePath = capturedPath.Substring(pluginsRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                        string subFolder = Path.GetDirectoryName(relativePath);
                        string destDir = string.IsNullOrEmpty(subFolder) ? disabledRoot : Path.Combine(disabledRoot, subFolder);
                        Directory.CreateDirectory(destDir);
                        File.Move(capturedPath, Path.Combine(destDir, Path.GetFileName(capturedPath) + ".disabled"));
                    }
                    LoadInstalledMods();
                }
                catch (Exception ex) { MessageBox.Show("Failed to toggle mod: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            uninstallBtn.Click += (s, e) =>
            {
                if (MessageBox.Show($"Uninstall {modName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                try { File.Delete(capturedPath); LoadInstalledMods(); }
                catch (Exception ex) { MessageBox.Show("Failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };

            card.Controls.Add(toggleBtn);
            if (!isDisabled) card.Controls.Add(uninstallBtn);

            Color capturedBorder = borderFg;
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(capturedBorder, 1f))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }

        private void LaunchGameBtn_Click(object sender, EventArgs e)
        {
            ApplyLoadout(_activeLoadout);
            LaunchGame();
        }

        private string FindSteamExe()
        {
            foreach (var p in new[] { @"C:\Program Files (x86)\Steam\steam.exe", @"C:\Program Files\Steam\steam.exe" })
                if (File.Exists(p)) return p;
            try
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (key != null)
                    {
                        string sp = key.GetValue("SteamExe") as string;
                        if (!string.IsNullOrEmpty(sp) && File.Exists(sp)) return sp;
                        string sd = key.GetValue("SteamPath") as string;
                        if (!string.IsNullOrEmpty(sd)) { string exe = Path.Combine(sd.Replace("/", "\\"), "steam.exe"); if (File.Exists(exe)) return exe; }
                    }
                }
            }
            catch { }
            return null;
        }

        private void BrowseTabBtn_Click(object sender, EventArgs e) => SetActiveTab("browse");
        private void InstalledTabBtn_Click(object sender, EventArgs e) { SetActiveTab("installed"); LoadInstalledMods(); }
        private void SettingsTabBtn_Click(object sender, EventArgs e) => SetActiveTab("settings");
        private void Minimize_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;
        private void Close_Click(object sender, EventArgs e) => Application.Exit();
        private void DashboardBtn_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo { FileName = "https://sevvy-wevvy.com/mods/sb/dashboard/", UseShellExecute = true });
        private void MainApp_Load(object sender, EventArgs e) { InitializeSettingsView(); }
        private void Github_Click(object sender, EventArgs e) => Process.Start(new ProcessStartInfo { FileName = "https://github.com/void-develops?tab=repositories", UseShellExecute = true });
        private void SearchBox_TextChanged(object sender, EventArgs e) => ApplyFilter();
        private void FilterDropdown_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilter();

        private void opengamepath_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Title = "Select Gorilla Tag Executable", Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*", FilterIndex = 1 })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                string fn = Path.GetFileName(dlg.FileName);
                if (fn.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) || fn.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                {
                    GtagDirectory = Path.GetDirectoryName(dlg.FileName);
                    FilePath.Text = GtagDirectory;
                    if (settingsPathBox != null) settingsPathBox.Text = GtagDirectory;
                }
                else
                    MessageBox.Show("Sorry! That isn't Gorilla Tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSettingsView()
        {
            SettingsView.Controls.Clear();
            int rowH = 58, labelX = 24, controlX = 190, ctrlH = 32;
            int y = 28;
            var panel = SettingsView;

            Label MakeLabel(string text, int top) => new Label
            {
                Text = text,
                ForeColor = _currentTheme.SubLabelFore,
                Font = new Font("Poppins", 9F),
                Location = new Point(labelX, top + (ctrlH / 2) - 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            // Game Path
            panel.Controls.Add(MakeLabel("Game Path", y));
            settingsPathBox = new Guna.UI2.WinForms.Guna2TextBox
            {
                Location = new Point(controlX, y),
                Size = new Size(270, ctrlH),
                FillColor = _currentTheme.InputFill,
                BorderColor = _currentTheme.InputBorder,
                ForeColor = _currentTheme.LabelFore,
                Font = new Font("Poppins", 8.5F),
                BorderRadius = 4,
                Text = GtagDirectory ?? ""
            };
            settingsPathBox.FocusedState.BorderColor = _accentColor;

            var browsePathBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "...",
                Location = new Point(controlX + 276, y),
                Size = new Size(ctrlH, ctrlH),
                FillColor = _currentTheme.InputFill,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 4,
                Font = new Font("Poppins", 9F, FontStyle.Bold)
            };
            browsePathBtn.HoverState.FillColor = _currentTheme.PanelBack;
            browsePathBtn.Click += (s, e) =>
            {
                using (var dlg = new OpenFileDialog { Title = "Select Gorilla Tag Executable", Filter = "Executable Files (*.exe)|*.exe" })
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    string fn = Path.GetFileName(dlg.FileName);
                    if (fn.Equals("Gorilla Tag.exe", StringComparison.OrdinalIgnoreCase) || fn.Equals("GorillaTag.exe", StringComparison.OrdinalIgnoreCase))
                        settingsPathBox.Text = Path.GetDirectoryName(dlg.FileName);
                    else
                        MessageBox.Show("That isn't Gorilla Tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var openFolderBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Open Folder",
                Location = new Point(controlX + 276 + ctrlH + 6, y),
                Size = new Size(100, ctrlH),
                FillColor = _currentTheme.InputFill,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 4,
                Font = new Font("Poppins", 8F),
                Animated = true
            };
            openFolderBtn.HoverState.FillColor = _accentColor;
            openFolderBtn.Click += (s, e) =>
            {
                string fp = settingsPathBox.Text.Trim();
                if (string.IsNullOrEmpty(fp) || !Directory.Exists(fp))
                { MessageBox.Show("Game folder not set or doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = fp, UseShellExecute = true });
            };
            panel.Controls.Add(settingsPathBox);
            panel.Controls.Add(browsePathBtn);
            panel.Controls.Add(openFolderBtn);
            y += rowH;

            // Theme
            panel.Controls.Add(MakeLabel("Theme", y));
            themeDropdown = new ComboBox
            {
                Location = new Point(controlX, y + 2),
                Size = new Size(160, ctrlH),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = _currentTheme.InputFill,
                ForeColor = _currentTheme.LabelFore,
                Font = new Font("Poppins", 9F)
            };
            themeDropdown.Items.AddRange(new object[] { "Dark", "Darker", "Light" });
            themeDropdown.SelectedItem = _themeName;
            themeDropdown.SelectedIndexChanged += (s, e) =>
            {
                string sel = themeDropdown.SelectedItem?.ToString() ?? "Dark";
                _themeName = sel;
                _currentTheme = AppTheme.FromName(sel);
                ApplyTheme(_currentTheme);
                ApplyAccent(_accentColor);
            };
            panel.Controls.Add(themeDropdown);
            y += rowH;

            // Accent Color
            panel.Controls.Add(MakeLabel("Accent Color", y));
            var presets = new[]
            {
                ("Purple", Color.FromArgb(111, 69, 240)),
                ("Blue",   Color.FromArgb(56, 139, 253)),
                ("Green",  Color.FromArgb(39, 174, 96)),
                ("Red",    Color.FromArgb(220, 50, 50)),
                ("Pink",   Color.FromArgb(220, 80, 160)),
                ("Orange", Color.FromArgb(230, 120, 30))
            };
            int px = controlX;
            foreach (var (name, col) in presets)
            {
                Color cc = col;
                var pb = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = name,
                    Location = new Point(px, y),
                    Size = new Size(64, ctrlH),
                    FillColor = col,
                    ForeColor = Color.White,
                    BorderRadius = 4,
                    Font = new Font("Poppins", 7.5F, FontStyle.Bold)
                };
                pb.HoverState.FillColor = ControlPaint.Light(col, 0.2f);
                pb.Click += (s, e) => ApplyAccent(cc);
                panel.Controls.Add(pb);
                px += 70;
            }
            y += rowH;

            // Custom Color
            panel.Controls.Add(MakeLabel("Custom Color", y));
            var customColorBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Pick Color...",
                Location = new Point(controlX, y),
                Size = new Size(120, ctrlH),
                FillColor = _currentTheme.InputFill,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 4,
                Font = new Font("Poppins", 8.5F)
            };
            customColorBtn.HoverState.FillColor = _currentTheme.PanelBack;
            customColorBtn.Click += (s, e) =>
            {
                using (var dlg = new ColorDialog { Color = _accentColor, FullOpen = true })
                    if (dlg.ShowDialog() == DialogResult.OK) ApplyAccent(dlg.Color);
            };
            accentPreviewPanel = new Panel
            {
                Location = new Point(controlX + 126, y + (ctrlH / 2) - 11),
                Size = new Size(22, 22),
                BackColor = _accentColor
            };
            panel.Controls.Add(customColorBtn);
            panel.Controls.Add(accentPreviewPanel);
            y += rowH;

            // Separator
            panel.Controls.Add(new Panel
            {
                Location = new Point(labelX, y),
                Size = new Size(600, 1),
                BackColor = _currentTheme.InputBorder
            });
            y += 20;

            // Save
            var saveBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Save Settings",
                Location = new Point(controlX, y),
                Size = new Size(150, 36),
                FillColor = _accentColor,
                ForeColor = Color.White,
                BorderRadius = 4,
                Font = new Font("Poppins", 9F, FontStyle.Bold),
                Animated = true
            };
            saveBtn.HoverState.FillColor = ControlPaint.Light(_accentColor, 0.2f);
            saveBtn.Click += (s, e) =>
            {
                string newPath = settingsPathBox.Text.Trim();
                if (!string.IsNullOrEmpty(newPath) && Directory.Exists(newPath)) { GtagDirectory = newPath; FilePath.Text = GtagDirectory; }
                else if (!string.IsNullOrEmpty(newPath)) { MessageBox.Show("The path doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                saveBtn.FillColor = Color.FromArgb(39, 174, 96);
                SaveConfig();
                Task.Delay(1500).ContinueWith(_ => { if (saveBtn.IsHandleCreated) saveBtn.Invoke((Action)(() => saveBtn.FillColor = _accentColor)); });
            };
            panel.Controls.Add(saveBtn);
        }
        private async Task LoadModsFromApi()
        {
            flowPanel.Controls.Clear();
            flowPanel.Controls.Add(new Label { Text = "Loading mods...", ForeColor = Color.FromArgb(160, 140, 200), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) });
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
                        allMods.Add(new SbModData { Id = m["id"]?.ToObject<int>() ?? 0, Name = m["name"]?.ToString() ?? "", Description = m["description"]?.ToString() ?? "", Author = m["author_username"]?.ToString() ?? "", RepoUrl = (m["repo_url"]?.ToString() ?? "").Replace("\\/", "/"), DllName = m["dll_name"]?.ToString() ?? "", ImageUrl = (m["image_url"]?.ToString() ?? "").Replace("\\/", "/"), Upvotes = m["upvotes"]?.ToObject<int>() ?? 0, IsVerified = m["is_verified"]?.ToObject<int>() == 1, IsFeatured = m["is_featured"]?.ToObject<int>() == 1 });
                }
            }
            catch (Exception ex) { flowPanel.Controls.Clear(); flowPanel.Controls.Add(new Label { Text = "Failed to load: " + ex.Message, ForeColor = Color.FromArgb(248, 113, 113), Font = new Font("Poppins", 9F), AutoSize = true, Margin = new Padding(10) }); return; }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (allMods == null || flowPanel == null) return;
            string search = SearchBox?.Text?.Trim().ToLowerInvariant() ?? "";
            string filter = FilterDropdown?.SelectedItem?.ToString() ?? "All";
            IEnumerable<SbModData> filtered = allMods;
            if (!string.IsNullOrEmpty(search)) filtered = filtered.Where(m => m.Name.ToLowerInvariant().Contains(search) || m.Author.ToLowerInvariant().Contains(search) || m.Description.ToLowerInvariant().Contains(search));
            if (filter == "Featured") filtered = filtered.Where(m => m.IsFeatured);
            else if (filter == "Verified") filtered = filtered.Where(m => m.IsVerified && !m.IsFeatured);
            else if (filter == "Unverified") filtered = filtered.Where(m => !m.IsVerified && !m.IsFeatured);
            var sorted = filtered.OrderByDescending(m => m.IsFeatured ? 2 : m.IsVerified ? 1 : 0).ThenByDescending(m => m.Upvotes).ToList();
            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();
            if (!sorted.Any()) { flowPanel.Controls.Add(new Label { Text = "No mods found.", ForeColor = Color.FromArgb(120, 100, 160), Font = new Font("Poppins", 10F), AutoSize = true, Margin = new Padding(14) }); flowPanel.ResumeLayout(); return; }
            foreach (var mod in sorted) flowPanel.Controls.Add(new ModCard(mod, GtagDirectory));
            flowPanel.ResumeLayout();
        }

        private async void InstallBepinex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GtagDirectory) || !Directory.Exists(GtagDirectory)) { MessageBox.Show("Please set your Gorilla Tag directory first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string bepinexUrl = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip";
            string zipPath = Path.Combine(Path.GetTempPath(), "BepInEx.zip");
            var btn = (Guna.UI2.WinForms.Guna2Button)sender;
            btn.Enabled = false;
            try
            {
                using (var client = new HttpClient()) { client.DefaultRequestHeaders.Add("User-Agent", "GorillaModManager"); File.WriteAllBytes(zipPath, await client.GetByteArrayAsync(bepinexUrl)); }
                string destDir = GtagDirectory;
                await Task.Run(() => { using (var archive = new ZipArchive(File.OpenRead(zipPath), ZipArchiveMode.Read)) foreach (var entry in archive.Entries) { string destPath = Path.Combine(destDir, entry.FullName); if (string.IsNullOrEmpty(entry.Name)) Directory.CreateDirectory(destPath); else { Directory.CreateDirectory(Path.GetDirectoryName(destPath)); entry.ExtractToFile(destPath, true); } } });
                File.Delete(zipPath);
                btn.FillColor = Color.FromArgb(39, 174, 96); btn.HoverState.FillColor = Color.FromArgb(39, 174, 96); btn.BorderColor = Color.FromArgb(39, 174, 96); btn.Enabled = true;
                MessageBox.Show("BepInEx 5.4.23.5 installed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { btn.Enabled = true; MessageBox.Show("Failed to install BepInEx: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ModView_Paint(object sender, PaintEventArgs e) { }
        private void InstalledView_Paint(object sender, PaintEventArgs e) { }
        private void SettingsView_Paint(object sender, PaintEventArgs e) { }
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