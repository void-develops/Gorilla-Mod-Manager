namespace Gorilla_Mod_Manager
{
    partial class MainApp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.gorillamodmanager = new System.Windows.Forms.Label();
            this.undertext = new System.Windows.Forms.Label();
            this.FilePath = new Guna.UI2.WinForms.Guna2TextBox();
            this.opengamepath = new Guna.UI2.WinForms.Guna2Button();
            this.DashboardBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SearchBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.FilterDropdown = new Guna.UI2.WinForms.Guna2ComboBox();
            this.Close = new Guna.UI2.WinForms.Guna2CircleButton();
            this.Minimize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.ModView = new Guna.UI2.WinForms.Guna2Panel();
            this.InstallBepinex = new Guna.UI2.WinForms.Guna2Button();
            this.Github = new Guna.UI2.WinForms.Guna2CircleButton();
            this.BrowseTabBtn = new Guna.UI2.WinForms.Guna2Button();
            this.InstalledTabBtn = new Guna.UI2.WinForms.Guna2Button();
            this.InstalledView = new Guna.UI2.WinForms.Guna2Panel();
            this.LaunchGameBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.AnimateWindow = true;
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.DragStartTransparencyValue = 0.7D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // gorillamodmanager
            // 
            this.gorillamodmanager.AutoSize = true;
            this.gorillamodmanager.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gorillamodmanager.ForeColor = System.Drawing.Color.White;
            this.gorillamodmanager.Location = new System.Drawing.Point(112, 13);
            this.gorillamodmanager.Name = "gorillamodmanager";
            this.gorillamodmanager.Size = new System.Drawing.Size(207, 25);
            this.gorillamodmanager.TabIndex = 0;
            this.gorillamodmanager.Text = "gorilla mod manager";
            // 
            // undertext
            // 
            this.undertext.AutoSize = true;
            this.undertext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undertext.ForeColor = System.Drawing.Color.White;
            this.undertext.Location = new System.Drawing.Point(114, 50);
            this.undertext.Name = "undertext";
            this.undertext.Size = new System.Drawing.Size(167, 13);
            this.undertext.TabIndex = 4;
            this.undertext.Text = "inspired by monkey mod manager.";
            // 
            // FilePath
            // 
            this.FilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilePath.Animated = true;
            this.FilePath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.FilePath.BorderRadius = 4;
            this.FilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilePath.DefaultText = "";
            this.FilePath.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.FilePath.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.FilePath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FilePath.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.FilePath.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.FilePath.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FilePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FilePath.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FilePath.Location = new System.Drawing.Point(117, 74);
            this.FilePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FilePath.Name = "FilePath";
            this.FilePath.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.FilePath.PlaceholderText = "";
            this.FilePath.SelectedText = "";
            this.FilePath.Size = new System.Drawing.Size(273, 26);
            this.FilePath.TabIndex = 2;
            // 
            // opengamepath
            // 
            this.opengamepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.opengamepath.Animated = true;
            this.opengamepath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.opengamepath.BorderRadius = 4;
            this.opengamepath.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.opengamepath.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.opengamepath.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.opengamepath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.opengamepath.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.opengamepath.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.opengamepath.ForeColor = System.Drawing.Color.White;
            this.opengamepath.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.opengamepath.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.opengamepath.Location = new System.Drawing.Point(396, 74);
            this.opengamepath.Name = "opengamepath";
            this.opengamepath.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.opengamepath.Size = new System.Drawing.Size(37, 26);
            this.opengamepath.TabIndex = 8;
            this.opengamepath.Text = "...";
            this.opengamepath.Click += new System.EventHandler(this.opengamepath_Click);
            // 
            // DashboardBtn
            // 
            this.DashboardBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DashboardBtn.Animated = true;
            this.DashboardBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.DashboardBtn.BorderRadius = 4;
            this.DashboardBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.DashboardBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.DashboardBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.DashboardBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.DashboardBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.DashboardBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.DashboardBtn.ForeColor = System.Drawing.Color.White;
            this.DashboardBtn.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(69)))), ((int)(((byte)(240)))));
            this.DashboardBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(45)))), ((int)(((byte)(170)))));
            this.DashboardBtn.Location = new System.Drawing.Point(439, 74);
            this.DashboardBtn.Name = "DashboardBtn";
            this.DashboardBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.DashboardBtn.Size = new System.Drawing.Size(89, 26);
            this.DashboardBtn.TabIndex = 9;
            this.DashboardBtn.Text = "Dashboard";
            this.DashboardBtn.Click += new System.EventHandler(this.DashboardBtn_Click);
            // 
            // SearchBox
            // 
            this.SearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBox.Animated = true;
            this.SearchBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.SearchBox.BorderRadius = 4;
            this.SearchBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SearchBox.DefaultText = "";
            this.SearchBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.SearchBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.SearchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SearchBox.ForeColor = System.Drawing.Color.White;
            this.SearchBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.SearchBox.Location = new System.Drawing.Point(117, 106);
            this.SearchBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.SearchBox.PlaceholderText = "Search mods...";
            this.SearchBox.SelectedText = "";
            this.SearchBox.Size = new System.Drawing.Size(273, 26);
            this.SearchBox.TabIndex = 10;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // FilterDropdown
            // 
            this.FilterDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.FilterDropdown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.FilterDropdown.BorderRadius = 5;
            this.FilterDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FilterDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterDropdown.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.FilterDropdown.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FilterDropdown.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.FilterDropdown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FilterDropdown.ForeColor = System.Drawing.Color.White;
            this.FilterDropdown.ItemHeight = 30;
            this.FilterDropdown.Items.AddRange(new object[] {
            "All",
            "Featured",
            "Verified",
            "Unverified"});
            this.FilterDropdown.Location = new System.Drawing.Point(534, 74);
            this.FilterDropdown.Name = "FilterDropdown";
            this.FilterDropdown.Size = new System.Drawing.Size(132, 36);
            this.FilterDropdown.TabIndex = 11;
            this.FilterDropdown.SelectedIndexChanged += new System.EventHandler(this.FilterDropdown_SelectedIndexChanged);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.Animated = true;
            this.Close.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Close.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Close.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Close.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Close.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.Close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Close.ForeColor = System.Drawing.Color.White;
            this.Close.Location = new System.Drawing.Point(665, 12);
            this.Close.Name = "Close";
            this.Close.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Close.Size = new System.Drawing.Size(20, 20);
            this.Close.TabIndex = 5;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // Minimize
            // 
            this.Minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Minimize.Animated = true;
            this.Minimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Minimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Minimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Minimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Minimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(80)))));
            this.Minimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.Location = new System.Drawing.Point(639, 12);
            this.Minimize.Name = "Minimize";
            this.Minimize.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Minimize.Size = new System.Drawing.Size(20, 20);
            this.Minimize.TabIndex = 6;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.guna2Separator1.Location = new System.Drawing.Point(117, 138);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(591, 10);
            this.guna2Separator1.TabIndex = 12;
            // 
            // ModView
            // 
            this.ModView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ModView.BorderRadius = 5;
            this.ModView.Location = new System.Drawing.Point(117, 152);
            this.ModView.Name = "ModView";
            this.ModView.Size = new System.Drawing.Size(614, 290);
            this.ModView.TabIndex = 13;
            this.ModView.Paint += new System.Windows.Forms.PaintEventHandler(this.ModView_Paint);
            // 
            // InstallBepinex
            // 
            this.InstallBepinex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InstallBepinex.Animated = true;
            this.InstallBepinex.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.InstallBepinex.BorderRadius = 4;
            this.InstallBepinex.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.InstallBepinex.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.InstallBepinex.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.InstallBepinex.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.InstallBepinex.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.InstallBepinex.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.InstallBepinex.ForeColor = System.Drawing.Color.White;
            this.InstallBepinex.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.InstallBepinex.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.InstallBepinex.Image = global::Gorilla_Mod_Manager.Properties.Resources._39589027;
            this.InstallBepinex.Location = new System.Drawing.Point(396, 106);
            this.InstallBepinex.Name = "InstallBepinex";
            this.InstallBepinex.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.InstallBepinex.Size = new System.Drawing.Size(37, 26);
            this.InstallBepinex.TabIndex = 14;
            this.InstallBepinex.Click += new System.EventHandler(this.InstallBepinex_Click);
            // 
            // Github
            // 
            this.Github.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Github.Animated = true;
            this.Github.BackColor = System.Drawing.Color.Transparent;
            this.Github.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Github.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Github.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Github.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Github.FillColor = System.Drawing.Color.Transparent;
            this.Github.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Github.ForeColor = System.Drawing.Color.White;
            this.Github.Image = global::Gorilla_Mod_Manager.Properties.Resources.icons8_github_30;
            this.Github.ImageSize = new System.Drawing.Size(30, 30);
            this.Github.Location = new System.Drawing.Point(603, 7);
            this.Github.Name = "Github";
            this.Github.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Github.Size = new System.Drawing.Size(30, 30);
            this.Github.TabIndex = 7;
            this.Github.Click += new System.EventHandler(this.Github_Click);
            // 
            // BrowseTabBtn
            // 
            this.BrowseTabBtn.Animated = true;
            this.BrowseTabBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.BrowseTabBtn.BorderRadius = 4;
            this.BrowseTabBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BrowseTabBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BrowseTabBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BrowseTabBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BrowseTabBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BrowseTabBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.BrowseTabBtn.ForeColor = System.Drawing.Color.White;
            this.BrowseTabBtn.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.BrowseTabBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.BrowseTabBtn.Location = new System.Drawing.Point(12, 12);
            this.BrowseTabBtn.Name = "BrowseTabBtn";
            this.BrowseTabBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.BrowseTabBtn.Size = new System.Drawing.Size(96, 26);
            this.BrowseTabBtn.TabIndex = 15;
            this.BrowseTabBtn.Text = "Browse";
            this.BrowseTabBtn.Click += new System.EventHandler(this.BrowseTabBtn_Click);
            // 
            // InstalledTabBtn
            // 
            this.InstalledTabBtn.Animated = true;
            this.InstalledTabBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.InstalledTabBtn.BorderRadius = 4;
            this.InstalledTabBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.InstalledTabBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.InstalledTabBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.InstalledTabBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.InstalledTabBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.InstalledTabBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.InstalledTabBtn.ForeColor = System.Drawing.Color.White;
            this.InstalledTabBtn.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.InstalledTabBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.InstalledTabBtn.Location = new System.Drawing.Point(12, 49);
            this.InstalledTabBtn.Name = "InstalledTabBtn";
            this.InstalledTabBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.InstalledTabBtn.Size = new System.Drawing.Size(96, 26);
            this.InstalledTabBtn.TabIndex = 16;
            this.InstalledTabBtn.Text = "Installed";
            this.InstalledTabBtn.Click += new System.EventHandler(this.InstalledTabBtn_Click);
            // 
            // InstalledView
            // 
            this.InstalledView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstalledView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.InstalledView.BorderRadius = 5;
            this.InstalledView.Location = new System.Drawing.Point(117, 152);
            this.InstalledView.Name = "InstalledView";
            this.InstalledView.Size = new System.Drawing.Size(614, 290);
            this.InstalledView.TabIndex = 17;
            this.InstalledView.Visible = false;
            this.InstalledView.Paint += new System.Windows.Forms.PaintEventHandler(this.InstalledView_Paint);
            // 
            // LaunchGameBtn
            // 
            this.LaunchGameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LaunchGameBtn.Animated = true;
            this.LaunchGameBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.LaunchGameBtn.BorderRadius = 4;
            this.LaunchGameBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.LaunchGameBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.LaunchGameBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.LaunchGameBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.LaunchGameBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.LaunchGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold);
            this.LaunchGameBtn.ForeColor = System.Drawing.Color.White;
            this.LaunchGameBtn.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.LaunchGameBtn.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(200)))), ((int)(((byte)(115)))));
            this.LaunchGameBtn.Location = new System.Drawing.Point(12, 416);
            this.LaunchGameBtn.Name = "LaunchGameBtn";
            this.LaunchGameBtn.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.LaunchGameBtn.Size = new System.Drawing.Size(96, 26);
            this.LaunchGameBtn.TabIndex = 18;
            this.LaunchGameBtn.Text = "Launch";
            this.LaunchGameBtn.Click += new System.EventHandler(this.LaunchGameBtn_Click);
            // 
            // MainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(697, 454);
            this.Controls.Add(this.LaunchGameBtn);
            this.Controls.Add(this.InstalledView);
            this.Controls.Add(this.InstalledTabBtn);
            this.Controls.Add(this.BrowseTabBtn);
            this.Controls.Add(this.InstallBepinex);
            this.Controls.Add(this.ModView);
            this.Controls.Add(this.guna2Separator1);
            this.Controls.Add(this.FilterDropdown);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.DashboardBtn);
            this.Controls.Add(this.opengamepath);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.undertext);
            this.Controls.Add(this.gorillamodmanager);
            this.Controls.Add(this.Github);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(541, 454);
            this.Name = "MainApp";
            this.Text = "Gorilla Mod Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Fields
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button opengamepath;
        private Guna.UI2.WinForms.Guna2Button DashboardBtn;
        private Guna.UI2.WinForms.Guna2TextBox FilePath;
        private Guna.UI2.WinForms.Guna2TextBox SearchBox;
        private Guna.UI2.WinForms.Guna2ComboBox FilterDropdown;
        private Guna.UI2.WinForms.Guna2CircleButton Minimize;
        private Guna.UI2.WinForms.Guna2CircleButton Close;
        private Guna.UI2.WinForms.Guna2CircleButton Github;
        private Guna.UI2.WinForms.Guna2Panel ModView;
        private System.Windows.Forms.Label gorillamodmanager;
        private System.Windows.Forms.Label undertext;
        private Guna.UI2.WinForms.Guna2Button InstallBepinex;
        private Guna.UI2.WinForms.Guna2Panel InstalledView;
        private Guna.UI2.WinForms.Guna2Button InstalledTabBtn;
        private Guna.UI2.WinForms.Guna2Button BrowseTabBtn;
        private Guna.UI2.WinForms.Guna2Button LaunchGameBtn;
        #endregion
    }
}