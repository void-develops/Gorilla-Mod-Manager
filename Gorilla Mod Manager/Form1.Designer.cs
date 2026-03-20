namespace Gorilla_Mod_Manager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.gorillamodmanager = new System.Windows.Forms.Label();
            this.undertext = new System.Windows.Forms.Label();
            this.FilePath = new Guna.UI2.WinForms.Guna2TextBox();
            this.Open = new Guna.UI2.WinForms.Guna2Button();
            this.opengamepath = new Guna.UI2.WinForms.Guna2Button();
            this.DashboardBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SearchBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.FilterDropdown = new Guna.UI2.WinForms.Guna2ComboBox();
            this.Close = new Guna.UI2.WinForms.Guna2CircleButton();
            this.Minimize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.Github = new Guna.UI2.WinForms.Guna2CircleButton();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.ModView = new Guna.UI2.WinForms.Guna2Panel();
            this.SuspendLayout();

            // guna2BorderlessForm1
            this.guna2BorderlessForm1.AnimateWindow = true;
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.DragStartTransparencyValue = 0.7D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;

            // gorillamodmanager
            this.gorillamodmanager.AutoSize = true;
            this.gorillamodmanager.Font = new System.Drawing.Font("Poppins", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gorillamodmanager.ForeColor = System.Drawing.Color.White;
            this.gorillamodmanager.Location = new System.Drawing.Point(7, 12);
            this.gorillamodmanager.Name = "gorillamodmanager";
            this.gorillamodmanager.Size = new System.Drawing.Size(240, 37);
            this.gorillamodmanager.TabIndex = 0;
            this.gorillamodmanager.Text = "gorilla mod manager";
            this.gorillamodmanager.Click += new System.EventHandler(this.gorillamodmanager_Click);

            // undertext
            this.undertext.AutoSize = true;
            this.undertext.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undertext.ForeColor = System.Drawing.Color.White;
            this.undertext.Location = new System.Drawing.Point(7, 40);
            this.undertext.Name = "undertext";
            this.undertext.Size = new System.Drawing.Size(199, 19);
            this.undertext.TabIndex = 4;
            this.undertext.Text = "inspired by monkey mod manager.";
            this.undertext.Click += new System.EventHandler(this.label1_Click);

            // FilePath
            this.FilePath.Animated = true;
            this.FilePath.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.FilePath.BorderRadius = 4;
            this.FilePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilePath.DefaultText = "";
            this.FilePath.FillColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.FilePath.FocusedState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
            this.FilePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FilePath.HoverState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
            this.FilePath.Location = new System.Drawing.Point(196, 38);
            this.FilePath.Name = "FilePath";
            this.FilePath.PlaceholderText = "";
            this.FilePath.SelectedText = "";
            this.FilePath.Size = new System.Drawing.Size(222, 26);
            this.FilePath.TabIndex = 2;
            this.FilePath.TextChanged += new System.EventHandler(this.guna2TextBox1_TextChanged);

            // Open (hidden — replaced by mod view but kept for any existing handlers)
            this.Open.Animated = true;
            this.Open.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.Open.BorderRadius = 4;
            this.Open.FillColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.Open.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Open.ForeColor = System.Drawing.Color.White;
            this.Open.Location = new System.Drawing.Point(424, 38);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(0, 0);
            this.Open.TabIndex = 1;
            this.Open.Text = "Open";
            this.Open.Visible = false;
            this.Open.Click += new System.EventHandler(this.guna2Button1_Click);

            // opengamepath
            this.opengamepath.Animated = true;
            this.opengamepath.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.opengamepath.BorderRadius = 4;
            this.opengamepath.FillColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.opengamepath.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold);
            this.opengamepath.ForeColor = System.Drawing.Color.White;
            this.opengamepath.Location = new System.Drawing.Point(424, 38);
            this.opengamepath.Name = "opengamepath";
            this.opengamepath.Size = new System.Drawing.Size(37, 26);
            this.opengamepath.TabIndex = 8;
            this.opengamepath.Text = "...";
            this.opengamepath.Click += new System.EventHandler(this.opengamepath_Click);

            // DashboardBtn
            this.DashboardBtn.Animated = true;
            this.DashboardBtn.BorderColor = System.Drawing.Color.FromArgb(111, 69, 240);
            this.DashboardBtn.BorderRadius = 4;
            this.DashboardBtn.FillColor = System.Drawing.Color.FromArgb(60, 30, 140);
            this.DashboardBtn.Font = new System.Drawing.Font("Poppins SemiBold", 7F, System.Drawing.FontStyle.Bold);
            this.DashboardBtn.ForeColor = System.Drawing.Color.White;
            this.DashboardBtn.Location = new System.Drawing.Point(467, 38);
            this.DashboardBtn.Name = "DashboardBtn";
            this.DashboardBtn.Size = new System.Drawing.Size(68, 26);
            this.DashboardBtn.TabIndex = 9;
            this.DashboardBtn.Text = "Dashboard";
            this.DashboardBtn.Click += new System.EventHandler(this.DashboardBtn_Click);

            // SearchBox
            this.SearchBox.Animated = true;
            this.SearchBox.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.SearchBox.BorderRadius = 4;
            this.SearchBox.FillColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.SearchBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SearchBox.ForeColor = System.Drawing.Color.White;
            this.SearchBox.Location = new System.Drawing.Point(12, 72);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.PlaceholderText = "Search mods...";
            this.SearchBox.Size = new System.Drawing.Size(280, 26);
            this.SearchBox.TabIndex = 7;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);

            // FilterDropdown
            this.FilterDropdown.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.FilterDropdown.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FilterDropdown.FillColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.FilterDropdown.FocusedColor = System.Drawing.Color.FromArgb(111, 69, 240);
            this.FilterDropdown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FilterDropdown.ForeColor = System.Drawing.Color.White;
            this.FilterDropdown.Location = new System.Drawing.Point(300, 72);
            this.FilterDropdown.Name = "FilterDropdown";
            this.FilterDropdown.Size = new System.Drawing.Size(130, 26);
            this.FilterDropdown.TabIndex = 10;
            this.FilterDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterDropdown.Items.AddRange(new object[] { "All", "Featured", "Verified", "Unverified" });
            this.FilterDropdown.SelectedIndex = 0;
            this.FilterDropdown.SelectedIndexChanged += new System.EventHandler(this.FilterDropdown_SelectedIndexChanged);

            // Close
            this.Close.Animated = true;
            this.Close.FillColor = System.Drawing.Color.FromArgb(255, 80, 80);
            this.Close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Close.ForeColor = System.Drawing.Color.White;
            this.Close.Location = new System.Drawing.Point(541, 12);
            this.Close.Name = "Close";
            this.Close.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Close.Size = new System.Drawing.Size(20, 20);
            this.Close.TabIndex = 5;
            this.Close.Click += new System.EventHandler(this.Close_Click);

            // Minimize
            this.Minimize.Animated = true;
            this.Minimize.FillColor = System.Drawing.Color.FromArgb(255, 162, 80);
            this.Minimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.Location = new System.Drawing.Point(515, 12);
            this.Minimize.Name = "Minimize";
            this.Minimize.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Minimize.Size = new System.Drawing.Size(20, 20);
            this.Minimize.TabIndex = 6;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);

            // Github
            this.Github.Animated = true;
            this.Github.BackColor = System.Drawing.Color.Transparent;
            this.Github.FillColor = System.Drawing.Color.Transparent;
            this.Github.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Github.ForeColor = System.Drawing.Color.White;
            this.Github.Image = global::Gorilla_Mod_Manager.Properties.Resources.icons8_github_30;
            this.Github.ImageSize = new System.Drawing.Size(30, 30);
            this.Github.Location = new System.Drawing.Point(479, 12);
            this.Github.Name = "Github";
            this.Github.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Github.Size = new System.Drawing.Size(30, 30);
            this.Github.TabIndex = 11;
            this.Github.Click += new System.EventHandler(this.Github_Click);

            // guna2Separator1
            this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.guna2Separator1.Location = new System.Drawing.Point(-12, 101);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(600, 6);
            this.guna2Separator1.TabIndex = 12;

            // ModView
            this.ModView.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
            this.ModView.BorderRadius = 5;
            this.ModView.Location = new System.Drawing.Point(12, 110);
            this.ModView.Name = "ModView";
            this.ModView.Size = new System.Drawing.Size(549, 310);
            this.ModView.TabIndex = 13;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.ClientSize = new System.Drawing.Size(573, 432);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Gorilla Mod Manager";
            this.Load += new System.EventHandler(this.Form1_Load_1);

            this.Controls.Add(this.gorillamodmanager);
            this.Controls.Add(this.undertext);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.opengamepath);
            this.Controls.Add(this.DashboardBtn);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.FilterDropdown);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Github);
            this.Controls.Add(this.guna2Separator1);
            this.Controls.Add(this.ModView);
            this.Controls.Add(this.Open);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button Open;
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
    }
}