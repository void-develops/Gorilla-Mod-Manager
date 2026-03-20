using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gorilla_Mod_Manager
{
    public partial class Form1 : Form
    {
        private string GtagDirectory;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void gorillamodmanager_Click(object sender, EventArgs e)
        {
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Github_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/void-develops?tab=repositories");
        }

        public void OpenUrl(string url)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Link couldn't be opened: " + ex.Message);
            }
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
            {
                if (File.Exists(p))
                {
                    foundPath = p;
                    break;
                }
            }

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
                        {
                            MessageBox.Show("Invalid executable!", "Error");
                        }
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
                    {
                        MessageBox.Show(
                            "Sorry! That isn't the correct file for Gorilla Tag",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }
    }
}