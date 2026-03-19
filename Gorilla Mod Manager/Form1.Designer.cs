namespace Gorilla_Mod_Manager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.gorillamodmanager = new System.Windows.Forms.Label();
            this.Open = new Guna.UI2.WinForms.Guna2Button();
            this.FilePath = new Guna.UI2.WinForms.Guna2TextBox();
            this.undertext = new System.Windows.Forms.Label();
            this.Close = new Guna.UI2.WinForms.Guna2CircleButton();
            this.Minimize = new Guna.UI2.WinForms.Guna2CircleButton();
            this.guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(this.components);
            this.opengamepath = new Guna.UI2.WinForms.Guna2Button();
            this.Github = new Guna.UI2.WinForms.Guna2CircleButton();
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
            this.gorillamodmanager.Font = new System.Drawing.Font("Poppins", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gorillamodmanager.ForeColor = System.Drawing.Color.White;
            this.gorillamodmanager.Location = new System.Drawing.Point(111, 76);
            this.gorillamodmanager.Name = "gorillamodmanager";
            this.gorillamodmanager.Size = new System.Drawing.Size(240, 37);
            this.gorillamodmanager.TabIndex = 0;
            this.gorillamodmanager.Text = "gorilla mod manager";
            this.gorillamodmanager.Click += new System.EventHandler(this.gorillamodmanager_Click);
            // 
            // Open
            // 
            this.Open.Animated = true;
            this.Open.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.Open.BorderRadius = 4;
            this.Open.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Open.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Open.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Open.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Open.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.Open.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Open.ForeColor = System.Drawing.Color.White;
            this.Open.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Open.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.Open.Location = new System.Drawing.Point(120, 184);
            this.Open.Name = "Open";
            this.Open.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.Open.Size = new System.Drawing.Size(222, 30);
            this.Open.TabIndex = 1;
            this.Open.Text = "Open";
            this.Open.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // FilePath
            // 
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
            this.FilePath.Location = new System.Drawing.Point(120, 152);
            this.FilePath.Name = "FilePath";
            this.FilePath.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.FilePath.PlaceholderText = "";
            this.FilePath.SelectedText = "";
            this.FilePath.Size = new System.Drawing.Size(222, 26);
            this.FilePath.TabIndex = 2;
            this.FilePath.TextChanged += new System.EventHandler(this.guna2TextBox1_TextChanged);
            // 
            // undertext
            // 
            this.undertext.AutoSize = true;
            this.undertext.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undertext.ForeColor = System.Drawing.Color.White;
            this.undertext.Location = new System.Drawing.Point(132, 113);
            this.undertext.Name = "undertext";
            this.undertext.Size = new System.Drawing.Size(199, 19);
            this.undertext.TabIndex = 4;
            this.undertext.Text = "inspired by monkey mod manager.";
            this.undertext.Click += new System.EventHandler(this.label1_Click);
            // 
            // Close
            // 
            this.Close.Animated = true;
            this.Close.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Close.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Close.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Close.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Close.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.Close.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Close.ForeColor = System.Drawing.Color.White;
            this.Close.Location = new System.Drawing.Point(435, 12);
            this.Close.Name = "Close";
            this.Close.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Close.Size = new System.Drawing.Size(20, 20);
            this.Close.TabIndex = 5;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // Minimize
            // 
            this.Minimize.Animated = true;
            this.Minimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.Minimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.Minimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.Minimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.Minimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(80)))));
            this.Minimize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.Location = new System.Drawing.Point(409, 12);
            this.Minimize.Name = "Minimize";
            this.Minimize.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Minimize.Size = new System.Drawing.Size(20, 20);
            this.Minimize.TabIndex = 6;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);
            // 
            // opengamepath
            // 
            this.opengamepath.Animated = true;
            this.opengamepath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.opengamepath.BorderRadius = 4;
            this.opengamepath.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.opengamepath.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.opengamepath.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.opengamepath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.opengamepath.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.opengamepath.Font = new System.Drawing.Font("Poppins SemiBold", 7.5F, System.Drawing.FontStyle.Bold);
            this.opengamepath.ForeColor = System.Drawing.Color.White;
            this.opengamepath.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.opengamepath.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.opengamepath.Location = new System.Drawing.Point(348, 152);
            this.opengamepath.Name = "opengamepath";
            this.opengamepath.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.opengamepath.Size = new System.Drawing.Size(37, 26);
            this.opengamepath.TabIndex = 8;
            this.opengamepath.Text = "...";
            this.opengamepath.Click += new System.EventHandler(this.opengamepath_Click);
            // 
            // Github
            // 
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
            this.Github.Location = new System.Drawing.Point(216, 220);
            this.Github.Name = "Github";
            this.Github.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.Github.Size = new System.Drawing.Size(30, 30);
            this.Github.TabIndex = 7;
            this.Github.Click += new System.EventHandler(this.Github_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(463, 326);
            this.Controls.Add(this.opengamepath);
            this.Controls.Add(this.Github);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.undertext);
            this.Controls.Add(this.FilePath);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.gorillamodmanager);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Gorilla Mod Manager";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Label gorillamodmanager;
        private Guna.UI2.WinForms.Guna2TextBox FilePath;
        private Guna.UI2.WinForms.Guna2Button Open;
        private System.Windows.Forms.Label undertext;
        private Guna.UI2.WinForms.Guna2CircleButton Minimize;
        private Guna.UI2.WinForms.Guna2CircleButton Close;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Guna.UI2.WinForms.Guna2CircleButton Github;
        private Guna.UI2.WinForms.Guna2Button opengamepath;
    }
}

