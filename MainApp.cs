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
        private FlowLayoutPanel homeFlowPanel;

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
        private Panel _modDetailOverlay;
        private Panel _overlayOwner;

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

            var tabTip = new ToolTip { AutoPopDelay = 3000, InitialDelay = 400, ReshowDelay = 200, ShowAlways = true };
            tabTip.SetToolTip(BrowseTabBtn, "Browse Mods");
            tabTip.SetToolTip(InstalledTabBtn, "Library");
            tabTip.SetToolTip(SettingsTabBtn, "Settings");
            tabTip.SetToolTip(LaunchGameBtn, "Launch Game");
            if (HomeTabBtn != null) tabTip.SetToolTip(HomeTabBtn, "Home");

            InitializeModView();
            InitializeInstalledView();
            InitializeHomeView();
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
            if (HomeView != null) HomeView.BackColor = theme.PanelBack;
            if (flowPanel != null) flowPanel.BackColor = theme.FlowBack;
            if (installedFlowPanel != null) installedFlowPanel.BackColor = theme.PanelBack;
            if (homeFlowPanel != null) homeFlowPanel.BackColor = theme.FlowBack;
            gorillamodmanager.ForeColor = theme.LabelFore;
            undertext.ForeColor = theme.LabelFore;
            FilePath.FillColor = theme.InputFill; FilePath.BorderColor = theme.InputBorder; FilePath.ForeColor = theme.LabelFore;
            SearchBox.FillColor = theme.InputFill; SearchBox.BorderColor = theme.InputBorder; SearchBox.ForeColor = theme.LabelFore;
            FilterDropdown.FillColor = theme.InputFill; FilterDropdown.BorderColor = theme.InputBorder; FilterDropdown.BackColor = theme.InputFill; FilterDropdown.ForeColor = theme.LabelFore;
            opengamepath.FillColor = theme.InputFill; opengamepath.ForeColor = theme.LabelFore;
            opengamepath.HoverState.FillColor = theme.PanelBack; opengamepath.HoverState.BorderColor = theme.InputBorder;
            DashboardBtn.FillColor = theme.InputFill; DashboardBtn.ForeColor = theme.LabelFore;
            guna2Separator1.FillColor = theme.InputBorder;

            foreach (var btn in new[] { BrowseTabBtn, InstalledTabBtn, SettingsTabBtn })
            {
                btn.ForeColor = theme.LabelFore;
                btn.BorderColor = theme.InputBorder;
                btn.HoverState.BorderColor = theme.InputBorder;
            }
            if (HomeTabBtn != null)
            {
                HomeTabBtn.ForeColor = theme.LabelFore;
                HomeTabBtn.BorderColor = theme.InputBorder;
                HomeTabBtn.HoverState.BorderColor = theme.InputBorder;
            }
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
            if (accentPreviewPanel != null) { accentPreviewPanel.BackColor = accent; accentPreviewPanel.Invalidate(); }

            foreach (var btn in new[] { BrowseTabBtn, InstalledTabBtn, SettingsTabBtn })
            {
                btn.HoverState.FillColor = accent;
                btn.HoverState.BorderColor = _currentTheme.InputBorder;
            }
            if (HomeTabBtn != null)
            {
                HomeTabBtn.HoverState.FillColor = accent;
                HomeTabBtn.HoverState.BorderColor = _currentTheme.InputBorder;
            }
            DashboardBtn.HoverState.FillColor = accent;
            DashboardBtn.HoverState.BorderColor = accent;

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

            SetActiveTab(ModView.Visible ? "browse" : InstalledView.Visible ? "installed" : SettingsView.Visible ? "settings" : "home");
        }

        private void SetActiveTab(string tab)
        {
            CloseModDetail();
            ModView.Visible = tab == "browse";
            InstalledView.Visible = tab == "installed";
            SettingsView.Visible = tab == "settings";
            if (HomeView != null) HomeView.Visible = tab == "home";
            BrowseTabBtn.FillColor = tab == "browse" ? _accentColor : _currentTheme.ButtonInactive;
            InstalledTabBtn.FillColor = tab == "installed" ? _accentColor : _currentTheme.ButtonInactive;
            SettingsTabBtn.FillColor = tab == "settings" ? _accentColor : _currentTheme.ButtonInactive;
            if (HomeTabBtn != null) HomeTabBtn.FillColor = tab == "home" ? _accentColor : _currentTheme.ButtonInactive;
            BrowseTabBtn.BorderColor = _currentTheme.InputBorder;
            InstalledTabBtn.BorderColor = _currentTheme.InputBorder;
            SettingsTabBtn.BorderColor = _currentTheme.InputBorder;
            if (HomeTabBtn != null) HomeTabBtn.BorderColor = _currentTheme.InputBorder;
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

        private void InitializeHomeView()
        {
            if (HomeView == null) return;
            HomeView.Controls.Clear();

            var titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = _currentTheme.PanelBack
            };

            titlePanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(_currentTheme.InputBorder, 1f))
                    e.Graphics.DrawLine(pen, 0, titlePanel.Height - 1, titlePanel.Width, titlePanel.Height - 1);
            };

            var titleRow = new Panel
            {
                Location = new Point(0, 10),
                Height = 26,
                Dock = DockStyle.None,
                BackColor = Color.Transparent
            };
            titlePanel.Controls.Add(titleRow);
            titlePanel.Resize += (s, e) => titleRow.Width = titlePanel.Width;
            titleRow.Width = titlePanel.Width > 0 ? titlePanel.Width : 800;

            var starLabel = new Label
            {
                Text = "✦",
                ForeColor = _accentColor,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            titleRow.Controls.Add(starLabel);
            starLabel.Location = new Point((titleRow.Width / 2) - 100, 2);
            titlePanel.Resize += (s, e) => starLabel.Location = new Point((titlePanel.Width / 2) - 100, 2);

            var titleLabel = new Label
            {
                Text = "Featured Mods",
                ForeColor = _currentTheme.LabelFore,
                Font = new Font("Poppins", 12F, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            titleRow.Controls.Add(titleLabel);
            titleLabel.Location = new Point((titleRow.Width / 2) - 76, 1);
            titlePanel.Resize += (s, e) => titleLabel.Location = new Point((titlePanel.Width / 2) - 76, 1);

            var subLabel = new Label
            {
                Text = "hand-picked by the community",
                ForeColor = _currentTheme.SubLabelFore,
                Font = new Font("Poppins", 7.5F),
                Dock = DockStyle.None,
                AutoSize = true,
                BackColor = Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            titlePanel.Controls.Add(subLabel);
            subLabel.Location = new Point((titlePanel.Width - subLabel.PreferredWidth) / 2, 40);
            titlePanel.Resize += (s, e) =>
            {
                subLabel.Location = new Point((titlePanel.Width - subLabel.PreferredWidth) / 2, 40);
            };

            homeFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = _currentTheme.FlowBack,
                Padding = new Padding(8),
                Margin = new Padding(0)
            };
            homeFlowPanel.HorizontalScroll.Visible = false;

            HomeView.Controls.Add(homeFlowPanel);
            HomeView.Controls.Add(titlePanel);

            PopulateHomeView();
        }

        private void PopulateHomeView()
        {
            if (homeFlowPanel == null) return;
            homeFlowPanel.SuspendLayout();
            homeFlowPanel.Controls.Clear();

            var featured = allMods?.Where(m => m.IsFeatured).OrderByDescending(m => m.Upvotes).ToList();

            if (featured == null || !featured.Any())
            {
                homeFlowPanel.Controls.Add(new Label
                {
                    Text = allMods == null || allMods.Count == 0 ? "Loading..." : "No featured mods right now.",
                    ForeColor = Color.FromArgb(120, 100, 160),
                    Font = new Font("Poppins", 10F),
                    AutoSize = true,
                    Margin = new Padding(14)
                });
                homeFlowPanel.ResumeLayout();
                return;
            }

            foreach (var mod in featured)
            {
                var card = new ModCard(mod, GtagDirectory);
                var capturedMod = mod;
                WireCardClick(card, () => ShowModDetail(capturedMod));
                homeFlowPanel.Controls.Add(card);
            }

            homeFlowPanel.ResumeLayout();
        }

        private static void WireCardClick(Control root, Action handler)
        {
            root.Click += (s, e) => handler();
            root.Cursor = Cursors.Hand;
            foreach (Control child in root.Controls)
            {
                if (child is Guna.UI2.WinForms.Guna2Button) continue;
                WireCardClick(child, handler);
            }
        }

        private void ShowModDetail(SbModData mod)
        {
            CloseModDetail();

            Panel targetPanel = (HomeView != null && HomeView.Visible && homeFlowPanel != null)
                ? homeFlowPanel
                : flowPanel;

            _overlayOwner = targetPanel;

            _modDetailOverlay = new Panel
            {
                Location = new Point(0, 0),
                Size = targetPanel.ClientSize,
                BackColor = Color.FromArgb(210, 10, 10, 16)
            };

            targetPanel.Resize += SyncOverlaySize;

            var fvCard = new FullViewModCard(mod, GtagDirectory);

            fvCard.downloadButton.FillColor = _accentColor;
            fvCard.downloadButton.HoverState.FillColor = ControlPaint.Light(_accentColor, 0.2f);

            fvCard.ViewRepoBtn.FillColor = _currentTheme.ButtonInactive;
            fvCard.ViewRepoBtn.ForeColor = _currentTheme.LabelFore;
            fvCard.ViewRepoBtn.HoverState.FillColor = _accentColor;

            fvCard.ViewRepoBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(mod.RepoUrl))
                    Process.Start(new ProcessStartInfo { FileName = mod.RepoUrl, UseShellExecute = true });
            };

            fvCard.ExitBtn.Click += (s, e) => CloseModDetail();

            void CentreAndScaleCard()
            {
                if (fvCard.IsDisposed || _modDetailOverlay == null) return;

                bool isFullscreen = this.WindowState == FormWindowState.Maximized;

                int targetWidth, targetHeight;

                if (isFullscreen)
                {
                    targetWidth = Math.Max(620, Math.Min(820, _modDetailOverlay.Width - 160));
                    targetHeight = (int)(targetWidth * 0.52);
                }
                else
                {
                    targetWidth = 595;
                    targetHeight = 305;
                }

                targetWidth = Math.Min(targetWidth, _modDetailOverlay.Width - 100);
                targetHeight = Math.Min(targetHeight, _modDetailOverlay.Height - 100);

                fvCard.Size = new Size(targetWidth, targetHeight);

                fvCard.Location = new Point(
                    (_modDetailOverlay.Width - fvCard.Width) / 2,
                    (_modDetailOverlay.Height - fvCard.Height) / 2);
            }

            CentreAndScaleCard();

            _modDetailOverlay.Resize += (s, e) => CentreAndScaleCard();

            if (targetPanel is FlowLayoutPanel flp)
                flp.AutoScroll = false;

            _modDetailOverlay.Click += (s, e) => CloseModDetail();
            _modDetailOverlay.Controls.Add(fvCard);
            targetPanel.Controls.Add(_modDetailOverlay);
            _modDetailOverlay.BringToFront();
        }

        private void SyncOverlaySize(object sender, EventArgs e)
        {
            if (_modDetailOverlay == null || _modDetailOverlay.IsDisposed) return;
            if (sender is Panel p) _modDetailOverlay.Size = p.ClientSize;
        }

        private void CloseModDetail()
        {
            if (_modDetailOverlay == null || _modDetailOverlay.IsDisposed) return;

            if (_overlayOwner is FlowLayoutPanel flp)
                flp.AutoScroll = true;

            if (_overlayOwner != null && !_overlayOwner.IsDisposed)
            {
                _overlayOwner.Resize -= SyncOverlaySize;
                _overlayOwner.Controls.Remove(_modDetailOverlay);
            }

            _modDetailOverlay.Dispose();
            _modDetailOverlay = null;
            _overlayOwner = null;
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

            var colHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.FromArgb(18, 18, 18)
            };
            colHeader.Controls.Add(new Label { Text = "MOD", ForeColor = Color.FromArgb(90, 90, 90), Font = new Font("Poppins", 7F, FontStyle.Bold), Location = new Point(80, 5), AutoSize = true, BackColor = Color.Transparent });
            colHeader.Controls.Add(new Label { Text = "FILE", ForeColor = Color.FromArgb(90, 90, 90), Font = new Font("Poppins", 7F, FontStyle.Bold), Location = new Point(340, 5), AutoSize = true, BackColor = Color.Transparent });
            colHeader.Controls.Add(new Label { Text = "SIZE", ForeColor = Color.FromArgb(90, 90, 90), Font = new Font("Poppins", 7F, FontStyle.Bold), Location = new Point(520, 5), AutoSize = true, BackColor = Color.Transparent });
            colHeader.Controls.Add(new Label { Text = "ACTIONS", ForeColor = Color.FromArgb(90, 90, 90), Font = new Font("Poppins", 7F, FontStyle.Bold), Location = new Point(600, 5), AutoSize = true, BackColor = Color.Transparent });

            installedFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = _currentTheme.PanelBack,
                Padding = new Padding(6, 4, 6, 4),
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
            InstalledView.Controls.Add(colHeader);
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
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = _currentTheme.PanelBack,
                Padding = new Padding(6, 4, 6, 4)
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

                    if (allDlls.Any())
                        foreach (var dll in allDlls)
                            modFlow.Controls.Add(BuildLoadoutModRow(dll, loadoutIndex, loadout));
                    else
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

        private Panel BuildLoadoutModRow(string filePath, int loadoutIndex, Loadout loadout)
        {
            bool isDisabled = filePath.EndsWith(".disabled", StringComparison.OrdinalIgnoreCase);
            string fileName = isDisabled
                ? Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath)) + ".dll"
                : Path.GetFileName(filePath);
            string modName = Path.GetFileNameWithoutExtension(fileName);
            bool included = loadout.EnabledMods.Contains(fileName);

            long fileSize = new FileInfo(filePath).Length;
            string sizeText = fileSize >= 1024 * 1024
                ? $"{fileSize / (1024f * 1024f):0.00} MB"
                : $"{fileSize / 1024f:0.0} KB";

            Color rowBg = included ? _currentTheme.CardBack : Color.FromArgb(20, 20, 20);
            Color nameFg = included ? _currentTheme.LabelFore : Color.FromArgb(70, 70, 70);
            Color borderCol = included ? Color.FromArgb(52, 52, 52) : Color.FromArgb(35, 35, 35);

            var row = new Panel
            {
                Height = 54,
                Width = 740,
                BackColor = rowBg,
                Margin = new Padding(0, 0, 0, 2)
            };

            _loadoutEditorPanel.Resize += (s, e) =>
            {
                if (!row.IsDisposed)
                    row.Width = _loadoutEditorPanel.ClientSize.Width - 20;
            };

            var thumb = new Panel
            {
                Location = new Point(10, 5),
                Size = new Size(44, 44),
                BackColor = Color.Transparent
            };
            thumb.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, 43, 43), 6))
                using (var bg = new SolidBrush(included ? Color.FromArgb(55, 35, 100) : Color.FromArgb(28, 28, 28)))
                    e.Graphics.FillPath(bg, path);
                string letter = modName.Length > 0 ? modName[0].ToString().ToUpper() : "?";
                using (var font = new Font("Poppins", 15F, FontStyle.Bold))
                using (var brush = new SolidBrush(included ? Color.FromArgb(180, 150, 255) : Color.FromArgb(55, 55, 55)))
                using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(letter, font, brush, new RectangleF(0, 0, 44, 44), fmt);
            };

            var matchedMod = allMods?.FirstOrDefault(m =>
                string.Equals(m.DllName, fileName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(m.Name, modName, StringComparison.OrdinalIgnoreCase));
            if (matchedMod != null && !string.IsNullOrEmpty(matchedMod.ImageUrl))
            {
                var pic = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent };
                thumb.Controls.Add(pic);
                _ = LoadThumbAsync(pic, matchedMod.ImageUrl);
            }
            row.Controls.Add(thumb);

            row.Controls.Add(new Label
            {
                Text = modName,
                ForeColor = nameFg,
                Font = new Font("Poppins", 9F, FontStyle.Bold),
                Location = new Point(62, 8),
                Size = new Size(250, 20),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            });
            row.Controls.Add(new Label
            {
                Text = fileName,
                ForeColor = Color.FromArgb(70, 70, 80),
                Font = new Font("Consolas", 7.5F),
                Location = new Point(62, 30),
                Size = new Size(250, 16),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            });
            row.Controls.Add(new Label
            {
                Text = sizeText,
                ForeColor = Color.FromArgb(70, 70, 80),
                Font = new Font("Poppins", 7.5F),
                Location = new Point(320, 19),
                Size = new Size(90, 16),
                BackColor = Color.Transparent
            });

            bool capturedIncluded = included;
            string capturedFileName = fileName;

            var includeBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = included ? "✓ Included" : "Include",
                FillColor = included ? _accentColor : _currentTheme.ButtonInactive,
                ForeColor = Color.White,
                Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(100, 28),
                Location = new Point(620, 13),
                Animated = true
            };
            includeBtn.HoverState.FillColor = capturedIncluded ? ControlPaint.Light(_accentColor, 0.2f) : _accentColor;

            includeBtn.Click += (s, e) =>
            {
                capturedIncluded = !capturedIncluded;
                if (capturedIncluded)
                {
                    _loadouts[loadoutIndex].EnabledMods.Add(capturedFileName);
                    includeBtn.Text = "✓ Included";
                    includeBtn.FillColor = _accentColor;
                    includeBtn.HoverState.FillColor = ControlPaint.Light(_accentColor, 0.2f);
                    row.BackColor = _currentTheme.CardBack;
                }
                else
                {
                    _loadouts[loadoutIndex].EnabledMods.Remove(capturedFileName);
                    includeBtn.Text = "Include";
                    includeBtn.FillColor = _currentTheme.ButtonInactive;
                    includeBtn.HoverState.FillColor = _accentColor;
                    row.BackColor = Color.FromArgb(20, 20, 20);
                }
                SaveConfig();
            };
            row.Controls.Add(includeBtn);

            Color capturedBorder = borderCol;
            row.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, row.Width - 1, row.Height - 1), 7))
                using (var pen = new Pen(capturedBorder, 1f))
                    e.Graphics.DrawPath(pen, path);
            };

            return row;
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

            var checkedRows = new List<(string path, bool disabled, Panel chk)>();

            installedFlowPanel.SuspendLayout();
            foreach (var dll in enabledDlls) installedFlowPanel.Controls.Add(BuildInstalledRow(dll, false, checkedRows));
            foreach (var dll in disabledDlls) installedFlowPanel.Controls.Add(BuildInstalledRow(dll, true, checkedRows));
            installedFlowPanel.ResumeLayout();
        }

        private Panel BuildInstalledRow(string filePath, bool isDisabled, List<(string path, bool disabled, Panel chk)> checkedRows)
        {
            string modName = isDisabled
                ? Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath))
                : Path.GetFileNameWithoutExtension(filePath);
            string fileName = isDisabled ? modName + ".dll" : Path.GetFileName(filePath);

            long fileSize = new FileInfo(filePath).Length;
            string sizeText = fileSize >= 1024 * 1024
                ? $"{fileSize / (1024f * 1024f):0.00} MB"
                : $"{fileSize / 1024f:0.0} KB";

            Color rowBg = isDisabled ? Color.FromArgb(20, 20, 20) : _currentTheme.CardBack;
            Color nameFg = isDisabled ? Color.FromArgb(70, 70, 70) : _currentTheme.LabelFore;
            Color subFg = isDisabled ? Color.FromArgb(55, 55, 55) : _currentTheme.SubLabelFore;
            Color borderCol = isDisabled ? Color.FromArgb(35, 35, 35) : Color.FromArgb(52, 52, 52);

            var row = new Panel
            {
                Height = 58,
                Width = installedFlowPanel.ClientSize.Width > 14 ? installedFlowPanel.ClientSize.Width - 14 : 740,
                BackColor = rowBg,
                Margin = new Padding(0, 0, 0, 2)
            };

            bool chkChecked = false;
            var chk = new Panel
            {
                Location = new Point(8, 19),
                Size = new Size(18, 18),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            chk.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, 17, 17), 4))
                {
                    using (var bg = new SolidBrush(chkChecked ? _accentColor : _currentTheme.InputFill))
                        e.Graphics.FillPath(bg, path);
                    using (var pen = new Pen(chkChecked ? _accentColor : Color.FromArgb(75, 75, 85), 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }
                if (chkChecked)
                {
                    using (var pen = new Pen(Color.White, 2f) { LineJoin = LineJoin.Round })
                        e.Graphics.DrawLines(pen, new[] { new PointF(3.5f, 9f), new PointF(7f, 13f), new PointF(14f, 4.5f) });
                }
            };
            chk.Click += (s, e) => { chkChecked = !chkChecked; chk.Tag = chkChecked; chk.Invalidate(); };
            row.Controls.Add(chk);

            checkedRows.Add((filePath, isDisabled, chk));

            var thumb = new Panel
            {
                Location = new Point(32, 7),
                Size = new Size(44, 44),
                BackColor = Color.Transparent
            };

            var matchedMod = allMods?.FirstOrDefault(m =>
                string.Equals(m.DllName, fileName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(m.Name, modName, StringComparison.OrdinalIgnoreCase));

            thumb.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, 43, 43), 6))
                using (var bg = new SolidBrush(isDisabled ? Color.FromArgb(28, 28, 28) : Color.FromArgb(55, 35, 100)))
                    e.Graphics.FillPath(bg, path);
                string letter = modName.Length > 0 ? modName[0].ToString().ToUpper() : "?";
                using (var font = new Font("Poppins", 15F, FontStyle.Bold))
                using (var brush = new SolidBrush(isDisabled ? Color.FromArgb(55, 55, 55) : Color.FromArgb(180, 150, 255)))
                using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(letter, font, brush, new RectangleF(0, 0, 44, 44), fmt);
            };

            if (matchedMod != null && !string.IsNullOrEmpty(matchedMod.ImageUrl))
            {
                var pic = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent };
                thumb.Controls.Add(pic);
                _ = LoadThumbAsync(pic, matchedMod.ImageUrl);
            }
            row.Controls.Add(thumb);

            row.Controls.Add(new Label
            {
                Text = modName,
                ForeColor = nameFg,
                Font = new Font("Poppins", 9F, isDisabled ? FontStyle.Bold | FontStyle.Strikeout : FontStyle.Bold),
                Location = new Point(84, 10),
                Size = new Size(220, 20),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            });

            if (matchedMod != null && !string.IsNullOrEmpty(matchedMod.Author))
            {
                row.Controls.Add(new Label
                {
                    Text = "@" + matchedMod.Author,
                    ForeColor = isDisabled ? Color.FromArgb(45, 45, 45) : Color.FromArgb(100, 100, 120),
                    Font = new Font("Segoe UI", 7F),
                    Location = new Point(84, 31),
                    Size = new Size(220, 15),
                    AutoEllipsis = true,
                    BackColor = Color.Transparent
                });
            }

            row.Controls.Add(new Label
            {
                Text = fileName,
                ForeColor = subFg,
                Font = new Font("Consolas", 7.5F),
                Location = new Point(318, 10),
                Size = new Size(190, 18),
                AutoEllipsis = true,
                BackColor = Color.Transparent
            });
            row.Controls.Add(new Label
            {
                Text = sizeText,
                ForeColor = subFg,
                Font = new Font("Poppins", 7.5F),
                Location = new Point(318, 30),
                Size = new Size(100, 16),
                BackColor = Color.Transparent
            });

            var pill = new Panel
            {
                Location = new Point(row.Width - 264, 19),
                Size = new Size(80, 20),
                BackColor = Color.Transparent
            };
            string pillText = isDisabled ? "Disabled" : "Enabled";
            Color pillFg = isDisabled ? Color.FromArgb(90, 90, 90) : Color.FromArgb(80, 200, 120);
            Color pillBg = isDisabled ? Color.FromArgb(28, 28, 28) : Color.FromArgb(20, 60, 35);
            pill.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, pill.Width - 1, pill.Height - 1), 9))
                {
                    using (var bg = new SolidBrush(pillBg)) e.Graphics.FillPath(bg, path);
                    using (var pen = new Pen(pillFg, 1f)) e.Graphics.DrawPath(pen, path);
                }
                using (var brush = new SolidBrush(pillFg))
                using (var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(pillText, new Font("Poppins", 6.5F, FontStyle.Bold), brush, new RectangleF(0, 0, pill.Width, pill.Height), fmt);
            };
            row.Controls.Add(pill);

            var toggleBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = isDisabled ? "Enable" : "Disable",
                FillColor = isDisabled ? Color.FromArgb(38, 38, 38) : Color.FromArgb(60, 30, 0),
                ForeColor = isDisabled ? Color.FromArgb(150, 150, 150) : Color.FromArgb(255, 140, 40),
                Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(82, 26),
                Location = new Point(row.Width - 174, 16),
                Animated = true
            };
            toggleBtn.HoverState.FillColor = isDisabled ? Color.FromArgb(55, 55, 55) : Color.FromArgb(90, 45, 0);

            var uninstallBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Remove",
                FillColor = Color.FromArgb(70, 18, 18),
                ForeColor = Color.FromArgb(255, 90, 90),
                Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                BorderRadius = 4,
                Size = new Size(82, 26),
                Location = new Point(row.Width - 88, 16),
                Animated = true
            };
            uninstallBtn.HoverState.FillColor = Color.FromArgb(120, 25, 25);

            installedFlowPanel.Resize += (s, e) =>
            {
                if (!row.IsDisposed)
                {
                    row.Width = installedFlowPanel.ClientSize.Width - 14;
                    toggleBtn.Left = row.Width - 174;
                    uninstallBtn.Left = row.Width - 88;
                    pill.Left = row.Width - 264;
                }
            };

            string capturedPath = filePath;
            bool capturedDisabled = isDisabled;

            toggleBtn.Click += (s, e) =>
            {
                var targets = checkedRows.Where(r => IsChkChecked(r.chk)).ToList();
                if (targets.Count == 0) targets.Add((capturedPath, capturedDisabled, chk));

                string pluginsRoot = Path.Combine(GtagDirectory, "BepInEx", "plugins");
                string disabledRoot = Path.Combine(pluginsRoot, "disabled");
                Directory.CreateDirectory(disabledRoot);

                foreach (var (tPath, tDisabled, _) in targets)
                {
                    try
                    {
                        if (tDisabled)
                        {
                            string relativePath = tPath.Substring(disabledRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                            string dllName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(relativePath));
                            string subFolder = Path.GetDirectoryName(relativePath);
                            string destDir = string.IsNullOrEmpty(subFolder) ? pluginsRoot : Path.Combine(pluginsRoot, subFolder);
                            Directory.CreateDirectory(destDir);
                            File.Move(tPath, Path.Combine(destDir, dllName + ".dll"));
                        }
                        else
                        {
                            string relativePath = tPath.Substring(pluginsRoot.Length).TrimStart(Path.DirectorySeparatorChar);
                            string subFolder = Path.GetDirectoryName(relativePath);
                            string destDir = string.IsNullOrEmpty(subFolder) ? disabledRoot : Path.Combine(disabledRoot, subFolder);
                            Directory.CreateDirectory(destDir);
                            File.Move(tPath, Path.Combine(destDir, Path.GetFileName(tPath) + ".disabled"));
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("Failed to toggle mod: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                LoadInstalledMods();
            };

            uninstallBtn.Click += (s, e) =>
            {
                var targets = checkedRows.Where(r => IsChkChecked(r.chk)).ToList();
                if (targets.Count == 0) targets.Add((capturedPath, capturedDisabled, chk));

                string names = targets.Count == 1
                    ? Path.GetFileNameWithoutExtension(targets[0].path)
                    : $"{targets.Count} mods";
                if (MessageBox.Show($"Uninstall {names}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                foreach (var (tPath, _, _x) in targets)
                {
                    try { File.Delete(tPath); }
                    catch (Exception ex) { MessageBox.Show("Failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                LoadInstalledMods();
            };

            row.Controls.Add(toggleBtn);
            row.Controls.Add(uninstallBtn);

            Color capturedBorder = borderCol;
            bool hovering = false;
            row.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rc = new Rectangle(0, 0, row.Width - 1, row.Height - 1);
                using (var path = RoundedRect(rc, 7))
                {
                    if (hovering)
                        using (var hl = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
                            e.Graphics.FillPath(hl, path);
                    using (var pen = new Pen(capturedBorder, 1f))
                        e.Graphics.DrawPath(pen, path);
                }
            };
            row.MouseEnter += (s, e) => { hovering = true; row.Invalidate(); };
            row.MouseLeave += (s, e) => { hovering = false; row.Invalidate(); };

            return row;
        }

        private static bool IsChkChecked(Panel chk)
        {
            if (chk == null || chk.IsDisposed) return false;
            return chk.Tag is bool b && b;
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

        private static async Task LoadThumbAsync(PictureBox pic, string url)
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
                        if (pic.IsHandleCreated)
                            pic.Invoke((Action)(() => { pic.Image = img; }));
                        else
                            pic.Image = img;
                    }
                }
            }
            catch { }
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

            const int padX = 20;
            const int padY = 16;
            const int cardGap = 12;
            const int rowH = 52;
            const int labelCol = 14;
            const int controlCol = 160;
            const int ctrlH = 30;
            const int headerH = 36;

            var scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            SettingsView.Controls.Add(scroll);

            int CardWidth() => scroll.ClientSize.Width > padX * 2 + 10 ? scroll.ClientSize.Width - padX * 2 : 680;

            Panel MakeCard(int cardY, int cardH)
            {
                var card = new Panel
                {
                    Location = new Point(padX, cardY),
                    Width = CardWidth(),
                    Height = cardH,
                    BackColor = _currentTheme.CardBack
                };
                scroll.Resize += (s, e) => { if (!card.IsDisposed) card.Width = CardWidth(); };
                card.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 8))
                    {
                        using (var bg = new SolidBrush(_currentTheme.CardBack))
                            e.Graphics.FillPath(bg, path);
                        using (var pen = new Pen(_currentTheme.InputBorder, 1f))
                            e.Graphics.DrawPath(pen, path);
                    }
                };
                return card;
            }

            void AddCardHeader(Panel card, string title)
            {
                card.Controls.Add(new Label
                {
                    Text = title,
                    ForeColor = _accentColor,
                    Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                    Location = new Point(labelCol, 10),
                    AutoSize = true,
                    BackColor = Color.Transparent
                });
                var div = new Panel { Location = new Point(labelCol, 26), Height = 1, BackColor = _currentTheme.InputBorder };
                card.Controls.Add(div);
                card.Resize += (s, e) => { if (!div.IsDisposed) div.Width = card.Width - labelCol * 2; };
                div.Width = card.Width - labelCol * 2;
            }

            Label MakeLabel(Panel card, string text, int rowTop) => new Label
            {
                Text = text,
                ForeColor = _currentTheme.SubLabelFore,
                Font = new Font("Poppins", 8.5F),
                Location = new Point(labelCol, rowTop + (ctrlH / 2) - 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            int cy = padY;

            int pathCardH = headerH + rowH + 8;
            var pathCard = MakeCard(cy, pathCardH);
            scroll.Controls.Add(pathCard);
            AddCardHeader(pathCard, "GAME PATH");

            int r1 = headerH + 8;
            pathCard.Controls.Add(MakeLabel(pathCard, "Game Directory", r1));

            settingsPathBox = new Guna.UI2.WinForms.Guna2TextBox
            {
                Location = new Point(controlCol, r1),
                Size = new Size(270, ctrlH),
                FillColor = _currentTheme.InputFill,
                BorderColor = _currentTheme.InputBorder,
                ForeColor = _currentTheme.LabelFore,
                Font = new Font("Poppins", 8F),
                BorderRadius = 6,
                Text = GtagDirectory ?? ""
            };
            settingsPathBox.FocusedState.BorderColor = _accentColor;
            settingsPathBox.HoverState.BorderColor = _accentColor;
            pathCard.Controls.Add(settingsPathBox);

            var browsePathBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Browse",
                Location = new Point(controlCol + 276, r1),
                Size = new Size(70, ctrlH),
                FillColor = _currentTheme.ButtonInactive,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 6,
                Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                Animated = true
            };
            browsePathBtn.HoverState.FillColor = _accentColor;
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
            pathCard.Controls.Add(browsePathBtn);

            var openFolderBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Open",
                Location = new Point(controlCol + 276 + 76, r1),
                Size = new Size(60, ctrlH),
                FillColor = _currentTheme.ButtonInactive,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 6,
                Font = new Font("Poppins", 7.5F, FontStyle.Bold),
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
            pathCard.Controls.Add(openFolderBtn);

            cy += pathCardH + cardGap;

            int appearCardH = headerH + rowH + rowH + rowH + 8;
            var appearCard = MakeCard(cy, appearCardH);
            scroll.Controls.Add(appearCard);
            AddCardHeader(appearCard, "APPEARANCE");

            int ra = headerH + 8;
            appearCard.Controls.Add(MakeLabel(appearCard, "Theme", ra));
            var themes = new[] { "Dark", "Darker", "Light" };
            for (int i = 0; i < themes.Length; i++)
            {
                string t = themes[i];
                bool sel = t == _themeName;
                var tb = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = t,
                    Location = new Point(controlCol + i * 90, ra),
                    Size = new Size(84, ctrlH),
                    FillColor = sel ? _accentColor : _currentTheme.ButtonInactive,
                    ForeColor = Color.White,
                    BorderRadius = 6,
                    Font = new Font("Poppins", 8F, FontStyle.Bold),
                    Animated = true
                };
                tb.HoverState.FillColor = sel ? ControlPaint.Light(_accentColor, 0.15f) : _accentColor;
                tb.Click += (s, e) =>
                {
                    _themeName = t;
                    _currentTheme = AppTheme.FromName(t);
                    ApplyTheme(_currentTheme);
                    ApplyAccent(_accentColor);
                    InitializeSettingsView();
                };
                appearCard.Controls.Add(tb);
            }

            int rb = ra + rowH;
            appearCard.Controls.Add(MakeLabel(appearCard, "Accent Color", rb));
            var presets = new[]
            {
                ("Purple", Color.FromArgb(111, 69, 240)),
                ("Blue",   Color.FromArgb(56, 139, 253)),
                ("Green",  Color.FromArgb(39, 174, 96)),
                ("Red",    Color.FromArgb(220, 50, 50)),
                ("Pink",   Color.FromArgb(220, 80, 160)),
                ("Orange", Color.FromArgb(230, 120, 30))
            };
            int px = 0;
            foreach (var (name, col) in presets)
            {
                Color cc = col;
                bool active = Math.Abs(_accentColor.R - col.R) < 5 && Math.Abs(_accentColor.G - col.G) < 5 && Math.Abs(_accentColor.B - col.B) < 5;
                var pb = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = name,
                    Location = new Point(controlCol + px, rb),
                    Size = new Size(72, ctrlH),
                    FillColor = active ? ControlPaint.Light(col, 0.2f) : col,
                    ForeColor = Color.White,
                    BorderRadius = 6,
                    Font = new Font("Poppins", 7.5F, FontStyle.Bold),
                    Animated = true
                };
                pb.HoverState.FillColor = ControlPaint.Light(col, 0.2f);
                pb.Click += (s, e) => ApplyAccent(cc);
                appearCard.Controls.Add(pb);
                px += 78;
            }

            int rc = rb + rowH;
            appearCard.Controls.Add(MakeLabel(appearCard, "Custom Color", rc));

            var customColorBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Pick Color...",
                Location = new Point(controlCol, rc),
                Size = new Size(110, ctrlH),
                FillColor = _currentTheme.ButtonInactive,
                ForeColor = _currentTheme.LabelFore,
                BorderRadius = 6,
                Font = new Font("Poppins", 8F),
                Animated = true
            };
            customColorBtn.HoverState.FillColor = _accentColor;
            customColorBtn.Click += (s, e) =>
            {
                using (var dlg = new ColorDialog { Color = _accentColor, FullOpen = true })
                    if (dlg.ShowDialog() == DialogResult.OK) ApplyAccent(dlg.Color);
            };
            appearCard.Controls.Add(customColorBtn);

            accentPreviewPanel = new Panel
            {
                Location = new Point(controlCol + 116, rc + (ctrlH / 2) - 10),
                Size = new Size(20, 20),
                BackColor = Color.Transparent
            };
            accentPreviewPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, 19, 19), 5))
                using (var bg = new SolidBrush(_accentColor))
                    e.Graphics.FillPath(bg, path);
            };
            appearCard.Controls.Add(accentPreviewPanel);

            cy += appearCardH + cardGap;

            var saveBtn = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Save Settings",
                Location = new Point(padX, cy + 4),
                Size = new Size(160, 36),
                FillColor = _accentColor,
                ForeColor = Color.White,
                BorderRadius = 8,
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
            scroll.Controls.Add(saveBtn);

            themeDropdown = new ComboBox { Visible = false };
            scroll.Controls.Add(themeDropdown);
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
            foreach (var mod in sorted)
            {
                var card = new ModCard(mod, GtagDirectory);
                var capturedMod = mod;
                WireCardClick(card, () => ShowModDetail(capturedMod));
                flowPanel.Controls.Add(card);
            }
            flowPanel.ResumeLayout();

            PopulateHomeView();
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
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void gorillamodmanager_Click(object sender, EventArgs e) { }

        private void FullScreenButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void HomeView_Paint(object sender, PaintEventArgs e) { }
        private void HomeTabBtn_Click(object sender, EventArgs e) => SetActiveTab("home");
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